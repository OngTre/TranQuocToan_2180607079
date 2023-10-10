using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAL.Entitis;
namespace BUS
{
    public class ProductService
    {
        public static List<SanPham> GetAll()
        {
            ProductModel context = new ProductModel();
            return context.SanPhams.ToList();
        }


        public List<SanPham> GetAllHasMajor()
        {
            ProductModel context = new ProductModel();
            return context.SanPhams.Where(p => p.MaLoai == null).ToList();
        }

       


        public SanPham FindById(string productID)
        {
            ProductModel context = new ProductModel();
            return context.SanPhams.FirstOrDefault(p => p.MaSP == productID);

        }

        public static void InsertUpdate(SanPham s)
        {
            ProductModel context = new ProductModel();
            context.SanPhams.AddOrUpdate(s);
            context.SaveChanges();
        }

        public static void UpdateSanpham(string maSP, string tenSP, string ngayNhap, string tenLoai)
        {
            using (var context = new ProductModel())
            {
                var product = context.SanPhams.FirstOrDefault(sp => sp.MaSP == maSP);
                if (product != null)
                {

                    product.TenSP = tenSP;
                    product.Ngaynhap = DateTime.Parse(ngayNhap);
                    product.Loai.TenLoai = tenLoai;

                    context.SaveChanges();
                }
            }
        }

        
        public static List<SanPham> SearchSanphamByTenSP(string tenSP )
        {
            using (var context = new ProductModel())
            {
                return context.SanPhams.Where(s => s.TenSP.Contains(tenSP)).ToList();
            }
        }
        

       

    }
}
