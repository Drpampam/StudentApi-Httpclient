using StudentInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfo.DataStore.Interfaces
{
    public interface IStudentServices
    {
        Task<List<Student>> GetAllAsync();
        Task<IEnumerable<Student>> GetAll_Two_Async();
        Task<Student> GetStudentAsync(Guid id);
        Task<Uri> CreateStudentInfoAsync(Student student);
        Task<HttpStatusCode> DeleteStudentAsync(Guid id);
        Task<Student> UpdateStudentInfoAsync(Student student);
        Task<Student> Update(Student student);
    }
}
