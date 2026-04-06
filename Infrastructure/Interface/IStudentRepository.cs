public interface IStudentRepository
{
	Task<IEnumerable<Student>> GetAll();
	Task<Student> GetById(int id);
	Task<int> Add(Student student);
	Task<int> Update(Student student);
	Task<int> Delete(int id);
}