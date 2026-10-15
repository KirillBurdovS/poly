namespace poly.Models;

public enum EmployeeStatus
{
    Active,     // Работает
    OnLeave,    // В отпуске
    Sick,       // Болеет
    Terminated  // Уволен
}

public static class EmployeeStatusExtensions
{
    public static string ToText(this EmployeeStatus status) => status switch
    {
        EmployeeStatus.Active => "Работает",
        EmployeeStatus.OnLeave => "В отпуске",
        EmployeeStatus.Sick => "Болеет",
        EmployeeStatus.Terminated => "Уволен",
        _ => "—"
    };

    public static string BadgeClass(this EmployeeStatus status) => status switch
    {
        EmployeeStatus.Active => "bg-success",
        EmployeeStatus.OnLeave => "bg-warning text-dark",
        EmployeeStatus.Sick => "bg-danger",
        _ => "bg-secondary"
    };
}
