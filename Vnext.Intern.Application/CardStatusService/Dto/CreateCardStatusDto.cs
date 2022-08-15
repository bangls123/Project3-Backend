using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardStatusService.Dto
{
    [AutoMapTo(typeof(CardStatus))]
    public class CreateCardStatusDto
    {
        [Required]
        public string CardStatusTitle { get; set; }
        [Required]
        public int OrderNo { get; set; }
    }
}
