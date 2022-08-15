using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardCommentService;
using Vnext.Intern.CardCommentService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.Controllers
{
    [DontWrapResult(WrapOnSuccess = true, WrapOnError = true)]
    public class CardCommentController : AbpApiController
    {
        public ICardCommentAppService _cardCommentAppService;
        public CardCommentController(ICardCommentAppService cardCommentAppService)
        {
            _cardCommentAppService = cardCommentAppService;
        }
        [HttpPost]
        public Task<CardCommentDto> Create(CreateCardCommentDto input)
        {
            return _cardCommentAppService.Create(input);
        }
        [HttpDelete]
        public Task<ResultDto> Delete(int id)
        {
            return _cardCommentAppService.Delete(id);
        }
        [HttpPut]
        public Task<CardCommentDto> Update(UpdateCardCommentDto input)
        {
            return _cardCommentAppService.Update(input);
        }
    }
}
