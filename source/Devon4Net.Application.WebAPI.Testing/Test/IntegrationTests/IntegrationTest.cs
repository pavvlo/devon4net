using AutoMapper;
using Devon4Net.Application.WebAPI.Implementation.Domain.Database;
using Devon4Net.Infrastructure.Test;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Devon4Net.Application.XUnit.Test.IntegrationTests
{
    public class IntegrationTest : DatabaseManagementTest<EmployeeContext>
    {
        public SqliteConnection? SqliteConnection { get; set; }

        public override void ConfigureContext()
        {
            try
            {
                var route = $"Data Source={Path.GetFullPath("Employee.db")}";
                SqliteConnection = new SqliteConnection(route);
                var builder = new DbContextOptionsBuilder<EmployeeContext>();
                builder.UseSqlite(SqliteConnection);
                ContextOptions = builder.Options;
                Context = new EmployeeContext(ContextOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} : {ex.InnerException}");
            }
        }

        public override void ConfigureMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            Mapper = mockMapper.CreateMapper();
        }

        public override void SeedData()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }
    }
}
