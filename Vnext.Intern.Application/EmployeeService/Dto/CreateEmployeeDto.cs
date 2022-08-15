using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.EmployeeService.Dto
{
	[AutoMapTo(typeof(Employee))]
	public class CreateEmployeeDto
	{
		[Required]
		public string EmployeeName { get; set; }
		[Required]
		public int DepartmentId { get; set; }
		public string Color { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string ConfirmPassword { get; set; }
		public string Notes { get; set; }
	}
}