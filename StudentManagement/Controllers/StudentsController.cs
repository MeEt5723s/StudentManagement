using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudentController : ControllerBase
{
	private readonly IStudentService _service;

	public StudentController(IStudentService service)
	{
		_service = service;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var students = await _service.GetAllStudents();
		return Ok(students);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id)
	{
		var student = await _service.GetStudent(id);
		if (student == null)
			return NotFound();
		return Ok(student);
	}

	[HttpPost]
	public async Task<IActionResult> Add(Student student)
	{
		await _service.AddStudent(student);
		return Ok("Student Added");
	}

	[HttpPut]
	public async Task<IActionResult> Update(Student student)
	{
		await _service.UpdateStudent(student);
		return Ok("Student Updated");
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _service.DeleteStudent(id);
		return Ok("Student Deleted");
	}
}