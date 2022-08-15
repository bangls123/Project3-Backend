using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Threading.Tasks;
using System.Web.Http;

using Vnext.Intern.DepartmentService;
using Vnext.Intern.DepartmentService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.Controller
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class DepartmentController : AbpApiController
    {
        public IDepartmentAppService _departmentAppService;

        public DepartmentController(IDepartmentAppService departmentAppService)
        {
            _departmentAppService = departmentAppService;
        }

        [HttpPost]
        public Task<DepartmentDto> Create(CreateDepartmentDto input)
        {
            return _departmentAppService.Create(input);
        }

        [HttpGet]
        public PageResultDto<DepartmentDto> GetList(int skipCount, int maxResultCount, string keyword = "")
        {
            return _departmentAppService.GetList(skipCount, maxResultCount, keyword);
        }

        [HttpGet]
        public Task<DepartmentDto> Get(int id)
        {
            return _departmentAppService.GetDetail(id);
        }

        [HttpPut]
        public Task<DepartmentDto> Update(UpdateDepartmentDto input)
        {
            return _departmentAppService.Update(input);
        }

        [HttpDelete]
        public Task<ResultDto> Delete(int id)
        {
            return _departmentAppService.Delete(id);
        }

        [HttpGet]
        public ReportResultDto Download()
        {
            return _departmentAppService.Download();
        }

        [HttpPost]
        public ReportResultDto Upload()
        {
            return _departmentAppService.Upload();
        }
    }
}
