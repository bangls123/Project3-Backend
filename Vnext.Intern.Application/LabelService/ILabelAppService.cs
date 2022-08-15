using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.LabelService.Dto;

namespace Vnext.Intern.LabelService
{
    public interface ILabelAppService : IApplicationService
    {
        [HttpPost]
        Task<LabelDto> Create(CreateLabelDto input);

        [HttpGet]
        List<LabelDto> GetList(string keyword);
    }
}
