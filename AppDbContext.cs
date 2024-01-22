using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApi;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<employeeModel> Employees { get; set; }
}