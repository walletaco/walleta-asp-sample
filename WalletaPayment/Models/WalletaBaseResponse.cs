namespace Walleta.Cpg.SampleCode.WalletaPayment.Models
{
    public abstract class WalletaBaseResponse
    {
        public string type { get; set; }
        public string message { get; set; }

        public InvalidFieldMessage[] invalid_fields { get; set; }
    }

    public class InvalidFieldMessage
    {
        public string field { get; set; }
        public string message { get; set; }
    }
}