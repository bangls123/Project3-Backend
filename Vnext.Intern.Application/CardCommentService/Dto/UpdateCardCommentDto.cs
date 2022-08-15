using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardCommentService.Dto
{
    [AutoMapTo(typeof(CardComment))]
    public class UpdateCardCommentDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Detail { get; set; }
    }
}