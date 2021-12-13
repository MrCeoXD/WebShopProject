using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sport.Models;
namespace Sport.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        SportEntities2 db = new SportEntities2();
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Lấy số lượng người truy cập
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiDangOnline"].ToString();//Số người đang onl
            ViewBag.TongDoanhThu = ThongKeDoangThu(); //Doanh thu của cửa hàng
            ViewBag.TongDDH= ThongKeDonHang();  //Thống kê số lượng đơn hàng
            ViewBag.TongThanhVien=  ThongKeThanhVien(); //Thống kê số lượng thành viên
            return View();
        }

        public double ThongKeDonHang()
        {
            //đếm đơn hàng
            double ddh = db.DonDatHangs.Count();
            return ddh;
        }

        public double ThongKeThanhVien()
        {
            //đếm thành viên
            double tv = db.ThanhViens.Count();
            return tv;
        }
        public decimal ThongKeDoangThu()
        {
            //Thống kê doanh thu cửa hàng
            decimal TongDoanhThu = db.ChiTietDonDatHangs.Sum(n  => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }

        public decimal ThongKeDoangThuThang(int Thang, int Nam)
        {
            //Thống kê doanh thu cửa hàng
            //List ra những đơn hàng nào có tháng, năm giống nhau
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            decimal TongTien = 0;
            //Duyệt chitietddh và lấy tổng tiền của đơn đặt hàng đó
            foreach (var item in lstDDH)
            {
                TongTien+=decimal.Parse(item.ChiTietDonDatHangs.Sum(n=>n.SoLuong*n.DonGia).Value.ToString());

            }

            return TongTien;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (disposing)
                {
                    if (db!=null)
                    {
                        db.Dispose();
                        db.Dispose();
                    }
                    base.Dispose(disposing);
                }
            }
        }

    }
}