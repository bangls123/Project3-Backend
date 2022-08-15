using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.LabelService.Dto
{
	[AutoMapTo(typeof(Label))]
	public class CreateLabelDto
	{
		[Required]
		public string LabelName { get; set; }

        [Required]
		public string Color { get; set; }
	}
}