using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
namespace EmployeeManagement.Services
{
    public class EmployeeService
    {
        public List<Employee> employees = new List<Employee>();

        public EmployeeService() 
        {
            AddSuperAdmin();
        }

        public  void AddEmployee(EmployeeDTO employeeDTO,int ManagerId)
        {


            Employee employee = new Employee
            {
                Username = employeeDTO.Username,
                Password = employeeDTO.Password,
                Role = employeeDTO.Role,
                ManagerID = ManagerId,
                EmployeeId = employees.Count() + 1,
                Leaves = new List<Leave>()
                
            };


            employees.Add(employee);

        }



        public void AddSuperAdmin()
        {
            Employee admin = new Employee
            {
                Username = "Akhil",
                Password = "Hey",
                Role = "SUPERADMIN",
                ManagerID = 0,
                EmployeeId = 1
            };

            employees.Add(admin);
        }



        //remove role here if code fails!!!
        public IEnumerable<Employee> GetEmployees(int ManagerId)
        {
            return employees.Where(e => e.ManagerID == ManagerId && e.Role=="EMPLOYEE");

        }

        public Employee GetEmployeeById(int EmployeeId)
        {
            return employees.Where(e => e.EmployeeId == EmployeeId && e.Role=="EMPLOYEE").FirstOrDefault();
        }

        public void ApplyLeave(int CurrentEmployeeId)
        {
            var employee = GetEmployeeById(CurrentEmployeeId);

            Leave leave = new Leave 
            {
                LeaveId = employee.Leaves.Count()+1,
                IsApproved = false,
                ManagerId = employee.ManagerID
            };

            employee.Leaves.Add(leave);



        }


        public void ApproveLeave(Employee employee, int LeaveId)
        {
            employee.Leaves.Where(x => x.LeaveId == LeaveId).FirstOrDefault().IsApproved = true;
        }
    }
}
