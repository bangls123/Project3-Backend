using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.DepartmentService.Dto
{
    [AutoMapFrom(typeof(Department))]
    public class DepartmentDto:EntityDto<int>
    {
        public string DepartmentName { get; set; }
        public string Notes { get; set; }
    }
}
