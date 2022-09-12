using EmployeeRecords.Data.DomainEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EmployeeRecords.Data.Repositories.SqlServerRepositories
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly SqlConnection _connection;
        public EmployeeRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public int Create(Employee entity)
        {
            var command = _connection.CreateCommand();
            command.CommandText = @"insert into tblEmployee(Name,Department,DateOfBirth,Address)
                                        values(@Name,@Department,@DateOfBirth,@Address);
                                        SELECT scope_identity()";
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Department", entity.Department);
            command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth.Date);
            command.Parameters.AddWithValue("@Address", entity.Address);
            _connection.Open();
            int employeeId =Convert.ToInt32( command.ExecuteScalar());
            _connection.Close();
            return employeeId;
        }

        public void Delete(int id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "delete from tblEmployee  where Id=@employeeId";
            command.Parameters.AddWithValue("@employeeId", id);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public Employee GetById(int id)
        {
            Employee result = null;
            var command = _connection.CreateCommand();
            command.CommandText = @"select * from tblEmployee e where e.Id=@employeeId";
            command.Parameters.AddWithValue("@employeeId", id);
            _connection.Open();
            var readr = command.ExecuteReader();
            if (readr.Read())
            {
                result = new Employee()
                {
                    Id = Convert.ToInt32(readr["Id"]),
                    Name = readr["Name"].ToString(),
                    Department = readr["Department"] as string,
                    DateOfBirth = Convert.ToDateTime(readr["DateOfBirth"]),
                    Address = readr["Address"] as string
                };
            }
            _connection.Close();
            return result;
        }

        public List<Employee> Search(string keyword)
        {
            var result = new List<Employee>();
            if (string.IsNullOrEmpty(keyword))
                return result;
                var command = _connection.CreateCommand();
                command.CommandText = @"select * from tblEmployee
                                        where Name like @searchWord
                                        or Department like @searchWord
                                        or Address like @searchWord;
                                        select * from tblFile f where f.EmployeeId in (
	                                    select id from tblEmployee
	                                    where Name like @searchWord
	                                    or Department like @searchWord
	                                    or Address like @searchWord
                                    )";
                command.Parameters.AddWithValue("@searchWord", "%" + keyword + "%");
                _connection.Open();
                var readr = command.ExecuteReader();
                while (readr.Read())
                {
                    result.Add(new Employee()
                    {
                        Id = Convert.ToInt32(readr["Id"]),
                        Name = readr["Name"].ToString(),
                        Department = readr["Department"] as string,
                        DateOfBirth = Convert.ToDateTime(readr["DateOfBirth"]),
                        Address = readr["Address"] as string
                    });
                }
            _connection.Close();
            return result;
            }

        public void Update(int id, Employee entity)
        {
            var command = _connection.CreateCommand();
            command.CommandText = @"update tblEmployee set Name=@Name,Department=@Department,DateOfBirth=@DateOfBirth,Address=@Address
                                        where id=@employeeId";
            command.Parameters.AddWithValue("@employeeId", id);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Department", entity.Department);
            command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth.Date);
            command.Parameters.AddWithValue("@Address", entity.Address);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
