using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sport.Models;
using PagedList; 
namespace Sport.Controllers
{
    public class SanPhamController : Controller
    {
        SportEntities2 db = new SportEntities2();
        [ChildActionOnly]
        ////GET: SanPham
        //public ActionResult SanPham1()
        //{
        //    var lstSanPhamTheThao = db.SanPhams.Where(n => n.MaLoaiSP == 2);
        //    ViewBag.listSP = lstSanPhamTheThao;

        //    var lstSanPhamGiayDaBanh = db.SanPhams.Where(n => n.MaLoaiSP == 1);
        //    ViewBag.listGiay = lstSanPhamGiayDaBanh;
        //    return View();
        //}

        //public ActionResult SanPham2()
        //{
        //    var lstSanPhamTheThao = db.SanPhams.Where(n => n.MaLoaiSP == 2
        //                                                   || n.MaLoaiSP == 1 || n.MaLoaiSP == 3
        //                                                   || n.MaLoaiSP == 4);
        //    ViewBag.listSP = lstSanPhamTheThao;
        //    return View();
        //}
        //[ChildActionOnly]
        //public ActionResult SanPhamPartial()
        //{
        //    var lstSanPhamTheThao = db.SanPhams.Where(n => n.MaLoaiSP == 2
        //                                                   || n.MaLoaiSP == 1);
        //    return PartialView(lstSanPhamTheThao);
        //}

        //Tạo 2 partial view sản phẩm 1 và 2 để hiển thị sản phẩm theo style khác nhau
        
        public ActionResult SanPhamStyle1Parital()
        {

            return PartialView();
        }

        public ActionResult SanPhamStyle2Parital()
        {

            return PartialView();   
        }
        public ActionResult XemChiTiet(int? id, string tensp)
        {
            //Kiểm tra xem thông số truyền vào có rỗng không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Nếu không thì truy xuất csdl lấy ra sp tương ứng
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id&&n.DaXoa==0);
            if (sp == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }
            return View(sp);
        }
        //Xây dựng 1 action load sản phẩm theo mã loại sản phẩm và mã nhà sx
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int ?page)
        {

            if (MaLoaiSP== null|| MaNSX==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Load sản phẩm dựa theo 2 tiêu chi là Mã loại sản phẩm và mã nsx
            //2 trường trong bảng sản phẩm 
            var lstSP =db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            { 
                return HttpNotFound();
            }
            ////Thực hiện phân trang
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //Tạo biến số sản phẩm  trên trang
            int PageSize = 3;
            //Tạo biến thứ 2:Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber,PageSize)); 
        }

    }
}