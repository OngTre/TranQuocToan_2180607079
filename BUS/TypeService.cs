using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entitis;
namespace BUS
{
    public class TypeService
    {
        public static List<Loai> GetAll()
        {
            ProductModel context = new ProductModel();
            return context.Loais.ToList();
        }
    }
}
