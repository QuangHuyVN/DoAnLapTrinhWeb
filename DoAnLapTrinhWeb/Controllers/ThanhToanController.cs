using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using DoAnLapTrinhWeb.Model;

namespace DoAnLapTrinhWeb.Controllers
{
    public class ThanhToanController
    {
        BikeShopDataContext data = new BikeShopDataContext();
        // GET: ThanhToan
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Payment(int id)
        {
            DONDATHANG ddh = data.DONDATHANGs.SingleOrDefault(p => p.ID_DDH == id);

            string partnerCode = "MOMO40UL20220620";
            string accessKey = "4gX01slnOM0LtKGe";
            string serectkey = "wsBE4zQc6fY58OuJUOoPSykjSZfa0SmM"; //
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            // 4 đoạn mã trên lấy trong phần liên kết với Website trên Business.Momo.vn
            string orderInfo = "Thanh toán đơn hàng: " + ddh.ID_DDH.ToString(); //Nội dung thanh toán
            string returnUrl = "https://localhost:44308/ThanhToan/ConfirmPaymentClient";
            string notifyurl = "http://ba1adf48beba.ngrok.io/ThanToan/SavePayment";
            string amount = ddh.TongTien.ToString(); //Số tiền cần thanh toán
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Trước khi ký Chữ ký HMAC SHA256
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //Ký chữ ký SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //Yêu cầu xây dựng body json
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        public ActionResult ConfirmPaymentClient()
        {
            //hiển thị thông báo cho người dùng
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
        }
    }
}