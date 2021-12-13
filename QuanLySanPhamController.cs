using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sport.Models;
using OfficeOpenXml;

namespace Sport.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        SportEntities2 db = new SportEntities2();

        public ActionResult Index()
        {

            return View(db.SanPhams.Where(n => n.DaXoa == 0).OrderByDescending(n => n.MaSP));
        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            //load dropdownlist nhà cung cấp VÀ dropdown listLoaisp,MÃ NSX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TennhaSX");

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TennhaSX");


            //Kiểm tra hình ảnh tồn tại trong csdl chưa
            if (HinhAnh[0].ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var filename = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), filename);
                //nếu thư mục chứa hình ảnh đó r xuất ra thông báo
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    //Lấy hình ảnh đưa vào thư mục hình ảnh sản phẩm
                    HinhAnh[0].SaveAs(path);
                    sp.HinhAnh = filename;

                }

            }




            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //load dropdownlist nhà cung cấp VÀ dropdown listLoaisp,MÃ NSX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP =
                new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TennhaSX", sp.MaNSX);
            return View(sp);

        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",
                model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TennhaSX", model.MaNSX);
            //Nếu dữ liệu đầu vào ok
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //load dropdownlist nhà cung cấp VÀ dropdown listLoaisp,MÃ NSX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP =
                new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSXes.OrderBy(n => n.MaNSX), "MaNSX", "TennhaSX", sp.MaNSX);
            return View(sp);
        }


        [HttpPost]
        public ActionResult Xoa(int id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }

            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        //    public ActionResult ExportData()
        //    {
        //        FileInfo fileInfoTemplate = new FileInfo(Server.MapPath(@"~\ExportExcelFile\ExcelSP"));
        //        var excelPkgTemplate = new ExcelPackage(fileInfoTemplate);
        //        string fileName = "Data_Export_Products" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
        //        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        var query = @"SELECT * FROM (SELECT CS.id
        //                            ,CS.hinh_anh
        //                            ,CS.ten_sp
        //                            ,CS.gia_sp
        //                            ,CS.ma_loai_tien
        //                            ,PS.gia_tri AS ten_loai_tien
        //                            ,CS.noi_dung_nho
        //                            ,CS.noi_dung_lon
        //                            ,CS.tinh_trang_kho_hang
        //                         ,PB.gia_tri AS ten_kho_hang
        //                            ,CS.ngay_tao
        //                            ,CS.nguoi_tao
        //                            ,US.ten_nguoi_dung AS ten_nguoi_tao
        //                            ,CS.ngay_cap_nhat
        //                            ,CS.nguoi_cap_nhat
        //                            ,CS.trang_thai
        //                            ,PA.gia_tri as ten_trang_thai
        //                            ,US2.ten_nguoi_dung AS ten_nguoi_sua
        //                            FROM [Products] CS
        //                            LEFT JOIN [User] US ON US.ma_nguoi_dung = CS.nguoi_tao  
        //                            LEFT JOIN [User] US2 ON US2.ma_nguoi_dung = CS.nguoi_cap_nhat
        //                            LEFT JOIN [Parameters] PS ON PS.ma_tham_so = CS.ma_loai_tien AND PS.loai_tham_so = 'LOAITIEN'
        //                            LEFT JOIN [Parameters] PA ON PA.ma_tham_so = CS.trang_thai AND PA.loai_tham_so = 'TRANGTHAISP'
        //                            LEFT JOIN [Parameters] PB ON PB.ma_tham_so = CS.tinh_trang_kho_hang AND PB.loai_tham_so = 'ST_WAREHOUSE') Data";

        //         List Data
        //        var data = dbConn.Select<SanPham>(query);
        //        int rowData = 2;
        //        ExcelWorksheet Sheet = excelPkgTemplate.Workbook.Worksheets["Data"];
        //        foreach (var item in data)
        //        {
        //            rowData++;
        //            Sheet.Cells[rowData, 1].Value = item.ten_sp;
        //            Sheet.Cells[rowData, 2].Value = item.dia_chi_khach_hang;
        //            Sheet.Cells[rowData, 2].Value = item.gia_sp;
        //            Sheet.Cells[rowData, 2].Style.Numberformat.Format = "#,###";
        //            Sheet.Cells[rowData, 3].Value = item.ten_loai_tien;
        //            Sheet.Cells[rowData, 4].Value = item.ten_kho_hang;
        //            Sheet.Cells[rowData, 5].Value = item.ngay_tao;
        //            Sheet.Cells[rowData, 5].Value = item.ngay_tao != new DateTime() ? item.ngay_tao.ToString("dd/MM/yyyy HH:mm") : "";
        //            Sheet.Cells[rowData, 7].Value = item.ma_can_bo;
        //            if (item.trang_thai == "DADUYET")
        //            {
        //                nameTT = "Đã duyệt";
        //            }
        //            else if (item.trang_thai == "DAHUY")
        //            {
        //                nameTT = "Đã hủy";
        //            }
        //            Sheet.Cells[rowData, 6].Value = nameTT;
        //            Sheet.Cells[rowData, 6].Value = item.ngay_cap_nhat;
        //            Sheet.Cells[rowData, 6].Value = item.ngay_cap_nhat != new DateTime() ? item.ngay_cap_nhat.ToString("dd/MM/yyyy HH:mm") : "";
        //        }
        //        MemoryStream output = new MemoryStream();
        //        excelPkgTemplate.SaveAs(output);
        //        output.Position = 0;
        //        return File(output.ToArray(), contentType, fileName);
        //    }
        //}


    }
}
