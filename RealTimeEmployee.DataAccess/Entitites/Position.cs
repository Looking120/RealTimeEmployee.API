namespace RealTimeEmployee.DataAccess.Entitites;

public class Position : BaseEntity
{
    public required string Title { get; set; }

    public string? Description { get; set; }

    public decimal BaseSalary { get; set; }

    public Guid DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = []!;
}