using EmployeeManagementSystem.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public double Salary { get; set; }      

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth  { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public string ShortCode { get; set; }
        public string ImageUrl { get; set; }
    }
}
