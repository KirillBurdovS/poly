namespace poly.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    // Список сотрудников в этом отделе
    public List<Employee> Employees { get; set; } = new();
}
