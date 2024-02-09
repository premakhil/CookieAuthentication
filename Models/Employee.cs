namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int ManagerID { get; set; }

        public List<Leave> Leaves { get; set; }
        
       
    }
}

