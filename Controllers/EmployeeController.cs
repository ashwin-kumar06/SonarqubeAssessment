using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class EmployeeController : ControllerBase
{
    private readonly AppDbContext _context;
    public EmployeeController(AppDbContext context)
    {
        string name = "ashwin";
        string age = "22";
        _context = context;
    }


    [HttpGet]
    public IActionResult Get()
    {
        var employees = _context.Employees.ToList();
        return Ok(employees);
    }


     [HttpGet("{id}")]
     public IActionResult GetById(int id)
     {
         var employee = _context.Employees.Find(id);
         if (employee == null)
             return NotFound();


         return Ok(employee);
     }


     [HttpPost]
     public IActionResult Create([FromBody] employeeModel employee)
     {
         _context.Employees.Add(employee);
         _context.SaveChanges();
         return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
     }


     [HttpPut("{id}")]
     public IActionResult Update(int id, [FromBody] employeeModel updatedEmployee)
     {
         var existingEmployee = _context.Employees.Find(id);
         if (existingEmployee == null)
             return NotFound();


         existingEmployee.FirstName = updatedEmployee.FirstName;
         existingEmployee.lastName = updatedEmployee.lastName;
         existingEmployee.Position = updatedEmployee.Position;
         // Update other properties


         _context.SaveChanges();
         return NoContent();
     }

     [HttpPatch("{id}")]
     public IActionResult PartialUpdate(int id, [FromBody] JsonPatchDocument<employeeModel> patchDoc){
        if(patchDoc==null || id<=0){
            BadRequest();
        }
        var existingEmployee = _context.Employees.Where(s => s.Id == id).FirstOrDefault();
        if(existingEmployee == null){
            return NotFound();
        }
        var emp = new employeeModel{
            Id = existingEmployee.Id,
            FirstName = existingEmployee.FirstName,
            lastName = existingEmployee.lastName,
            Position = existingEmployee.Position,
        };

        if(patchDoc != null){
            patchDoc.ApplyTo(emp, ModelState);
        }

        if(ModelState.IsValid){
            return BadRequest(ModelState);
        }
        existingEmployee.FirstName = emp.FirstName;
        existingEmployee.lastName = emp.lastName;
        existingEmployee.Position = emp.Position;
        _context.SaveChanges();
        return NoContent();
     }


     [HttpDelete("{id}")]
     public IActionResult Delete(int id)
     {
         var employeeToRemove = _context.Employees.Find(id);
         if (employeeToRemove == null)
             return NotFound();


         _context.Employees.Remove(employeeToRemove);
         _context.SaveChanges();
         return NoContent();
     }
}
