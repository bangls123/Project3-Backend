using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardMemberService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.CardMemberService
{
    public interface ICardMemberAppService : IApplicationService
    {
        [HttpPost]
        Task<CardMemberDto> Create(CreateCardMemberDto input);

        [HttpDelete]
        Task<ResultDto> Delete(int id);

        [HttpGet]
        List<CardMemberDto> GetList(int cardId);
    }
}

