using System.ComponentModel.DataAnnotations.Schema;


namespace poly.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;


    // Это связь: ID должности
    public int PositionId { get; set; }

    // Это навигационное свойство: сам объект Должности
    [ForeignKey("PositionId")]
    public Position? Position { get; set; }

    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

}
