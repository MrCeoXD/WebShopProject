using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sport.Models  
{
    public class ItemGioHang
    {
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }

        
        public ItemGioHang(int iMaSp)
        {
            using (SportEntities2 db = new SportEntities2())
            {
                this.MaSP = iMaSp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSp);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;

            }
        }
        public ItemGioHang(int iMaSp, int sl)
        {
            using (SportEntities2 db = new SportEntities2())
            {
                this.MaSP = iMaSp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSp);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;

            }
        }
        public ItemGioHang()
        {

        }

    }
}