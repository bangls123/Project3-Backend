using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardService;
using Vnext.Intern.CardService.Dto;

namespace Vnext.Intern.Controllers
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class CardController : AbpApiController
    {
        public ICardAppService _cardAppService;

        public CardController(ICardAppService cardAppService)
        {
            _cardAppService = cardAppService;
        }

        [HttpPost]
        public Task<CardDto> Create(CreateCardDto input)
        {
            return _cardAppService.Create(input);
        }

        [HttpGet]
        public Task<CardDto> GetDetail(int id)
        {
            return _cardAppService.GetDetail(id);
        }

        [HttpPut]
        public Task<CardDto> Update(UpdateCardDto input)
        {
            return _cardAppService.Update(input);
        }
    }
}
