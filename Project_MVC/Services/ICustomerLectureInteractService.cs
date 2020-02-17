using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_MVC.Services
{
    interface ICustomerLectureInteractService
    {
        void CreateRating(decimal rating, int? lectureId, string userId);
        void UpdateRating(decimal rating, int? customerLectureInteractId);
        CustomerLectureInteract DetailByLectureIdAndUserId(int? lectureId);
        List<CustomerLectureInteract> GetListByLectureId(int? lectureId);
    }
}
