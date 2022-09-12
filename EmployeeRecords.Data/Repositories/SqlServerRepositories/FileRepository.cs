using EmployeeRecords.Data.DomainEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EmployeeRecords.Data.Repositories.SqlServerRepositories
{
    public class FileRepository : IFileRepository
    {
        private readonly SqlConnection _connection;
        public FileRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public void AddRange(IEnumerable<EmployeeFile> files)
        {
            var command = _connection.CreateCommand();
            command.CommandText = @"insert into tblFile(FileName,ContentType,FileSize,EmployeeId)values";
            var i = 0;
            foreach (var file in files)
            {
                command.CommandText += $"(@FileName{i},@ContentType{i},@FileSize{i},@EmployeeId{i})";
                command.Parameters.AddWithValue($"@FileName{i}", file.FileName);
                command.Parameters.AddWithValue($"@ContentType{i}", file.ContentType);
                command.Parameters.AddWithValue($"@FileSize{i}", file.ContentType);
                command.Parameters.AddWithValue($"@EmployeeId{i}", file.EmployeeId);
                i++;
            }
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public int Create(EmployeeFile entity)
        {
            var command = _connection.CreateCommand();
            command.CommandText = @"insert into tblFile(FileName,ContentType,FileSize,EmployeeId)
                                            values(@FileName,@ContentType,@FileSize,@EmployeeId);
                                            SELECT scope_identity()";
            command.Parameters.AddWithValue("@FileName", entity.FileName);
            command.Parameters.AddWithValue("@ContentType", entity.ContentType);
            command.Parameters.AddWithValue("@FileSize", entity.FileSize);
            command.Parameters.AddWithValue("@EmployeeId", entity.EmployeeId);
            _connection.Open();
            int fileId =Convert.ToInt32( command.ExecuteScalar());
            _connection.Close();
            return fileId;
        }

        public void Delete(int id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "delete from tblFile e where e.Id=@fileId";
            command.Parameters.AddWithValue("@fileId", id);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void DeleteEmployeeFiles(int employeeId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "delete from tblFile  where EmployeeId=@employeeId";
            command.Parameters.AddWithValue("@employeeId", employeeId);
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public EmployeeFile GetById(int id)
        {
            EmployeeFile result = null;
            var command = _connection.CreateCommand();
            command.CommandText = @"select * from tblFile e where e.Id=@fileId";
            command.Parameters.AddWithValue("@fileId", id);
            _connection.Open();
            var readr = command.ExecuteReader();
            if (readr.Read())
            {
                result = new  EmployeeFile()
                {
                    Id = Convert.ToInt32(readr["Id"]),
                    FileName = readr["FileName"].ToString(),
                    ContentType = readr["ContentType"].ToString(),
                    FileSize = Convert.ToInt64(readr["FileSize"]),
                    EmployeeId = Convert.ToInt32(readr["EmployeeId"])
                };
            }
            _connection.Close();
            return result;
        }

        public List<EmployeeFile> GetEmployeeFiles(int employeeId)
        {
            List<EmployeeFile> result = new List<EmployeeFile>();
            var command = _connection.CreateCommand();
            command.CommandText = @"select * from tblFile e where e.EmployeeId=@employeeId";
            command.Parameters.AddWithValue("@employeeId", employeeId);
            _connection.Open();
            var readr = command.ExecuteReader();
            while (readr.Read())
            {
                result.Add( new EmployeeFile()
                {
                    Id = Convert.ToInt32(readr["Id"]),
                    FileName = readr["FileName"].ToString(),
                    ContentType = readr["ContentType"].ToString(),
                    FileSize = Convert.ToInt64(readr["FileSize"]),
                    EmployeeId = Convert.ToInt32(readr["EmployeeId"])
                });
            }
            _connection.Close();
            return result;
        }

        public void Update(int id, EmployeeFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
