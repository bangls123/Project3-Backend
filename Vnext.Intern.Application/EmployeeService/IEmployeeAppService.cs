using Abp.Application.Services;
using System.Threading.Tasks;
using System.Web.Http;

using Vnext.Intern.EmployeeService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.EmployeeService
{
    public interface IEmployeeAppService : IApplicationService
    {
        [HttpPost]
        PageResultDto<EmployeeDto> GetList(int skipCount, int maxResultCount, string keyword);
        [HttpDelete]
        Task<ResultDto> Delete(int id);

        [HttpPost]
        ReportResultDto Download();

        [HttpPost]
        ReportResultDto Upload();
        [HttpGet]
        Task<EmployeeResponeDto> GetDetail(int id);
        [HttpPost]
        Task<EmployeeResponeDto> Create(CreateEmployeeDto input);
        [HttpPut]
        Task<EmployeeResponeDto> Update(UpdateEmployeeDto input);
    }
}

