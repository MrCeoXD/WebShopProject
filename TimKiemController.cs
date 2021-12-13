using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sport.Models;
using PagedList;
namespace Sport.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        SportEntities2 db=new SportEntities2();
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //Tạo biến số sản phẩm  trên trang
            int pageSize = 6;
            //Tạo biến thứ 2:Số trang hiện tại
            int pageNumber = (page ?? 1);
            //Tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(pageNumber,pageSize));
        }

        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa, int? page,FormCollection f)
        {
            //Gọi về hàm get tìm kiếm
            return RedirectToAction("KQTimKiem",new {@sTuKhoa=sTuKhoa});
        }

        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            //Tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP.OrderBy(n => n.DonGia));
        }
    }
}