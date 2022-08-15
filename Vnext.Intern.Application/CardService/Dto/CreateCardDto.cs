using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardService.Dto
{
	[AutoMapTo(typeof(Card))]
	public class CreateCardDto
	{
		[Required]
		public int CardStatusId { get; set; }

        [Required]
		public string CardTitle { get; set; }
	}
}