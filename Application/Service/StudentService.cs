public class StudentService : IStudentService
{
	private readonly IStudentRepository _repo;

	public StudentService(IStudentRepository repo)
	{
		_repo = repo;
	}

	public async Task<IEnumerable<Student>> GetAllStudents()
		=> await _repo.GetAll();

	public async Task<Student> GetStudent(int id)
		=> await _repo.GetById(id);

	public async Task AddStudent(Student student)
		=> await _repo.Add(student);

	public async Task UpdateStudent(Student student)
		=> await _repo.Update(student);

	public async Task DeleteStudent(int id)
		=> await _repo.Delete(id);
}