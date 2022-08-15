using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.DepartmentService.Dto
{
	[AutoMapTo(typeof(Department))]
	public class CreateDepartmentDto
	{
		[Required]
		public string DepartmentName { get; set; }
		public string Notes { get; set; }
	}
}