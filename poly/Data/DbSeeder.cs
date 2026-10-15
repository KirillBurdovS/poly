using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using poly.Models;

namespace poly.Data;

public static class DbSeeder
{
    public const string DemoEmail = "hr@demo.ru";
    public const string DemoPassword = "Demo123!";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        if (await userManager.FindByEmailAsync(DemoEmail) is null)
        {
            var user = new ApplicationUser { UserName = DemoEmail, Email = DemoEmail, EmailConfirmed = true };
            await userManager.CreateAsync(user, DemoPassword);
        }

        // Справочники заполняются только в пустой базе, чтобы не затирать данные пользователя
        if (await db.Departments.AnyAsync()) return;

        var it = new Department { Name = "ИТ-отдел", Description = "Разработка и поддержка информационных систем" };
        var accounting = new Department { Name = "Бухгалтерия", Description = "Учёт, отчётность и расчёт заработной платы" };
        var sales = new Department { Name = "Отдел продаж", Description = "Работа с клиентами и заключение договоров" };
        var hr = new Department { Name = "Отдел кадров", Description = "Подбор, адаптация и учёт персонала" };
        db.Departments.AddRange(it, accounting, sales, hr);

        var developer = new Position { Title = "Разработчик", BaseSalary = 1500 };
        var tester = new Position { Title = "Тестировщик", BaseSalary = 1100 };
        var accountant = new Position { Title = "Бухгалтер", BaseSalary = 900 };
        var manager = new Position { Title = "Менеджер по продажам", BaseSalary = 800 };
        var recruiter = new Position { Title = "Специалист по кадрам", BaseSalary = 850 };
        db.Positions.AddRange(developer, tester, accountant, manager, recruiter);

        var employees = new List<Employee>
        {
            new() { FullName = "Иванов Иван Петрович", Department = it, Position = developer, Salary = 180000 },
            new() { FullName = "Кузнецов Дмитрий Олегович", Department = it, Position = developer, Salary = 165000, Status = EmployeeStatus.OnLeave },
            new() { FullName = "Смирнова Анна Сергеевна", Department = it, Position = tester, Salary = 120000 },
            new() { FullName = "Петрова Мария Ивановна", Department = accounting, Position = accountant, Salary = 95000 },
            new() { FullName = "Новикова Ольга Дмитриевна", Department = accounting, Position = accountant, Salary = 88000 },
            new() { FullName = "Волкова Елена Андреевна", Department = sales, Position = manager, Salary = 90000 },
            new() { FullName = "Соколов Алексей Викторович", Department = sales, Position = manager, Salary = 85000, Status = EmployeeStatus.Sick },
            new() { FullName = "Морозов Павел Николаевич", Department = sales, Position = manager, Salary = 70000, Status = EmployeeStatus.Terminated },
            new() { FullName = "Лебедева Ирина Александровна", Department = hr, Position = recruiter, Salary = 82000 },
        };
        db.Employees.AddRange(employees);

        var random = new Random(42);
        var today = DateTime.Today;
        foreach (var emp in employees.Where(e => e.Status == EmployeeStatus.Active))
        {
            for (var day = 10; day >= 1; day--)
            {
                var date = today.AddDays(-day);
                if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
                db.WorkLogs.Add(new WorkLog
                {
                    Employee = emp,
                    Date = date.AddHours(18),
                    HoursWorked = random.Next(12, 19) / 2.0,
                    HourlyRate = emp.Position!.BaseSalary
                });
            }
        }

        await db.SaveChangesAsync();
    }
}
