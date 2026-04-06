using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

public class StudentRepository : IStudentRepository
{
	private readonly string _connectionString;
	private readonly ILogger<StudentRepository> _logger;

	public StudentRepository(IConfiguration config, ILogger<StudentRepository> logger)
	{
		_connectionString = config.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		_logger = logger;
	}

	private IDbConnection CreateConnection()
		=> new SqlConnection(_connectionString);

	public async Task<IEnumerable<Student>> GetAll()
	{
		try
		{
			using var connection = CreateConnection();
			var query = "SELECT * FROM Students";

			var students = await connection.QueryAsync<Student>(query);

			if (students == null || !students.Any())
			{
				_logger.LogWarning("No students found in the database.");
				throw new KeyNotFoundException("No students found.");
			}

			_logger.LogInformation("Retrieved {Count} students.", students.Count());
			return students;
		}
		catch (SqlException ex)
		{
			_logger.LogError(ex, "Database error occurred while fetching all students.");
			throw;
		}
	}

	public async Task<Student> GetById(int id)
	{
		if (id <= 0)
		{
			_logger.LogWarning("GetById called with invalid ID: {Id}", id);
			throw new ArgumentException("Invalid student ID.", nameof(id));
		}

		try
		{
			using var connection = CreateConnection();
			var query = "SELECT * FROM Students WHERE Id = @Id";

			var student = await connection.QueryFirstOrDefaultAsync<Student>(query, new { Id = id });

			if (student == null)
			{
				_logger.LogWarning("Student with ID {Id} was not found.", id);
				throw new KeyNotFoundException($"Student with ID {id} was not found.");
			}

			_logger.LogInformation("Retrieved student with ID {Id}.", id);
			return student;
		}
		catch (SqlException ex)
		{
			_logger.LogError(ex, "Database error occurred while fetching student with ID {Id}.", id);
			throw;
		}
	}

	public async Task<int> Add(Student student)
	{
		if (student == null)
			throw new ArgumentNullException(nameof(student), "Student cannot be null.");

		try
		{
			using var connection = CreateConnection();
			var query = @"INSERT INTO Students (Name, Email, Age, Course, CreatedDate)
                          VALUES (@Name, @Email, @Age, @Course, GETDATE())";

			var result = await connection.ExecuteAsync(query, student);
			_logger.LogInformation("Added new student: {Name}", student.Name);
			return result;
		}
		catch (SqlException ex)
		{
			_logger.LogError(ex, "Database error occurred while adding student: {Name}.", student.Name);
			throw;
		}
	}

	public async Task<int> Update(Student student)
	{
		if (student == null)
			throw new ArgumentNullException(nameof(student), "Student cannot be null.");

		if (student.Id <= 0)
			throw new ArgumentException("Invalid student ID for update.", nameof(student));

		try
		{
			using var connection = CreateConnection();
			var query = @"UPDATE Students 
                          SET Name=@Name, Email=@Email, Age=@Age, Course=@Course
                          WHERE Id=@Id";

			var result = await connection.ExecuteAsync(query, student);

			if (result == 0)
			{
				_logger.LogWarning("Update failed — no student found with ID {Id}.", student.Id);
				throw new KeyNotFoundException($"Student with ID {student.Id} was not found.");
			}

			_logger.LogInformation("Updated student with ID {Id}.", student.Id);
			return result;
		}
		catch (SqlException ex)
		{
			_logger.LogError(ex, "Database error occurred while updating student with ID {Id}.", student.Id);
			throw;
		}
	}

	public async Task<int> Delete(int id)
	{
		if (id <= 0)
			throw new ArgumentException("Invalid student ID.", nameof(id));

		try
		{
			using var connection = CreateConnection();
			var query = "DELETE FROM Students WHERE Id=@Id";

			var result = await connection.ExecuteAsync(query, new { Id = id });

			if (result == 0)
			{
				_logger.LogWarning("Delete failed — no student found with ID {Id}.", id);
				throw new KeyNotFoundException($"Student with ID {id} was not found.");
			}

			_logger.LogInformation("Deleted student with ID {Id}.", id);
			return result;
		}
		catch (SqlException ex)
		{
			_logger.LogError(ex, "Database error occurred while deleting student with ID {Id}.", id);
			throw;
		}
	}
}