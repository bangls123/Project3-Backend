using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardStatusService;
using Vnext.Intern.CardStatusService.Dto;

namespace Vnext.Intern.Controller
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class CardStatusController : AbpApiController
    {
        public ICardStatusAppService _cardStatusAppService;

        public CardStatusController(ICardStatusAppService cardStatusAppService)
        {
            _cardStatusAppService = cardStatusAppService;
        }

        [HttpGet]
        public List<CardStatusRespone> GetList(string keyword = "")
        {
            return _cardStatusAppService.GetList(keyword);
        }

        [HttpPost]
        public Task<CardStatusDto> Create(CreateCardStatusDto input)
        {
            return _cardStatusAppService.Create(input);
        }

        [HttpPut]
        public Task<CardStatusDto> Update(UpdateCardStatusDto input)
        {
            return _cardStatusAppService.Update(input);
        }

        [HttpGet]
        public List<CardStatusRespone> Get(int id)
        {
            return _cardStatusAppService.GetMyPage(id);
        }
    }
}
