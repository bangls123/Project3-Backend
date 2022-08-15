using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardCommentService.Dto
{
    [AutoMapFrom(typeof(CardComment))]
	public class CardCommentDto : EntityDto<int>
	{
		public int CardId { get; set; }
		public int EmployeeId { get; set; }
		public string Detail { get; set; }
	}
}
