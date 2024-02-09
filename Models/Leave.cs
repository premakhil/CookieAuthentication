namespace EmployeeManagement.Models
{
    public class Leave
    {
        public int LeaveId { get; set; }
        public bool IsApproved { get; set; }
        public int ManagerId { get; set; }

    }
}
