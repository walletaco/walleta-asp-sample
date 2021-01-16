namespace Walleta.Cpg.SampleCode.WalletaPayment.Models
{
    public class WalletaVerifyResponseModel : WalletaBaseResponse
    {
        public bool? is_paid { get; set; }
        public float? invoice_amount { get; set; }
    }
}