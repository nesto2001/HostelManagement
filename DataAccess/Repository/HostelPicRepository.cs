using BusinessObject.BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class HostelPicRepository : IHostelPicRepository
    {
        public async Task AddHostelPic(HostelPic hostelPic) => await HostelPicDAO.Instance.AddHostelPic(hostelPic);
    }
}
