using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardStatusService.Dto
{
    [AutoMapFrom(typeof(CardStatus))]
    public class CardStatusRespone:EntityDto<int>
    {
        public string CardStatusTitle { get; set; }
        public int OrderNo { get; set; }
        public List<CardsDto> Cards { get; set; }

    }
}
