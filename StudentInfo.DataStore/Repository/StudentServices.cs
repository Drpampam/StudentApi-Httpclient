using Newtonsoft.Json;
using StudentInfo.DataStore.Interfaces;
using StudentInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentInfo.DataStore.Repository
{
    public class StudentServices : IStudentServices
    {
        private readonly IHttpClientFactory httpClient;
        private readonly string BaseUrl = "http://etestapi.test.eminenttechnology.com/api/Student";

        public StudentServices(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory;
        }

        #region CreateMethod
        public async Task<Uri> CreateStudentInfoAsync(Student student)
        {
            //create httpclient
            HttpClient httpClient = new HttpClient();

            //create request
            httpClient.BaseAddress = new Uri(BaseUrl);
            HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync(BaseUrl, student);
            HttpResponseMessage httpResponseMessage = httpResponse;
            httpResponseMessage.EnsureSuccessStatusCode();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // return URI of the created resource.
            return httpResponse.Headers.Location ?? new Uri(BaseUrl);
        }
        #endregion

        #region SecondGetAllMethod
        public async Task<IEnumerable<Student>> GetAll_Two_Async()
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(BaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            //This mean  our application can deserialize either by xml or json format
            httpClient.Dispose();
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                if (response.IsSuccessStatusCode)
                {
                    var student = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Student>>(responseContent,
                        new JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                    return student!;
                }
                return new List<Student>();
            }
            //if the user selects xml format for deserialization
            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(List<Student>));
                var student = (List<Student>)serializer.Deserialize(new StringReader(responseContent));
                return student;
            }
            return new List<Student> { new Student() };
        }
        #endregion
        #region FirstGetAllMethod
        public async Task<List<Student>> GetAllAsync()
        {
            var httpClient = this.httpClient.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync(BaseUrl);
            httpClient.Dispose();
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<List<Student>>(responseContent);
                return student ?? new List<Student>();
            }
            return null!;
        }
        #endregion

        #region GetById
        public async Task<Student> GetStudentAsync(Guid id)
        {
            var uri = new Uri($"http://etestapi.test.eminenttechnology.com/api/Student/{id}");
            Student student = new Student();
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                student = JsonConvert.DeserializeObject<Student>(responseContent) ?? throw new Exception("Student NotFound");
            }
            return student ?? new Student();
        }
        #endregion

        #region Delete
        public async Task<HttpStatusCode> DeleteStudentAsync(Guid id)
        {
            var uri = new Uri($"http://etestapi.test.eminenttechnology.com/api/Student/{id}");
            HttpResponseMessage response = await this.httpClient.CreateClient().DeleteAsync(uri);
            return response.StatusCode;
        }
        #endregion

        #region Update
        public async Task<Student> UpdateStudentInfoAsync(Student student)
        {
            var uri = new Uri($"http://etestapi.test.eminenttechnology.com/api/Student/{student.id}");
            var httpClient = new HttpClient();
            this.httpClient.CreateClient();
            HttpResponseMessage response = httpClient.PutAsJsonAsync(uri, student).Result;
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the updated product from the response body.
                student = await response.Content.ReadFromJsonAsync<Student>() ?? throw new Exception("Not Found");
                return student;
            }
            return null ?? new Student();

        }
        #endregion

        public async Task <Student> Update(Student student)
        {
            HttpClient httpClient = new HttpClient();
            var uri = new Uri($"http://etestapi.test.eminenttechnology.com/api/Student/{student.id}");
            var studentDataToUpdate = System.Text.Json.JsonSerializer.Serialize(student);
            var resuest = new HttpRequestMessage(HttpMethod.Put, uri);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            resuest.Content = new StringContent(studentDataToUpdate);
            resuest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await httpClient.SendAsync(resuest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updateedStudent = System.Text.Json.JsonSerializer.Deserialize<Student>(content);
            return updateedStudent;
        }
    }
}

