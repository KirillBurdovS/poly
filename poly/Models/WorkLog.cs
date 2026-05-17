namespace poly.Models;

public class WorkLog
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; } // Имя будем брать отсюда

    public double HoursWorked { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public decimal HourlyRate { get; set; }

    public decimal TotalSalary => (decimal)HoursWorked * HourlyRate;
}
