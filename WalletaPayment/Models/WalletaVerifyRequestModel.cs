namespace Walleta.Cpg.SampleCode.WalletaPayment.Models
{

    public class WalletaVerifyRequestModel
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantCode">کد پذیرنده</param>
        /// <param name="token">توکن پرداخت</param>
        /// <param name="invoiceReference">شماره فاکتور</param>
        /// <param name="invoiceAmount">مبلغ قابل پرداخت فاکتور (تومان)</param>
        public WalletaVerifyRequestModel(string merchantCode, string token, string invoiceReference, int invoiceAmount)
        {
            this.merchant_code = merchantCode;
            this.token = token;
            this.invoice_reference = invoiceReference;
            this.invoice_amount = invoiceAmount;
        }

        public string merchant_code { get; set; }
        public string token { get; set; }
        public string invoice_reference { get; set; }
        public int invoice_amount { get; set; }
    }
}