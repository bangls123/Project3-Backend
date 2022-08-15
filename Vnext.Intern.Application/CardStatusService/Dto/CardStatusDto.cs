using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardStatusService.Dto
{
    [AutoMapFrom(typeof(CardStatus))]
    public class CardStatusDto:EntityDto<int>
    {
        public string CardStatusTitle { get; set; }
        public int OrderNo { get; set; }

    }
}
