using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardMemberService.Dto
{
	[AutoMapTo(typeof(CardMember))]
	public class CreateCardMemberDto
	{
		[Required]
		public int CardId { get; set; }

		[Required]
		public int EmployeeId { get; set; }
	}
}