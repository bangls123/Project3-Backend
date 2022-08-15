using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using System;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.DepartmentService.Dto
{
	[AutoMapTo(typeof(Department))]
	public class UpdateDepartmentDto : EntityDto<int>
	{
		[Required]
		public string DepartmentName { get; set; }
		public string Notes { get; set; }

	}
}

