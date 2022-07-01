using ValsquadTaskSystemManagementCars.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValsquadTaskSystemManagementCars.ViewModel;
using Newtonsoft.Json;

namespace ValsquadTaskSystemManagementCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ManagementCarsDBContext _context;

        public EmployeeController(ManagementCarsDBContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<ModelResult>> GetEmployees()
        {
            var employees = await _context.Employees.Include(c => c.EmployeeCards).ThenInclude(c=>c.Cars).AsNoTracking().ToListAsync();
            if (employees is null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found" });
            }
            
            string str = JsonConvert.SerializeObject(employees,new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            return new ModelResult { Result = str };
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelResult>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(c=>c.EmployeeCards).ThenInclude(c=>c.Cars).FirstOrDefaultAsync(o => o.Id == id);

            if (employee == null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found" });
            }

            string str = JsonConvert.SerializeObject(employee, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return new ModelResult { Result = str };
        }
        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<ModelResult>> PostEmployee(VMEmployee vmEmployee)
        {
            var employee = new Employee
            {
                Age = vmEmployee.Age,
                Name = vmEmployee.Name,
                Position = vmEmployee.Position,
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            string str = JsonConvert.SerializeObject(employee, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Ok(new ModelResult { Result = str });
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, VMEmployee vmEmployee)
        {
            if (id != vmEmployee.Id)
            {
                return BadRequest(new ModelResult { ErrorCount = 1, Message = "Error Update" });
            }
            var employeeOld = _context.Employees.FirstOrDefault(x=>x.Id == id);
            EmployeeCard employeeCard;


            employeeOld.Id = vmEmployee.Id;
            employeeOld.Age = vmEmployee.Age;
            employeeOld.Name = vmEmployee.Name;
            employeeOld.Position = vmEmployee.Position;
            _context.Entry(employeeOld).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!EmployeesExists(id))
                {
                    return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found" });
                }
                else
                {
                    return this.BadRequest(new ModelResult { ErrorCount = 1, Message = e.Message });
                }
            }

            return Ok(new ModelResult { ErrorCount = 0, Message = "Successful Update" });
        }

        
        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.Include(x=>x.EmployeeCards).FirstOrDefaultAsync(x=> x.Id == id);
            if (employee == null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found" });
            }
            _context.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new ModelResult { ErrorCount = 0, Message = "Successful Delete" });
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
