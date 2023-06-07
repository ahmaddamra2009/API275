using API275.Data;
using API275.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API275.Controllers
{
    [Route("mohApi/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (!_context.Employees.Any())
            {
                return NotFound();
            }
            return await _context.Employees.ToListAsync();

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int? id)
        {
            try
            {
                if (!_context.Employees.Any())
                {
                    return NotFound();
                }
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound(); //  هذا الموظف غير موجود
                }
                if (id == null)
                {
                    return BadRequest();  // لم يتم ادخال رقم الموظف
                }
                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Search Data, Contact admin info@moh.iq");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, Employee employee)
        {

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }
            _context.Employees.Update(employee);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!EmployeeExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }

            var emp = await _context.Employees.FindAsync(id);
            if (emp==null)
            {
                return NotFound();
            }
            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            return NoContent();
        }
       





        private bool EmployeeExist(int id)
        {
            return (_context.Employees.Any(e => e.EmployeeId == id));
        }
    }
}
