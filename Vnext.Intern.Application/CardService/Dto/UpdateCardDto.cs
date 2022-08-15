using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardService.Dto
{
	[AutoMapTo(typeof(Card))]
	public class UpdateCardDto : EntityDto<int>
	{
		[Required]
		public int CardStatusId { get; set; }

		[Required]
		public string CardTitle { get; set; }
		public string Descriptions { get; set; }
	}
}

