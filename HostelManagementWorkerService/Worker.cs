using BusinessObject.BusinessObject;
using DataAccess.Repository;
using HostelManagement.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HostelManagementWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IRentRepository rentRepository;
        private IBillRepository billRepository;
        private IRoomRepository roomRepository;
        private IBillDetailRepository billDetailRepository;
        private ISendMailService sendMailService;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, IRentRepository _rentRepository, IServiceProvider serviceProvider,
            IBillRepository _billRepository, IBillDetailRepository _billDetailRepository, ISendMailService _sendMailService,
            IRoomRepository _roomRepository)
        {
            _logger = logger;
            rentRepository = _rentRepository;
            _serviceProvider = serviceProvider;
            billRepository = _billRepository;
            billDetailRepository = _billDetailRepository;
            sendMailService = _sendMailService;
            roomRepository = _roomRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var rents = await rentRepository.GetRentList();
                var rentDeposits = rents.Where(r => r.IsDeposited == 0 && r.Status == 0);
                foreach (var item in rentDeposits)
                {
                    if (item.StartRentDate < DateTime.Now.AddHours(-24))
                    {
                        item.Status = 4;
                        await rentRepository.UpdateRent(item);
                        Room _room = await roomRepository.GetRoomByID(item.RoomId);
                        _room.Status = 1;
                        _room.RoomCurrentCapacity = 0;
                        await roomRepository.UpdateRoom(_room);
                        _logger.LogInformation("The rent {0} of {1} is cancel at {2}", item.RentId, item.RentedBy, DateTime.Now);
                    }
                    
                }
                var rentWaitingStart = rents.Where(r => r.IsDeposited == 2 && r.Status == 0);
                foreach (var item in rentWaitingStart)
                {
                    if (item.StartRentDate.Date == DateTime.Now.Date)
                    {
                        item.Status = 1;
                        await rentRepository.UpdateRent(item);
                        string body = "Thank you for your service hostel renting. \n" +
                            "Your contract number " + item.RentId + "\n" +
                            " is started at " + DateTime.Now.ToString("dd-MM-yyyy") + " \n" +
                            "Thank you. \n" +
                            "Please send your feedback by reply mail.\n" +
                            "Best Regard,";
                        try
                        {
                            await sendMailService.SendEmailAsync(item.RentedBy, "Start of room", body);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation(ex.Message + "---" + ex);
                        }
                        _logger.LogInformation("The rent {0} of {1} is start at {2}", item.RentId, item.RentedBy, DateTime.Now);
                    }
                }
                var rentWorking = rents.Where(r => r.Status == 1);
                foreach (var item in rentWorking)
                {
                    DateTime lastBill = item.StartRentDate;
                    if (item.Bills.Count() != 0)
                    {
                        lastBill = (DateTime)item.Bills.Max(b => b.CreatedDate);
                    }
                    if (lastBill < DateTime.Now.AddDays(-35))
                    {
                        Bill bill = new Bill
                        {
                            CreatedDate = DateTime.Now,
                            DueDate = DateTime.Now.AddDays(7),
                            RentId = item.RentId,
                            RoomId = item.RoomId,
                            StartRentDate = item.StartRentDate,
                            EndRentDate = item.EndRentDate
                        };
                        await billRepository.AddBill(bill);
                        BillDetail billDetail = new BillDetail
                        {
                            BillId = bill.BillId,
                            BillDescription = "Room usage fee",
                            DateIssued = lastBill.AddMonths(1),
                            Fee = item.Total
                        };
                        await billDetailRepository.AddBillDetail(billDetail);
                        string body = "Thank you for your service hostel renting. \n" +
                            "Your bill is created at " + DateTime.Now + "\n" +
                            "Please pay bill before: " + bill.DueDate + "\n" +
                            "Thank you. \n" +
                            "Please send your feedback by reply mail.\n" +
                            "Best Regard,";
                        try {
                            await sendMailService.SendEmailAsync(item.RentedBy, "Bill for contract of room", body);
                        } catch (Exception ex)
                        {
                            _logger.LogInformation(ex.Message + "---" + ex);
                        }
                        
                        _logger.LogInformation("The rent {0} of {1} is created a bill at {2}", item.RentId, item.RentedBy, DateTime.Now);
                    }
                    if (item.EndRentDate.Date == DateTime.Now.AddDays(7).Date)
                    {
                        string body = "Thank you for your service hostel renting. \n" +
                            "Your contract will end at " + item.EndRentDate.ToString("dd-MM-yyyy") + "\n" +
                            "The contract number is " + item.RentId + "\n" +
                            "Please extend your bill before: " + item.EndRentDate.AddDays(-3).ToString("dd-MM-yyyy") + "\n" +
                            "Thank you. \n" +
                            "If you don't extend ontime, the same as you won't rent this room after end date of contract. \n " +
                            "Please send your feedback by reply mail.\n" +
                            "Best Regard,";
                        try
                        {
                            await sendMailService.SendEmailAsync(item.RentedBy, "Remind extend your contract ", body);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation(ex.Message + "---" + ex);
                        }

                        _logger.LogInformation("Remind extend contract {0} of {1} is created at {2}", item.RentId, item.RentedBy, DateTime.Now);
                    }
                    if (item.EndRentDate.Date == DateTime.Now.AddDays(2).Date)
                    {
                        item.Status = 2;
                        var room = await roomRepository.GetRoomByID(item.RoomId);
                        if (room != null)
                        {
                            room.Status = 1;
                            await roomRepository.UpdateRoom(room);
                        }
                        await rentRepository.UpdateRent(item);
                        string body = "Thank you for your service hostel renting. \n" +
                            "Your contract will end at " + item.EndRentDate.ToString("dd-MM-yyyy") + "\n" +
                            "The contract number is " + item.RentId + "\n" +
                            "Thank you. \n" +
                            "You didn't extend your contract, the same as you won't rent this room after end date of contract. \n " +
                            "Please send your feedback by reply mail.\n" +
                            "Best Regard,";
                        try
                        {
                            await sendMailService.SendEmailAsync(item.RentedBy, "Confirm did not extend your contract ", body);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation(ex.Message + "---" + ex);
                        }

                        _logger.LogInformation("Confirm did not extend contract {0} of {1} is created at {2}", item.RentId, item.RentedBy, DateTime.Now);
                    }
                }
                var rentMustStop = rents.Where(r => r.Status == 2 || r.Status == 5);
                foreach (var item in rentMustStop)
                {
                    if (item.EndRentDate.Date == DateTime.Now.Date)
                    {
                        item.Status = 3;
                        await rentRepository.UpdateRent(item);
                        Room room = await roomRepository.GetRoomByID(item.RoomId);
                        room.Status = 1;
                        room.RoomCurrentCapacity = 0;
                        await roomRepository.UpdateRoom(room);
                        string body = "Thank you for your service hostel renting. \n" +
                            "Your contract number " + item.RentId + "\n" +
                            " is ended at " + DateTime.Now.ToString("dd-MM-yyyy") + " \n" +
                            "Thank you. \n" +
                            "Please send your feedback by reply mail.\n" +
                            "Best Regard,";
                        try
                        {
                            await sendMailService.SendEmailAsync(item.RentedBy, "End contract of room", body);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation(ex.Message + "---" + ex);
                        }
                        _logger.LogInformation("The rent {0} of {1} is end at {2}", item.RentId, item.RentedBy, DateTime.Now);
                    }
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
