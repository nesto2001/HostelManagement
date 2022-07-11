using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class RentDAO
    {
        private static RentDAO instance = null;
        private static readonly object instanceLock = new object();
        public static RentDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RentDAO();
                    }
                    return instance;
                }
            }
        }
        private RentDAO() { }
        public async Task AddRent(Rent Rent)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(Rent).State = EntityState.Added;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Rent>> GetRentListByRoom(int roomId)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Rents
                        .Include(r => r.Room)
                        .Include(r => r.RentedByNavigation)
                        .Include(r => r.RoomMembers)
                        .Include(r => r.Bills)
                        .Where(r => r.RoomId == roomId)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Rent>> GetRentList()
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Rents
                        .Include(r => r.Room)
                            .ThenInclude(r => r.Hostel)
                                .ThenInclude(r => r.HostelOwnerEmailNavigation)
                        .Include(r => r.RentedByNavigation)
                        .Include(r => r.RoomMembers)
                        .Include(r => r.Bills.OrderByDescending(b => b.CreatedDate))
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Rent> GetRentByID(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.Rents
                    .Include(r => r.Room)
                            .ThenInclude(r => r.Hostel)
                                .ThenInclude(r => r.HostelOwnerEmailNavigation)
                    .Include(r => r.RentedByNavigation)
                    .Include(r => r.RoomMembers)
                    .Include(r => r.Bills.OrderByDescending(b => b.CreatedDate))
                        .ThenInclude(r => r.BillDetails)
                    .FirstOrDefaultAsync(r => r.RentId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRent(Rent rent)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(rent).State = EntityState.Modified;
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
