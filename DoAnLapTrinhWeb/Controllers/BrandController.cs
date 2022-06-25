using DoAnLapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnLapTrinhWeb.Controllers
{
    public class BrandController : Controller
    {
        // GET: Brand
        BikeShopDataContext data = new BikeShopDataContext();
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(data.BRANDs.ToList());
        }
        //2. Hiện thi chi tiết 1 loại sản phẩm
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var lsp = from sp in data.BRANDs where sp.ID_BRAND == id select sp;
                return View(lsp.SingleOrDefault());
            }
        }
        //3. Thêm mới loại sản phẩm
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Create(BRAND lsp)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.BRANDs.InsertOnSubmit(lsp);
                data.SubmitChanges();

                return RedirectToAction("Index", "Brand");
            }
        }
        //4. Xóa 1 loại sản phẩm gồm 2 trang: xác nhận xóa và xử lý xóa
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var lsp = from s in data.BRANDs where s.ID_BRAND == id select s;
                return View(lsp.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                BRAND lsp = data.BRANDs.SingleOrDefault(n => n.ID_BRAND == id);
                foreach (var item in data.SANPHAMs)
                {
                    if (item.ID_BRAND == id)
                    {
                        data.SANPHAMs.DeleteOnSubmit(item);
                        data.SubmitChanges();
                    }
                }
                data.BRANDs.DeleteOnSubmit(lsp);
                data.SubmitChanges();

                return RedirectToAction("Index", "Brand");
            }
        }
        //5. Điều chỉnh thông tin 1 loại sản phẩm gồm 2 trang: Xem và điều chỉnh và cập nhật lưu lại
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var lsp = from s in data.BRANDs where s.ID_BRAND == id select s;
                return View(lsp.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                BRAND lsp = data.BRANDs.SingleOrDefault(n => n.ID_BRAND == id);

                UpdateModel(lsp);
                data.SubmitChanges();
                return RedirectToAction("Index", "Brand");
            }
        }
    }
}