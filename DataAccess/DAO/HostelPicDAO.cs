using BusinessObject.BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                int a = 3;
                await HostelManagementContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [HostelManagement].[dbo].[HostelPics] ON");
                await HostelManagementContext.SaveChangesAsync();
                await HostelManagementContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [HostelManagement].[dbo].[HostelPics] OFF");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
