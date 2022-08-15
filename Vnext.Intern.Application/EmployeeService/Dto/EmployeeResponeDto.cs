using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.EmployeeService.Dto
{
	[AutoMapFrom(typeof(Employee))]
	public class EmployeeResponeDto : EntityDto<int>
	{
		public string EmployeeName { get; set; }
		public string DepartmentId { get; set; }
		public string Color { get; set; }
		public string Username { get; set; }
		public string Notes { get; set; }
	}
}