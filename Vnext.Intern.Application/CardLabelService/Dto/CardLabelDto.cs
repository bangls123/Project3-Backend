using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardLabelService.Dto
{
    [AutoMapFrom(typeof(CardLabel))]
    public class CardLabelDto : EntityDto<int>
    {
        public int CardId { get; set; }
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public string Color { get; set; }
    }
}
