using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardStatusService.Dto
{
    [AutoMapFrom(typeof(CardStatus))]
    public class UpdateCardStatusDto : EntityDto<int>
    {
        [Required]
        public string CardStatusTitle { get; set; }
        [Required]
        public int OrderNo { get; set; }
    }
}
