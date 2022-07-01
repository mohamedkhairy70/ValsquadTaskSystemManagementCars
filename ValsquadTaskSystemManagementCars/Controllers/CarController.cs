using ValsquadTaskSystemManagementCars.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ValsquadTaskSystemManagementCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ManagementCarsDBContext _context;

        public CarController(ManagementCarsDBContext context)
        {
            _context = context;
        }

        // GET: api/Car
        [HttpGet]
        public async Task<ActionResult<ModelResult>> GetCars()
        {
            var cars= await _context.Cars.AsNoTracking().ToListAsync();
            if (cars is null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found" });
            }

            string str = JsonConvert.SerializeObject(cars, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return new ModelResult { Result = str };
        }

        // GET: api/Car/5
        [HttpGet("{plateNumber}")]
        public async Task<ActionResult<ModelResult>> GetCar(string plateNumber)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(o => o.PlateNumber == plateNumber);
            if(car is null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found" });
            }

            string str = JsonConvert.SerializeObject(car, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return new ModelResult { Result = str };
        }
        // POST: api/Car
        [HttpPost]
        public async Task<ActionResult<ModelResult>> PostEmployee(Car car)
        {
            _context.Cars.Add(car);
            if(await _context.SaveChangesAsync() == 0)
            {
                return BadRequest(new ModelResult { ErrorCount = 1, Message = "Error Saving" });
            }

            string str = JsonConvert.SerializeObject(car, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Ok(new ModelResult { Result = str ,Message= "Successful Saving" });
        }
        // PUT: api/Car/5
        [HttpPut("{plateNumber}")]
        public async Task<IActionResult> PutCar(string plateNumber, Car car)
        {
            if (plateNumber != car.PlateNumber)
            {
                return BadRequest(new ModelResult { ErrorCount = 1, Message = "Error Update" });
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!CarsExists(plateNumber))
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
        // DELETE: api/Car/5
        [HttpDelete("{plateNumber}")]
        public async Task<IActionResult> DeleteEmployee(string plateNumber)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.PlateNumber == plateNumber);
            if (_context.EmployeeCard.Include(x => x.Cars).Any(x => x.Cars.PlateNumber == plateNumber))
            {
                return BadRequest(new ModelResult { ErrorCount = 1, Message = "Error Delete" });
            }
            else if(car is null)
            {
                return NotFound(new ModelResult { ErrorCount = 1, Message = "Not Found" });
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return Ok(new ModelResult { ErrorCount = 0, Message = "Successful Delete" });
        }
        private bool CarsExists(string plateNumber)
        {
            return _context.Cars.Any(e => e.PlateNumber == plateNumber);
        }
    }
}
