using System.ComponentModel.DataAnnotations;

namespace API275.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50)]
        public string? EmployeeName { get; set; }
        [Required]
        public decimal Salary { get; set; }


    }
}
