using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardMemberService;
using Vnext.Intern.CardMemberService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.Controllers
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class CardMemberController : AbpApiController
    {
        public ICardMemberAppService _cardMemberAppService;

        public CardMemberController(ICardMemberAppService cardMemberAppService)
        {
            _cardMemberAppService = cardMemberAppService;
        }

        [HttpPost]
        public Task<CardMemberDto> Create(CreateCardMemberDto input)
        {
            return _cardMemberAppService.Create(input);
        }

        [HttpDelete]
        public Task<ResultDto> Delete(int id)
        {
            return _cardMemberAppService.Delete(id);
        }

        [HttpGet]
        public List<CardMemberDto> GetList(int cardId)
        {
            return _cardMemberAppService.GetList(cardId);
        }
    }
}
