using System;

namespace Vnext.Intern.CardService.Dto
{
    public class CardCommentDto
    {
        public int EmployeeId { get; set; }
        public string Detail { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
