using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardLabelService;
using Vnext.Intern.CardLabelService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.Controllers
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class CardLabelController : AbpApiController
    {
        public ICardLabelAppService _cardLabelAppService;
        public CardLabelController(ICardLabelAppService cardCommentAppService)
        {
            _cardLabelAppService = cardCommentAppService;
        }
        [HttpPost]
        public Task<CardLabelDto> Create(CreateCardLabelDto input)
        {
            return _cardLabelAppService.Create(input);
        }
        [HttpDelete]
        public Task<ResultDto> Delete(int id)
        {
            return _cardLabelAppService.Delete(id);
        }
        [HttpGet]
        public List<CardLabelDto> GetList(int id)
        {
            return _cardLabelAppService.GetList(id);
        }
    }
}