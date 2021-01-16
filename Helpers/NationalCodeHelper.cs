using System;
using System.ComponentModel.DataAnnotations;

namespace Walleta.Cpg.SampleCode.Helpers
{
    public class NationalCode : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string nationalCode = value.ToString();

            return validationNationalCode(nationalCode);
        }

        private static bool validationNationalCode(string code)
        {
            try
            {
                //check length
                if (code.Length != 10)
                    return false;

                long nationalCode = long.Parse(code);
                byte[] arrayNationalCode = new byte[10];

                //extract digits from number
                for (int i = 0; i < 10; i++)
                {
                    arrayNationalCode[i] = (byte)(nationalCode % 10);
                    nationalCode = nationalCode / 10;
                }

                //Checking the control digit
                int sum = 0;
                for (int i = 9; i > 0; i--)
                    sum += arrayNationalCode[i] * (i + 1);
                int temp = sum % 11;
                if (temp < 2)
                    return arrayNationalCode[0] == temp;
                else
                    return arrayNationalCode[0] == 11 - temp;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}