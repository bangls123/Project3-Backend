using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Vnext.Intern.CardStatusService.Dto;

namespace Vnext.Intern.CardStatusService
{
    public interface ICardStatusAppService : IApplicationService
    {
        [HttpPost]
        Task<CardStatusDto> Create(CreateCardStatusDto input);
        [HttpPut]
        Task<CardStatusDto> Update(UpdateCardStatusDto input);
        [HttpGet]
        List<CardStatusRespone> GetList(string keyword);
        [HttpGet]
        List<CardStatusRespone> GetListLinq(string keyword);
        [HttpGet]
        List<CardStatusRespone> GetMyPage(int employeeId);
        [HttpGet]
        List<CardStatusRespone> GetMyPageLinq(int employeeId);
    }
}
