using Abp.Application.Services;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardCommentService.Dto;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.CardCommentService
{
    public interface ICardCommentAppService : IApplicationService
    {
        [HttpPost]
        Task<CardCommentDto> Create (CreateCardCommentDto input);

        [HttpDelete]
        Task<ResultDto> Delete(int id);

        [HttpPut]
        Task<CardCommentDto> Update(UpdateCardCommentDto input);
    }
}
