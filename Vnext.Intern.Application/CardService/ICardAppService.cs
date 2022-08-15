using Abp.Application.Services;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardService.Dto;

namespace Vnext.Intern.CardService
{
    public interface ICardAppService : IApplicationService
    {
        [HttpPost]
        Task<CardDto> Create(CreateCardDto input);

        [HttpGet]
        Task<CardDto> GetDetail(int id);

        [HttpGet]
        Task<CardDto> GetDetailLinq(int id);

        [HttpPut]
        Task<CardDto> Update(UpdateCardDto input);
    }
}
