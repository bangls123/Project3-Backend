using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardMemberService.Dto
{
    [AutoMapFrom(typeof(CardMember))]
    public class CardMemberDto : EntityDto<int>
    {
        public int CardId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Color { get; set; }
    }
}
