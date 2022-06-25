using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class GioHang
    {
        BikeShopDataContext data = new BikeShopDataContext();
        public int iIdsanpham { set; get; }
        public string sTensp { set; get; }
        public string sAnh { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1
        public GioHang(int ID_SANPHAM)
        {
            iIdsanpham = ID_SANPHAM;
            SANPHAM sp = data.SANPHAMs.Single(n => n.ID_SANPHAM == iIdsanpham);
            sTensp = sp.TenSP;
            sAnh = sp.Anh;
            dDongia = double.Parse(sp.GiaBan.ToString());
            iSoluong = 1;
        }
    }
}