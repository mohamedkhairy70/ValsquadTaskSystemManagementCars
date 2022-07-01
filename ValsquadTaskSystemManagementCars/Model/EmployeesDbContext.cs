using Microsoft.EntityFrameworkCore;

namespace ValsquadTaskSystemManagementCars.Model
{
    public class ManagementCarsDBContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<EmployeeCard> EmployeeCard { get; set; }
        public ManagementCarsDBContext(DbContextOptions<ManagementCarsDBContext> options)
            : base(options) => Database.Migrate();
    }
}
