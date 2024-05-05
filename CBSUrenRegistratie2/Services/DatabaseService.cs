using Microsoft.Data.Sqlite;
using CBSUrenRegistratie2.Models;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using BCrypt.Net;


namespace CBSUrenRegistratie2.Services
{
    public class DatabaseService
    {
        private readonly string _dbPath;

        public DatabaseService()
        {
            //_dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "appdb.db");
            _dbPath = @"C:\Uren\appdb.db";
            InitializeDatabase();
            //ResetAllPasswordsToDefault().Wait();
            //HashAndUpdatePasswords().Wait();
        }
        private void InitializeDatabase()
        {
            try
            {
                using var db = new SqliteConnection($"Filename={_dbPath}");
                db.Open();

                var hoursTableCommand = new SqliteCommand(
        @"CREATE TABLE IF NOT EXISTS HoursLogged (
            HoursId INTEGER PRIMARY KEY AUTOINCREMENT,
            WorkerId INTEGER,
            ProjectId INTEGER,
            DateWorked DATE,
            HoursWorked DECIMAL,
            FOREIGN KEY (WorkerId) REFERENCES Users (UserId),
            FOREIGN KEY (ProjectId) REFERENCES Projects (ProjectId)
        )", db);
                hoursTableCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
        }

        public async Task<List<TimeLogEntry>> GetTimeEntriesForDay(int workerId, DateTime day)
        {
            var entries = new List<TimeLogEntry>();
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var query = new SqliteCommand(
                @"SELECT * FROM HoursLogged WHERE WorkerId = @WorkerId AND DateWorked = @DateWorked", db);
            query.Parameters.AddWithValue("@WorkerId", workerId);
            query.Parameters.AddWithValue("@DateWorked", day.Date);
            using var reader = await query.ExecuteReaderAsync();
            while (reader.Read())
            {
                entries.Add(new TimeLogEntry(
            DateTime.Parse(reader["DateWorked"].ToString())  // Correctly passes DateTime
        )
                {
                    HoursId = Convert.ToInt32(reader["HoursId"]),
                    WorkerId = Convert.ToInt32(reader["WorkerId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    DateWorked = DateTime.Parse(reader["DateWorked"].ToString()),
                    StartTime = DateTime.Parse(reader["StartTime"].ToString()).TimeOfDay,
                    EndTime = DateTime.Parse(reader["EndTime"].ToString()).TimeOfDay
                });
            }
            return entries;
        }

        public async Task SubmitTimeEntriesForMonth(int workerId, DateTime month, List<TimeLogEntry> entries)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var transaction = db.BeginTransaction();
            try
            {
                foreach (var entry in entries)
                {
                    var updateCommand = new SqliteCommand(
                        @"INSERT INTO HoursLogged (WorkerId, ProjectId, DateWorked, StartTime, EndTime) 
                          VALUES (@WorkerId, @ProjectId, @DateWorked, @StartTime, @EndTime)", db, transaction);
                    updateCommand.Parameters.AddWithValue("@WorkerId", workerId);
                    updateCommand.Parameters.AddWithValue("@ProjectId", entry.ProjectId);
                    updateCommand.Parameters.AddWithValue("@DateWorked", entry.DateWorked);
                    updateCommand.Parameters.AddWithValue("@StartTime", entry.StartTime);
                    updateCommand.Parameters.AddWithValue("@EndTime", entry.EndTime);
                    await updateCommand.ExecuteNonQueryAsync();
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task UpdateTimeEntry(TimeLogEntry entry)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var command = new SqliteCommand(
                @"UPDATE HoursLogged SET ProjectId = @ProjectId, StartTime = @StartTime, EndTime = @EndTime
                  WHERE HoursId = @HoursId", db);
            command.Parameters.AddWithValue("@HoursId", entry.HoursId);
            command.Parameters.AddWithValue("@ProjectId", entry.ProjectId);
            command.Parameters.AddWithValue("@StartTime", entry.StartTime);
            command.Parameters.AddWithValue("@EndTime", entry.EndTime);
            await command.ExecuteNonQueryAsync();
        }



        public async Task LogHours(int workerId, int projectId, DateTime dateWorked, decimal hours)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var insertCommand = new SqliteCommand("INSERT INTO HoursLogged (WorkerId, ProjectId, DateWorked, HoursWorked) VALUES (@WorkerId, @ProjectId, @DateWorked, @HoursWorked)", db);
            insertCommand.Parameters.AddWithValue("@WorkerId", workerId);
            insertCommand.Parameters.AddWithValue("@ProjectId", projectId);
            insertCommand.Parameters.AddWithValue("@DateWorked", dateWorked);
            insertCommand.Parameters.AddWithValue("@HoursWorked", hours);
            await insertCommand.ExecuteNonQueryAsync();
        }

        public async Task<List<Project>> GetProjectsForWorker(int workerId)
        {
            var projects = new List<Project>();
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var queryCommand = new SqliteCommand(
                @"SELECT Projects.* FROM Projects
          JOIN WorkerProjects ON Projects.ProjectId = WorkerProjects.ProjectId
          WHERE WorkerProjects.WorkerId = @WorkerId", db);
            queryCommand.Parameters.AddWithValue("@WorkerId", workerId);

            using var reader = await queryCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                projects.Add(new Project
                {
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    Description = reader["Description"].ToString()
                });
            }
            return projects;
        }
        public async Task UpdateBreakEntry(BreakEntry breakEntry)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var command = new SqliteCommand(
                @"UPDATE BreakEntries SET StartTime = @StartTime, EndTime = @EndTime WHERE BreakId = @BreakId", db);
            command.Parameters.AddWithValue("@BreakId", breakEntry.BreakId);
            command.Parameters.AddWithValue("@StartTime", breakEntry.StartBreakTime);
            command.Parameters.AddWithValue("@EndTime", breakEntry.EndBreakTime);
            await command.ExecuteNonQueryAsync();
        }
        public async Task<User> AuthenticateUser(string username, string password)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var queryCommand = new SqliteCommand(
    @"SELECT Users.UserId, Users.Username, Users.Password, Users.RoleId, Roles.RoleName 
      FROM Users 
      JOIN Roles ON Users.RoleId = Roles.RoleId 
      WHERE Users.Username = @Username", db);
            queryCommand.Parameters.AddWithValue("@Username", username);

            using var reader = await queryCommand.ExecuteReaderAsync();
            if (reader.Read())
            {
                var hashedPassword = reader["Password"].ToString();
                if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                {
                    return new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString(),
                        Role = reader["RoleName"].ToString(),
                        RoleId = Convert.ToInt32(reader["RoleId"])
                    };
                }
            }
            return null;
        }

        public async Task AddWorker(Worker worker)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();

            var insertCommand = new SqliteCommand
            {
                Connection = db,
                CommandText = "INSERT INTO Workers (Name, Email, HourlyRate) VALUES (@Name, @Email, @HourlyRate)"
            };
            insertCommand.Parameters.AddWithValue("@Name", worker.Name);
            insertCommand.Parameters.AddWithValue("@Email", worker.Email);
            insertCommand.Parameters.AddWithValue("@HourlyRate", worker.HourlyRate);

            await insertCommand.ExecuteNonQueryAsync();
        }

        public async Task<List<Worker>> GetAllWorkers()
        {
            var workers = new List<Worker>();
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();

            var selectCommand = new SqliteCommand("SELECT * FROM Workers", db);
            var reader = await selectCommand.ExecuteReaderAsync();

            while (reader.Read())
            {
                workers.Add(new Worker
                {
                    WorkerId = Convert.ToInt32(reader["WorkerId"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    HourlyRate = Convert.ToDecimal(reader["HourlyRate"])
                });
            }
            return workers;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = new List<User>();
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var queryCommand = new SqliteCommand("SELECT Users.*, Roles.RoleName FROM Users JOIN Roles ON Users.RoleId = Roles.RoleId", db);
            using var reader = await queryCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                users.Add(new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Role = reader["RoleName"].ToString(),
                    RoleId = Convert.ToInt32(reader["RoleId"])
                });
            }
            return users;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            var projects = new List<Project>();
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var queryCommand = new SqliteCommand("SELECT ProjectId, ProjectName, Description FROM Projects", db);

            using var reader = await queryCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                projects.Add(new Project
                {
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    Description = reader["Description"].ToString()
                });
            }
            return projects;
        }

        public async Task AddRole(string roleName, string description = "")
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var insertCommand = new SqliteCommand("INSERT INTO Roles (RoleName, RoleDescription) VALUES (@RoleName, @Description)", db);
            insertCommand.Parameters.AddWithValue("@RoleName", roleName);
            insertCommand.Parameters.AddWithValue("@Description", description);
            await insertCommand.ExecuteNonQueryAsync();
        }

        public async Task AddUser(User user)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var command = new SqliteCommand("INSERT INTO Users (Username, Password, RoleId) VALUES (@Username, @HashedPassword, @RoleId)", db);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@HashedPassword", hashedPassword);  // Ensure password hashing in a production application
            command.Parameters.AddWithValue("@RoleId", user.RoleId);
            await command.ExecuteNonQueryAsync();
        }
        public async Task UpdateUser(User user)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var command = new SqliteCommand("UPDATE Users SET Username = @Username, Password = @Password, RoleId = @RoleId WHERE UserId = @UserId", db);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);  // Ensure password hashing in a production application
            command.Parameters.AddWithValue("@RoleId", user.RoleId);
            command.Parameters.AddWithValue("@UserId", user.UserId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteUser(int userId)
        {
            using var db = new SqliteConnection($"Filename={_dbPath}");
            await db.OpenAsync();
            var command = new SqliteCommand("DELETE FROM Users WHERE UserId = @UserId", db);
            command.Parameters.AddWithValue("@UserId", userId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<TimeLogEntry>> GetTimeEntriesForMonth(int userId, DateTime date)
        {
            // Dummy implementation - replace with actual database access code
            return new List<TimeLogEntry>();
        }
    }
}


