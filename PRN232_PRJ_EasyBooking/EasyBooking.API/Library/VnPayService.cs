using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using EasyBooking.Business.Library;

namespace EasyBooking.Business.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestDto model);
        VnPaymentResponseDto PaymentExecute(IQueryCollection collection);
        bool ValidateSignature(string secureHash, IQueryCollection collection);
    }
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestDto model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", model.Id);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", $"{model.Id}_{tick}");

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
            return paymentUrl;
        }

        public VnPaymentResponseDto PaymentExecute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var item in collection)
            {
                if (!string.IsNullOrEmpty(item.Key) && item.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(item.Key, item.Value.ToString());
                }
            }

            var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef").ToString();
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collection.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var vnp_Amount = vnpay.GetResponseData("vnp_Amount");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VNPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseDto
                {
                    Success = false
                };
            }

            return new VnPaymentResponseDto
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_orderId.ToString(),
                OrderId = vnp_OrderInfo,
                Amount = Convert.ToInt32(vnp_Amount) /100,
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayReponseCode = vnp_ResponseCode.ToString(),
            };
        }

        public bool ValidateSignature(string secureHash, IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var item in collection)
            {
                if (!string.IsNullOrEmpty(item.Key) && item.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(item.Key, item.Value.ToString());
                }
            }

            return vnpay.ValidateSignature(secureHash, _config["VNPay:HashSecret"]);
        }
    }
}
