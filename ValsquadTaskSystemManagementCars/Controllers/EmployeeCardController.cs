using ValsquadTaskSystemManagementCars.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ValsquadTaskSystemManagementCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCardController : ControllerBase
    {
        private readonly ManagementCarsDBContext _context;

        public EmployeeCardController(ManagementCarsDBContext context)
        {
            _context = context;
        }
        [HttpGet("{idCard}")]
        public async Task<ActionResult<ModelResult>> HighwayAccessCard(int idCard)
        {
            var employee = await _context.Employees.Include(x=>x.EmployeeCards)
                                        .FirstOrDefaultAsync(o => o.EmployeeCards.Any(x=>x.Id == idCard));
            var employeeCard = employee.EmployeeCards.FirstOrDefault(x=>x.Id == idCard);
            if (employeeCard == null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found EmployeeCar " });
            }
            else
            {
                if (employeeCard is not null)
                {
                    if (employeeCard?.DateTimeNow.Minute == DateTime.Now.Minute && employeeCard.CountAccessSameMinute == 1)
                    {
                        employeeCard.setCountAccessSameMinute(employeeCard.CountAccessSameMinute + 1);
                    }
                    else
                    {
                        employeeCard?.setdebit(employeeCard.Debit + 4);
                        employeeCard.setCountAccessSameMinute(1);
                    }
                    employeeCard.DateTimeNow = DateTime.Now;
                }
                _context.Entry(employeeCard).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return this.BadRequest(new ModelResult { ErrorCount = 1, Message = e.Message });
                }
            }
            string str = JsonConvert.SerializeObject(employeeCard, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Ok(new ModelResult { Result = str, Message = "Successful Access Card" });
        }
        [HttpPost("{employeeId}/{plateNumber}")]
        public async Task<ActionResult<ModelResult>> GeneratesHighway(int employeeId, string plateNumber)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            var car = await _context.Cars.FirstOrDefaultAsync(x=>x.PlateNumber == plateNumber);;
            if(employee is null || car is null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found employee or car" });
            }
            EmployeeCard employeeCard = await _context.EmployeeCard.FirstOrDefaultAsync(x=>x.Employees.Id == employeeId && x.Cars.PlateNumber == plateNumber);
            if (employeeCard is not null)
            {
                return BadRequest(new ModelResult { ErrorCount = 1, Message = "Can't Generate Employee Card" });
            }
            else
            {
                employeeCard = new EmployeeCard
                {
                    Employees = employee,
                    Cars = car,
                    DateTimeNow = DateTime.Now
                };
            }
            _context.EmployeeCard.Add(employeeCard);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return this.BadRequest(new ModelResult { ErrorCount = 1, Message = e.Message });
            }

            var emp = _context.Employees.Include(x => x.EmployeeCards).FirstOrDefault(x => x.Id == employeeCard.Id);
            string str = JsonConvert.SerializeObject(employee, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Ok(new ModelResult { Result = str, Message = "Successful Generate Card" });
        }
    }
}
