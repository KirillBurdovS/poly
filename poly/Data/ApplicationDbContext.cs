using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using poly.Models;

namespace poly.Data
{
    // ???????? ???? ??????: ???????????? Identity ? ?????? ???????????
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

            // ???????? ???? ?????? ? ????????? ?? ??????
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Salary).HasPrecision(18, 2);

                // ?????? ??????? ????? ??? ?????????, ???? ? ??? ???? ??????????
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Position)
                      .WithMany()
                      .HasForeignKey(e => e.PositionId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(p => p.BaseSalary).HasPrecision(18, 2);
            });

            modelBuilder.Entity<WorkLog>(entity =>
            {
                entity.Property(w => w.HourlyRate).HasPrecision(18, 2);

                // ??????? ?????????? ?? ????????? ?????? ? ???????????
                entity.HasOne(w => w.Employee)
                      .WithMany()
                      .HasForeignKey(w => w.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
