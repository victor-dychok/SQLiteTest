using Microsoft.EntityFrameworkCore;
using SQLiteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<DBMunsterbergResult> MunsterbergResults { get; set; } = null!;
        public DbSet<DBShulteResult> ShulteResults { get; set; } = null!;
        public DbSet<DBStrupResult> StrupResults { get; set; } = null!;
        public DbSet<MunsterbergWords> MunsterbergWords { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
    }
}
