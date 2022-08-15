using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.LabelService.Dto
{
    [AutoMapFrom(typeof(Label))]
    public class LabelDto : EntityDto<int>
    {
        public string LabelName { get; set; }
        public string Color { get; set; }
    }
}
