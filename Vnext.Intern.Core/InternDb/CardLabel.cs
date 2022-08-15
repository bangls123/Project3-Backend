using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vnext.Intern.InternDb
{
    [Table("CardLabel")]
    public class CardLabel : Entity<int>
    {
        [Column("CardLabelId")]
        public override int Id { get; set; }
        [Column("CardId")]
        public int CardId { get; set; }
        [Column("LabelId")]
        public int LabelId { get; set; }
        [Column("CreateBy")]
        public string CreateBy { get; set; }
        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}
