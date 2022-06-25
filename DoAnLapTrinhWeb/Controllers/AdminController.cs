using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnLapTrinhWeb.Models;
using PagedList;
namespace DoAnLapTrinhWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        BikeShopDataContext data = new BikeShopDataContext();
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var tendn = f["txtuser"];
            var matkhau = f["txtpass"];
            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            else
            {
                var ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
            }

            return View();
        }
        //1. Hiện thị danh sách các sản phẩm
        public ActionResult SanPham(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 7;
                //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
                int pagenum = (page ?? 1);
                return View(data.SANPHAMs.ToList().OrderByDescending(n => n.ID_SANPHAM).ToPagedList(pagenum, pagesize));
            }
        }
        //2. Xem chi tiết sản phẩm
        public ActionResult ChiTietSanPham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sp = from s in data.SANPHAMs where s.ID_SANPHAM == id select s;
                return View(sp.SingleOrDefault());
            }
        }
        //3. Xóa 1 quyển sach: Hiện thị trang thông tin chi tiết sản phẩm cần xóa, sau đó xác nhận xóa.
        [HttpGet]
        public ActionResult XoaSanPham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from s in data.SANPHAMs where s.ID_SANPHAM == id select s;
                return View(sach.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("XoaSanPham")]
        public ActionResult XacNhanXoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.ID_SANPHAM == id);
                data.SANPHAMs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("SanPham", "Admin");
            }
        }
        //4. Thêm mới 1 sản phẩm: Hiện thị view để thêm mới, sau đó Lưu 
        [HttpGet]
        public ActionResult ThemMoiSanPham()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ViewBag.ID_LSP = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.TenLoai), "ID_LSP", "TenLoai");
                ViewBag.ID_BRAND = new SelectList(data.BRANDs.ToList().OrderBy(n => n.Brand1), "ID_BRAND", "Brand1");
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiSanPham(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/HinhSanPham"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        sp.Anh = fileName;
                        //Luu vao CSDL
                        data.SANPHAMs.InsertOnSubmit(sp);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("SanPham", "Admin");
                }
            }
        }




        //5 Điều chỉnh thông tin sản phẩm
        public ActionResult SuaSanPham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.ID_SANPHAM == id);
                //Lay du liệu tư table LOAISANPHAM để đổ vào Dropdownlist, kèm theo chọn ID_SANPHAM tương tưng 
                ViewBag.ID_LSP = new SelectList(data.LOAISANPHAMs.ToList().OrderBy(n => n.TenLoai), "ID_LSP", "TenLoai", sp.ID_LSP);
                ViewBag.ID_BRAND = new SelectList(data.BRANDs.ToList().OrderBy(n => n.Brand1), "ID_BRAND", "Brand1", sp.ID_BRAND);
                return View(sp);
            }
        }
        [HttpPost, ActionName("SuaSanPham")]
        public ActionResult XacNhanSua(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.ID_SANPHAM == id);
                UpdateModel(sp);
                data.SubmitChanges();
                return RedirectToAction("SanPham", "Admin");
            }
        }
    }
}