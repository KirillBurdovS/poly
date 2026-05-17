using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using poly.Models;

namespace poly.Data
{
    // Используем классический конструктор без лишних наворотов
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
        public DbSet<Department> Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Фиксим точность для работы с деньгами
            modelBuilder.Entity<Employee>(entity => {
                entity.Property(e => e.Salary).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Position>(entity => {
                entity.Property(p => p.BaseSalary).HasPrecision(18, 2);
            });

            modelBuilder.Entity<WorkLog>(entity => {
                entity.Property(w => w.HourlyRate).HasPrecision(18, 2);
            });
        }
    }
}
