using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardLabelService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.CardLabelService
{
    public interface ICardLabelAppService : IApplicationService
    {
        [HttpPost]
        Task<CardLabelDto> Create(CreateCardLabelDto input);

        [HttpDelete]
        Task<ResultDto> Delete(int id);

        [HttpGet]
        List<CardLabelDto> GetList(int cardId);
    }
}
