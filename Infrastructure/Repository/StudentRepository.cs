using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

public class StudentRepository : IStudentRepository
{
	private readonly IConfiguration _config;
	private readonly string _connectionString;

	public StudentRepository(IConfiguration config)
	{
		_config = config;
		_connectionString = _config.GetConnectionString("DefaultConnection");
	}

	private IDbConnection CreateConnection()
		=> new SqlConnection(_connectionString);

	public async Task<IEnumerable<Student>> GetAll()
	{
		using var connection = CreateConnection();
		var query = "SELECT * FROM Students";
		return await connection.QueryAsync<Student>(query);
	}

	public async Task<Student> GetById(int id)
	{
		using var connection = CreateConnection();
		var query = "SELECT * FROM Students WHERE Id = @Id";
		return await connection.QueryFirstOrDefaultAsync<Student>(query, new { Id = id });
	}

	public async Task<int> Add(Student student)
	{
		using var connection = CreateConnection();
		var query = @"INSERT INTO Students (Name, Email, Age, Course, CreatedDate)
                      VALUES (@Name, @Email, @Age, @Course, GETDATE())";
		return await connection.ExecuteAsync(query, student);
	}

	public async Task<int> Update(Student student)
	{
		using var connection = CreateConnection();
		var query = @"UPDATE Students 
                      SET Name=@Name, Email=@Email, Age=@Age, Course=@Course
                      WHERE Id=@Id";
		return await connection.ExecuteAsync(query, student);
	}

	public async Task<int> Delete(int id)
	{
		using var connection = CreateConnection();
		var query = "DELETE FROM Students WHERE Id=@Id";
		return await connection.ExecuteAsync(query, new { Id = id });
	}
}