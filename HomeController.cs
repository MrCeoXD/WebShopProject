using CaptchaMvc.HtmlHelpers;
using Sport.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Sport.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        SportEntities2 db = new SportEntities2();

        public ActionResult Index()
        {
            //giay đá banh mới nhất
            var lstGDB = db.SanPhams.Where(n => n.MaLoaiSP == 1 );
            //Gán vào viewbag
            ViewBag.listGDB = lstGDB;
            //quần áo thể thao mới nhất
            var lstATT = db.SanPhams.Where(n => n.MaLoaiSP == 2 );
            //Gán vào viewbag
            ViewBag.listATT = lstATT;
            //giày thể thao mới nhất
            var lstGTT = db.SanPhams.Where(n => n.MaLoaiSP == 3 );
            //Gán vào viewbag
            ViewBag.listGTT = lstGTT;
            return View();
        }

        public ActionResult MenuPartial()
        {
            //Truy vấn lấy về 1 list sản phẩm
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }


        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();

        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv, FormCollection f)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //Kiểm tra captcha hợp lệ
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.ThongBao = "Thêm Thành Công";
                //Thêm khách hàng vào csdl
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                return View();
            }

            ViewBag.ThongBao = "Sai mã Captcha";
            return View();
        }

        //Load câu hỏi bí mật
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Đội bóng đá nam mà bạn yêu thích?");
            lstCauHoi.Add("Bạn có đẹp trai không");
            lstCauHoi.Add("Bạn tên gì");
            return lstCauHoi;
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();

        }

        [HttpPost]
        public ActionResult DangNhap(ThanhVien tv ,FormCollection f)
        {
            //Kiểm tra tên đăng nhập và mật khẩu
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }



    }
}