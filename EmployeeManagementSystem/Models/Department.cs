using EmployeeManagementSystem.SharedKernel;
using System.Collections.Generic;

namespace EmployeeManagementSystem.Models
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
