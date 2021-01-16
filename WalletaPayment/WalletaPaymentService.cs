using System;
using Walleta.Cpg.SampleCode.WalletaPayment.Models;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Text;

namespace Walleta.Cpg.SampleCode.WalletaPayment
{
    public class WalletaPaymentService
    {
        private string BaseUrl { get; }
        private JavaScriptSerializer JavaScriptSerializer { get; }
        private NLog.Logger Logger;
        private Guid LogTraceID { get; }

        public WalletaPaymentService(string baseUrl)
        {
            this.BaseUrl = baseUrl;

            if (string.IsNullOrWhiteSpace(BaseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            this.JavaScriptSerializer = new JavaScriptSerializer();
            this.Logger = NLog.LogManager.GetLogger(nameof(WalletaPaymentService));
            this.LogTraceID = Guid.NewGuid();
        }

        public WalletaTokenResponseModel GetToken(WalletaTokenRequestModel tokenRequest)
        {
            const string methodName = "payment/request.json";

            string serviceAddress = GetServiceAddress(methodName);

            var httpClient = new HttpClient();
            string jsonBody = JavaScriptSerializer.Serialize(tokenRequest);
            var jsonContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            Logger.Info($"Trace: {LogTraceID.ToString()} | Sending request to {serviceAddress} | Data: {jsonBody}");
            var httpResponse = httpClient.PostAsync(serviceAddress, jsonContent).Result;

            string jsonResponse = ReadJsonBody(httpResponse);

            Logger.Info($"Trace: {LogTraceID.ToString()} | Received response from {serviceAddress} | Data: {jsonResponse}");

            var walletaTokenModel = JavaScriptSerializer.Deserialize<WalletaTokenResponseModel>(jsonResponse);

            return walletaTokenModel;

        }

        public WalletaVerifyResponseModel VerifyPayment(WalletaVerifyRequestModel verifyRequest)
        {
            const string methodName = "payment/verify.json";

            string serviceAddress = GetServiceAddress(methodName);

            var httpClient = new HttpClient();
            string jsonBody = JavaScriptSerializer.Serialize(verifyRequest);
            var jsonContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");


            Logger.Info($"Trace: {LogTraceID.ToString()} | Sending request to {serviceAddress} | Data: {jsonBody}");
            var httpResponse = httpClient.PostAsync(serviceAddress, jsonContent).Result;

            string jsonResponse = ReadJsonBody(httpResponse);
            Logger.Info($"Trace: {LogTraceID.ToString()} | Received response from {serviceAddress} | Data: {jsonResponse}");

            var walletaVerifyResult = JavaScriptSerializer.Deserialize<WalletaVerifyResponseModel>(jsonResponse);

            return walletaVerifyResult;

        }

        private string GetServiceAddress(string methodName)
        {
            return this.BaseUrl.TrimEnd('/') + "/" + methodName;
        }

        private string ReadJsonBody(HttpResponseMessage httpResponse)
        {
            if (httpResponse == null || httpResponse.Content == null)
                return "{}";

            return httpResponse.Content.ReadAsStringAsync().Result;
        }
    }
}