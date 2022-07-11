using BusinessObject.BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IHostelPicRepository
    {
        Task AddHostelPic(HostelPic hostelPic);
        Task<IEnumerable<HostelPic>> GetHostelPicsOfAHostel(int hostelId);
        Task DeleteHostelPic(HostelPic hostelPic);
        Task<HostelPic> GetHostelPic(int id);
    }
}
