using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.LabelService;
using Vnext.Intern.LabelService.Dto;

namespace Vnext.Intern.Controller
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class LabelController : AbpApiController
    {
        public ILabelAppService _labelAppService;

        public LabelController(ILabelAppService labelAppService)
        {
            _labelAppService = labelAppService;
        }

        [HttpGet]
        public List<LabelDto> GetList(string keyword = "")
        {
            return _labelAppService.GetList(keyword);
        }

        [HttpPost]
        public Task<LabelDto> Create(CreateLabelDto input)
        {
            return _labelAppService.Create(input);
        }
    }
}
