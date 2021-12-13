using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sport.Models;

namespace Sport.Controllers
{
    public class GioHangController : Controller
    {
        SportEntities2 db = new SportEntities2();
        //Lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            //Gio hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //neu session gio hang chưa tồn tại thì tạo mới giỏ hàng
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;

            }
            //THêm giỏ hàng
            return lstGioHang;

        }

        //THêm giỏ hàng thông thường(Load lại trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Trường hợp 1 nếu sản phẩm không tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //Kiểm tra số lượng tồn trc khi mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }

            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }

        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }

            return lstGioHang.Sum(n => n.SoLuong);
        }
        //Tính tổng số lượng
        public decimal TinhTongTien()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }

            return lstGioHang.Sum(n => n.ThanhTien);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }

            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        // GET: GioHang
        public ActionResult XemGioHang()
        {
            //Lấy giỏ hàng 
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }


        //Chỉnh sửa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kIỂM TRA SP có tồn tại trong CSDL KHÔNG
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong giỏ hàng không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            //Nếu tồn tại rồi
            return View(spCheck);
        }
        //Xử lí cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.SingleOrDefault(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //Cập nhật số lượng trong session giỏ hàng
            //Bước 1: Lấy list<Giỏ Hàng> từ session
            List<ItemGioHang> lstGH = LayGioHang();
            //Lấy sp cần cập nhật từ trong list giỏ hàng ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //Cập nhật lại thông tin
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trang sản phẩm không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra xem sản phẩm đã có tồn tại trong giỏ hàng hay chưa
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xoá item trong giỏ hàng
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        //Xây dựng chức năng đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            KhachHang Khang=new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                //Thêm Khách Hàng vào bảng khách hàng đối với khách hàng vãng lai
                Khang = kh;
                db.KhachHangs.Add(Khang);
                db.SaveChanges();
            }
            else
            {
                //đối vs khách hàng là thành viên
                ThanhVien tv =Session["TaiKhoan"] as ThanhVien;
                Khang.TenKH = tv.HoTen;
                Khang.DiaChi = tv.DiaChi;
                Khang.Email = tv.Email;
                Khang.SDT = tv.SDT;
                Khang.Mathanhvien = tv.MaLoaiTV;
                db.KhachHangs.Add(Khang);
                db.SaveChanges();
            }
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = Khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTranDonHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            //ddh.DaHuy = false;
            //ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();//Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }

            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }

        //Thêm giỏ hàng ajax
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return Content("<script> alert (\"Sản phẩm đã hết hàng\")</script>");
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Trường hợp 1 nếu sản phẩm không tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //Kiểm tra số lượng tồn trc khi mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }

            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }
         
    }
}