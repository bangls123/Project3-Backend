using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardService.Dto
{
    [AutoMapFrom(typeof(Card))]
    public class CardDto : EntityDto<int>
    {
        public int CardStatusId { get; set; }
        public string CardTitle { get; set; }
        public string Descriptions { get; set; }
        public int OrderNo { get; set; }
        public string CardStatusTitle { get; set; }
        public List<CardMemberDto> CardMembers { get; set; } = new List<CardMemberDto>();
        public List<CardLabelDto> CardLabels { get; set; } = new List<CardLabelDto> { };
        public List<CardCommentDto> CardComments { get; set; } = new List<CardCommentDto> { };
    }
}
