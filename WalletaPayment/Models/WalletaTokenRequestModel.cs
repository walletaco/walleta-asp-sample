using System;

namespace Walleta.Cpg.SampleCode.WalletaPayment.Models
{
    public class WalletaTokenRequestModel
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantCode">کد پذیرنده</param>
        /// <param name="invoiceReference">شماره فاکتور</param>
        /// <param name="invoiceDate">تاریخ فاکتور طبق استاندارد</param>
        /// <param name="amount">مبلغ قابل پرداخت فاکتور (تومان)</param>
        /// <param name="payerFirstName">نام پرداخت کننده</param>
        /// <param name="payerLastName">نام خانوادگی پرداخت کننده</param>
        /// <param name="payerNationalCode">کد ملی پرداخت کننده</param>
        /// <param name="payerMobile">شماره موبایل پرداخت کننده</param>
        /// <param name="callbackUrl">آدرس صفحه برگشت</param>
        public WalletaTokenRequestModel
            (
            string merchantCode, 
            string invoiceReference,
            DateTime invoiceDate, 
            int amount, 
            string payerFirstName, 
            string payerLastName, 
            string payerNationalCode,
            string payerMobile,
            string callbackUrl)
        {
            this.merchant_code = merchantCode;
            this.invoice_reference = invoiceReference;
            this.invoice_date = invoiceDate.ToString("s");
            this.invoice_amount = amount;
            this.payer_first_name = payerFirstName;
            this.payer_last_name = payerLastName;
            this.payer_national_code = payerNationalCode;
            this.payer_mobile = payerMobile;
            this.callback_url = callbackUrl;
        }

        public string merchant_code { get; }
        public string invoice_reference { get; }
        public string invoice_date { get; }
        public long invoice_amount { get;}
        public string payer_first_name { get; }
        public string payer_last_name { get; }
        public string payer_national_code { get;}
        public string payer_mobile { get; }
        public string callback_url { get; }
        public string description { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string reference { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int unit_price { get; set; }
        public int unit_discount { get; set; }
        public int unit_tax_amount { get; set; }
        public int total_amount { get; set; }
    }
}