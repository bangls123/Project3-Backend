using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Vnext.Intern.CardStatusService.Dto
{
    public class CardsDto : EntityDto<int>
    {
        public string CardTitle { get; set; }
        public int OrderNo { get; set; }
        public List<CardMemberDto> CardMembers { get; set; }
        public List<CardLabelDto> CardLabels { get; set; }
    }
}
