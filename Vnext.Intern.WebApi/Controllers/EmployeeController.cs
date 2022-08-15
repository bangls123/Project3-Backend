using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Threading.Tasks;
using System.Web.Http;

using Vnext.Intern.EmployeeService;
using Vnext.Intern.EmployeeService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.Controller
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class EmployeeController : AbpApiController
    {
        public IEmployeeAppService _employeeAppService;

        public EmployeeController(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        [HttpPost]
        public Task<EmployeeResponeDto> Create(CreateEmployeeDto input)
        {
            return _employeeAppService.Create(input);
        }

        [HttpGet]
        public PageResultDto<EmployeeDto> GetList(int skipCount, int maxResultCount, string keyword = "")
        {
            return _employeeAppService.GetList(skipCount, maxResultCount, keyword);
        }

        [HttpGet]
        public Task<EmployeeResponeDto> Get(int id)
        {
            return _employeeAppService.GetDetail(id);
        }

        [HttpPut]
        public Task<EmployeeResponeDto> Update(UpdateEmployeeDto input)
        {
            return _employeeAppService.Update(input);
        }

        [HttpDelete]
        public Task<ResultDto> Delete(int id)
        {
            return _employeeAppService.Delete(id);
        }

        [HttpGet]
        public ReportResultDto Download()
        {
            return _employeeAppService.Download();
        }

        [HttpPost]
        public ReportResultDto Upload()
        {
            return _employeeAppService.Upload();
        }
    }
}
