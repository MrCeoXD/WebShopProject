using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sport.Models;
using System.Net.Mail;
namespace Sport.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        SportEntities2 db = new SportEntities2();
        public ActionResult ChuaThanhToan()
        {
            //Lấy danh sách đơn hàng chưa duyệt
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false && n.TinhTranDonHang == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }

        public ActionResult ChuaGiao()
        {
            //Lấy danh sách đơn hàng chưa giao
            var lstDHCG = db.DonDatHangs.Where(n => n.DaThanhToan == true && n.TinhTranDonHang == false).OrderBy(n => n.NgayDat);
            return View(lstDHCG);
        }

        public ActionResult DaGiaoDaThanhToan()
        {
            //Lấy danh sách đơn hàng chưa giao
            var lstDHDG = db.DonDatHangs.Where(n => n.TinhTranDonHang == true && n.DaThanhToan == true).OrderBy(n => n.NgayDat);
            return View(lstDHDG);
        }

        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //Kiem tra id hợp lệ không        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //Kiểm tra đơn hàng
            if (model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sách đơn hàng để hiển thị cho người dùng
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            //Truy vấn lấy dữ liệu đơn hàng
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTranDonHang = ddh.TinhTranDonHang;
            db.SaveChanges();
            //lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            //Gửi khách hàng mail để xác nhận việc thanh toán
            GuiEmail("Xác nhận đơn hàng ChauAndHoai SportStore"
                , "18110084@student.hcmute.edu.vn",
                "chau18110084@gmail.com", "chau0804",
                "Đơn hàng của bạn được đặt");
            return View(ddhUpdate);
        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string Passwd, string Content)
        {
            //Gởi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);//Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); //địa chỉ gửi
            mail.Subject = Title;//Tiêu đề
            mail.Body = Content;//Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";//Host gửi email
            smtp.Port = 587;    //Port của gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(FromEmail, Passwd);//Tài khoản password người gửi
            smtp.EnableSsl = true;//Kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail);//Gửi mail
        }

    }
}