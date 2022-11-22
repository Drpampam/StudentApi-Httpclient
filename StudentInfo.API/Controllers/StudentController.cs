using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInfo.DataStore.Interfaces;
using StudentInfo.DataStore.Repository;
using StudentInfo.Domain.Entities;

namespace StudentInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;

        public StudentController(IStudentServices student)
        {
            _studentServices = student;
        }

        [HttpGet(Name = "GetStudentInfo")]
        public async Task <IActionResult> GetStudentInfo()
        {
           var studentInfo = await _studentServices.GetAll_Two_Async();
            return Ok(studentInfo);
        }

        [HttpGet]
        [Route("student/{id:Guid}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var student = await _studentServices.GetStudentAsync(id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpPost]
        [Route("studentinfo")]
        public async Task<ActionResult> AddStudentInfo(Student student)
        {
            //write a method to validate all your method
            //try to remove the generated guid issues when creating student info
            await _studentServices.CreateStudentInfoAsync(student);
            return CreatedAtAction("GetStudentInfo", new
            {
                Id = student.id
            }, student);
        }

        [HttpDelete]
        [Route("book/{id:guid}")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            var result = await _studentServices.DeleteStudentAsync(id);
            //if (!result)
            //    return BadRequest("No book exist for this id");
            return NoContent();

        }
        [HttpPut]
        [Route("book/{id:Guid}")]
        public async Task<IActionResult> UpdateStudentInfo (Student student )
        {
             await _studentServices.Update(student);
            return NoContent();
        }
    }
}
