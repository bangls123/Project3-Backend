using Abp.Application.Services;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.DepartmentService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.DepartmentService
{
    public interface IDepartmentAppService : IApplicationService
    {
        [HttpPost]
        Task<DepartmentDto> Create(CreateDepartmentDto input);

        [HttpGet]
        PageResultDto<DepartmentDto> GetList(int skipCount, int maxResutlCount, string keyword = null);

        [HttpGet]
        Task<DepartmentDto> GetDetail(int id);

        [HttpPut]
        Task<DepartmentDto> Update(UpdateDepartmentDto input);

        [HttpDelete]
        Task<ResultDto> Delete(int id);

        [HttpPost]
        ReportResultDto Upload();

        [HttpGet]
        ReportResultDto Download();
    }
}
