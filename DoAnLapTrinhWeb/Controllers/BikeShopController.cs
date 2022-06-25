using DoAnLapTrinhWeb.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnLapTrinhWeb.Controllers
{
    public class BikeShopController : Controller
    {
        // GET: BikeShop
        BikeShopDataContext data = new BikeShopDataContext();
        // Hàm lấy n sản phẩm mới
        private List<SANPHAM> LaySanPham(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return data.SANPHAMs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        //Phương thức Index: Không có tham số (null) hoặc có tham số là số nguyên (biến page)
        public ActionResult Index(int? page, string tim)
        {
            //kích thước trang = số mẫu tin cho 1 trang
            int pagesize = 6;
            //Số thứ tự trang: nêu page là null thì pagenum = 1, ngược lại pagenum = page
            int pagenum = (page ?? 1);
            //Lấy top 5 bán chạy nhất;
            var sp = LaySanPham(15);
            return View(sp.ToPagedList(pagenum, pagesize));
        }
        public ActionResult LoaiSanPham()
        {
            var lsp = from s in data.LOAISANPHAMs select s;
            return PartialView(lsp);
        }

        public ActionResult SanPhamTheoLoai(int id)
        {
            var sp = from s in data.SANPHAMs where s.ID_LSP == id select s;
            return View(sp);

        }
        public ActionResult Brand()
        {
            var br = from s in data.BRANDs select s;
            return PartialView(br);
        }
        public ActionResult SanPhamTheoBrand(int id)
        {
            var sp = from s in data.SANPHAMs where s.ID_BRAND == id select s;
            return View(sp);

        }
        public ActionResult ChiTietSanPham(int id)
        {
            var sp = from s in data.SANPHAMs where s.ID_SANPHAM == id select s;
            //var i = from n in data.LOAISANPHAMs where n.ID_LSP == id select n.ID_LSP;
            //var sptheoloai = from x in data.SANPHAMs where x.ID_LSP = i select x;
            //ViewBag.sanphamtheoloai = sptheoloai;           
            return View(sp.Single());
        }
        public ActionResult ChiTietDonHang(int id)
        {
            return View(data.ChiTietDonHangs.Where(s => s.ID_DDH == id));

        }
        [Authorize]
        public ActionResult DonDatHang()
        {
            var userID = User.Identity.GetUserId();
            var ddh = from s in data.DONDATHANGs where s.ID_KH == userID select s;
            return View(ddh);
        }
    }
}