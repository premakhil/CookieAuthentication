using EmployeeManagement.DTOs;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [Route("api/EmployeeManagement")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {

            var employee = _employeeService.employees.Where(e => e.Username == loginDTO.Username && e.Password == loginDTO.Password).FirstOrDefault();

            if (employee == null)
            {
                return Unauthorized("Invalid credentials");

            } else
            {

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,employee.EmployeeId.ToString()),
                        new Claim(ClaimTypes.Name,employee.Username),
                        new Claim(ClaimTypes.Role,employee.Role)

                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok("Logged in successfully");
            }

        }

        //[HttpPost("employees")]
        //[Authorize(Roles = "SUPERADMIN,MANAGER")]
        //public IActionResult AddEmployee([FromBody] EmployeeDTO employeeDTO)
        //{
        //    int ManagerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //    _employeeService.AddEmployee(employeeDTO,ManagerId);
        //    return Ok();

        //}



        [HttpPost("employees")]
        [Authorize(Roles = "MANAGER")]
        public IActionResult AddEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            int ManagerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _employeeService.AddEmployee(employeeDTO, ManagerId);
            return Ok();

        }


        [HttpPost("managers")]
        [Authorize(Roles = "SUPERADMIN")]
        public IActionResult AddManager([FromBody] EmployeeDTO employeeDTO)
        {
            _employeeService.AddManager(employeeDTO);
            return Ok();

        }

        //[HttpGet("employees/managers")]
        //[Authorize(Roles = "SUPERADMIN")]
        //public IActionResult GetManagers()
        //{

        //    var managers = _employeeService.GetManagers();
        //    return Ok(managers);

        //}


        [HttpGet("employees")]
        [Authorize(Roles = "MANAGER")]
        public IActionResult GetEmployees()
        {

            var ManagerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var employees = _employeeService.GetEmployees(int.Parse(ManagerId));
            return Ok(employees);


        }

        [HttpGet("employees/{EmployeeId}")]
        [Authorize(Roles ="EMPLOYEE")]
        public IActionResult GetEmployeeById([FromRoute] int EmployeeId)
        {
            var CurrentEmployeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (EmployeeId != CurrentEmployeeId)
            {
                return Unauthorized("You do not have permission to view the details of this Employee!");
            }

            var employee = _employeeService.GetEmployeeById(EmployeeId);
            return Ok(employee);

        }


        [HttpPost("employees/{EmployeeId}/leaves")]
        [Authorize(Roles ="EMPLOYEE")]
        public IActionResult ApplyLeave([FromRoute] int EmployeeId)
        {
            var CurrentEmployeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (EmployeeId != CurrentEmployeeId)
            {
                return Unauthorized("Invalid Employee Id");
            }

            _employeeService.ApplyLeave(CurrentEmployeeId);

            return Ok();
        }



        [HttpPost("employees/{EmployeeId}/leaves/{LeaveId}")]
        [Authorize(Roles = "MANAGER")]
        public IActionResult ApproveLeave([FromRoute] int LeaveId, [FromRoute] int EmployeeId)
        {

            var CurrentEmployeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var Employee = _employeeService.GetEmployeeById(EmployeeId);

            if (Employee.ManagerID != CurrentEmployeeId)
            {
                return Unauthorized("Employee not under you!");
            }

            _employeeService.ApproveLeave(Employee,LeaveId);

            return Ok();


        }
        

    }
}
