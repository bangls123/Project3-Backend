using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.EmployeeService.Dto
{
    [AutoMapTo(typeof(Employee))]
    public class UpdateEmployeeDto : EntityDto<int>
    {
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public string Color { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Notes { get; set; }
    }
}
