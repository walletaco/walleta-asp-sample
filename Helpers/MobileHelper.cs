using System.ComponentModel.DataAnnotations;

namespace Walleta.Cpg.SampleCode.Helpers
{
    public class Mobile : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string mobile = value.ToString();

            if (string.IsNullOrWhiteSpace(mobile))
                return false;

            if (mobile.Length != 11)
                return false;

            return mobile.StartsWith("09");
        }
    }
}