using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class HostelPicDAO
    {
        private static HostelPicDAO instance = null;
        private static readonly object instanceLock = new object();
        public static HostelPicDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new HostelPicDAO();
                    }
                    return instance;
                }
            }
        }
        private HostelPicDAO() { }
        public async Task AddHostelPic(HostelPic hostelPic)
        {
            try
            {
                hostelPic.HostelPicsId = 0;
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.Attach(hostelPic).State = EntityState.Added;
                await HostelManagementContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [HostelManagement].[dbo].[HostelPics] ON"); 
                await HostelManagementContext.SaveChangesAsync();
                await HostelManagementContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [HostelManagement].[dbo].[HostelPics] OFF");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<HostelPic>> GetHostelPicsOfAHostel(int hostelId)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                return await HostelManagementContext.HostelPics
                    .Include(h => h.Hostel)
                    .Where(h => h.HostelId == hostelId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteHostelPic(HostelPic hostelPic)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                HostelManagementContext.HostelPics.Remove(hostelPic);
                await HostelManagementContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HostelPic> GetHostelPic(int id)
        {
            try
            {
                var HostelManagementContext = new HostelManagementContext();
                var pic = await HostelManagementContext.HostelPics.SingleOrDefaultAsync(hp => hp.HostelPicsId.Equals(id));
                return pic;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
