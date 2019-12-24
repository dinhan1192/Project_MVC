using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    interface ICRUDService<T>
    {
        bool Create(T item, ModelStateDictionary state);
        bool CreateWithImage(T item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images);
        bool Update(T existItem, T item, ModelStateDictionary state);
        bool Delete(T item, ModelStateDictionary state);
        T Detail(T item);
        void ValidateCode(T item, ModelStateDictionary state);
        void ValidateCategory(T item, ModelStateDictionary state);
    }
}
