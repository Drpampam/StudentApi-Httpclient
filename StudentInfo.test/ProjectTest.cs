using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace StudentInfo.test
{
    [TestClass]
    public class ProjectTest
    {
        private string BaseUrl = "http://etestapi.test.eminenttechnology.com/api/Student";

        [TestMethod]
        public void TestGetAllEndPoint()
        {
            //Step1. Create HttpClient
            HttpClient httpClient = new HttpClient();

            //Create the request and execute it
            httpClient.BaseAddress = new Uri(BaseUrl);

            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(BaseUrl);
            HttpResponseMessage httpResponseMessage= httpResponse.Result;
            Assert.IsNotNull(httpResponseMessage);
            Console.WriteLine(httpResponseMessage.ToString());

            //Status code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Assert.IsNotNull(statusCode);
            Assert.AreEqual(HttpStatusCode.OK, statusCode);
            Console.WriteLine("status code = " + statusCode );
            Console.WriteLine("status code = " + (int)statusCode);

            //Response Data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Assert.IsNotNull(data);
            Console.WriteLine(data);

            //Close connection
            httpClient.Dispose();

        }
    }
}
