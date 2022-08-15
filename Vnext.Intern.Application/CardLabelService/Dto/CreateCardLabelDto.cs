using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardLabelService.Dto
{
	[AutoMapTo(typeof(CardLabel))]
	public class CreateCardLabelDto
	{
		[Required]
		public int CardId { get; set; }

		[Required]
		public int LabelId { get; set; }
	}
}