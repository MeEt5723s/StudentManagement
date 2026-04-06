public interface IStudentService
{
	Task<IEnumerable<Student>> GetAllStudents();
	Task<Student> GetStudent(int id);
	Task AddStudent(Student student);
	Task UpdateStudent(Student student);
	Task DeleteStudent(int id);
}