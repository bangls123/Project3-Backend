using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.EmployeeService.Dto
{
	[AutoMapFrom(typeof(Employee))]
	public class EmployeeDto : EntityDto<int>
	{
		public int DepartmentId { get; set; }
		public string EmployeeName { get; set; }
		public string Color { get; set; }
		public string Notes { get; set; }
        public string DepartmentName { get; set; }
		public string Username { get; set; }
	}
}
