using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Walleta.Cpg.SampleCode.WalletaPayment;
using Walleta.Cpg.SampleCode.WalletaPayment.Models;
using System.Configuration;
using Walleta.Cpg.SampleCode.Models;
using System.Collections.Concurrent;
using System.Web.Script.Serialization;

namespace Walleta.Cpg.SampleCode.Controllers
{
    public class HomeController : Controller
    {

        private WalletaPaymentService WalletaPaymentService { get; }
        private string WalletaBaseUrl { get; }
        private string MerchantCode { get; }
        private string CallBack { get; }
        private string CpgUrl { get; }

        public HomeController()
        {
            this.WalletaBaseUrl = ConfigurationManager.AppSettings["Walleta_BaseUrl"];
            this.MerchantCode = ConfigurationManager.AppSettings["Walleta_MerchantCode"];
            this.CallBack = ConfigurationManager.AppSettings["Walleta_CallBack"];
            this.CpgUrl = ConfigurationManager.AppSettings["Walleta_CpgUrl"];

            this.WalletaPaymentService = new WalletaPaymentService(WalletaBaseUrl);
        }

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            var model = new SimplePayModel
            {
                Amount = 10000,
                Mobile = "09361541094",
                NationalCode = "0078624533"
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SimplePayModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string invoiceReference = DateTime.Now.Ticks.ToString();
            var tokenRequest = new WalletaTokenRequestModel
                (
                    MerchantCode,
                    invoiceReference,
                    DateTime.Now,
                    model.Amount,
                    "نام",
                    "نام خانوادگی",
                    model.NationalCode,
                    model.Mobile,
                    CallBack
                );


            tokenRequest.items = new Item[]
            {
                new Item
                {
                    reference = "PK-0001",
                    name = "کالای تست ۱",
                    quantity = 2,
                    unit_price = model.Amount / 2,
                    unit_discount = 0,
                    unit_tax_amount = 0,
                    total_amount = model.Amount,
                },
                new Item
                {
                    name = "هزینه ارسال",
                    quantity = 1,
                    unit_price = 10000,
                    unit_discount = 0,
                    unit_tax_amount = 0,
                    total_amount = 10000,
                }
            };

            var result = WalletaPaymentService.GetToken(tokenRequest);

            if(result == null)
            {
                ModelState.AddModelError("خطای سرویس", "پاسخی از سمت سرویس دریافت نشد");
                return View(model);
            }

            if (result.invalid_fields != null && result.invalid_fields.Length > 0)
            {
                foreach (var field in result.invalid_fields)
                {
                    ModelState.AddModelError(field.field, field.field + ": " +field.message);
                }

                return View(model);
            }

            if (string.IsNullOrWhiteSpace(result.token))
            {
                ModelState.AddModelError(result.type, result.message);
                return View(model);
            }

            SaveReference(result.token, invoiceReference, model.Amount);
            return Redirect(CpgUrl.TrimEnd('/') + "/" + result.token);
            
        }

        [HttpGet]
        public ActionResult PaymentResult(string status, string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    ModelState.AddModelError("ServerError", "پاسخی از سمت درگاه دریافت نشد");
                    return View();
                }

                if (status != "success")
                {
                    ModelState.AddModelError("ServerError", status);
                    return View();
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    ModelState.AddModelError("ServerError", "توکن از سمت سرور ارسال نشد");
                    return View();
                }

                int amount;
                string invoiceReference = GetReference(token, out amount);

                if (string.IsNullOrWhiteSpace(invoiceReference))
                {
                    ModelState.AddModelError("ServerError", "شماره فاکتور یافت نشد");
                    return View();
                }

                var verifyRequest = new WalletaVerifyRequestModel(MerchantCode, token, invoiceReference, amount);

                var verifyResult = WalletaPaymentService.VerifyPayment(verifyRequest);

                if (verifyResult == null)
                {
                    ModelState.AddModelError("ServerError", "پاسخ تایید تراکنش از سمت درگاه دریافت نشد");
                    return View();
                }

                ViewData["VerifyJsonResponse"] = new JavaScriptSerializer().Serialize(verifyResult);
                return View(verifyResult);
            }
            finally
            {
                RemoveReferece(token);
            }
        }





        #region ' Token References '


        private static readonly ConcurrentDictionary<string, string> TokenReferences = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// جهت پیدا کردن شماره تراکنش سمت پذیرنده پس از بازگشت از صفحه پرداخت
        /// بهتر است در دیتابیس ذخیره گردد
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="token"></param>
        private void SaveReference(string token, string reference, int amount)
        {
            TokenReferences.TryAdd(token, reference + "|" + amount);
        }


        /// <summary>
        /// جهت پیدا کردن شماره تراکنش سمت پذیرنده پس از بازگشت از صفحه پرداخت
        /// بهتر است در دیتابیس ذخیره گردد
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="token"></param>
        private string GetReference(string token, out int amount)
        {
            string reference;
            amount = 0;

            if(token == null)
            {
                return "";
            }

            TokenReferences.TryGetValue(token, out reference);

            if (string.IsNullOrWhiteSpace(reference))
            {
                return "";
            }

            string[] referenceArray = reference.Split('|');

            if(referenceArray != null && referenceArray.Length == 2)
            {
                reference = referenceArray[0];
                int.TryParse(referenceArray[1], out amount);
            }

            return reference;
        }

        private void RemoveReferece(string token)
        {
            string x;
            if(token != null)
                TokenReferences.TryRemove(token, out x);
        }

        #endregion
    }
}