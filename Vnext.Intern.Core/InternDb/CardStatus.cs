using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vnext.Intern.InternDb
{
    [Table("CardStatus")]
    public class CardStatus:Entity<int>
    {
        [Column("CardStatusId")]
        public override int Id { get; set; }
        [Column("CardStatusTitle")]
        public string CardStatusTitle { get; set; }
        [Column("OrderNo")]
        public int OrderNo { get; set; }
        [Column("CreateBy")]
        public string CreateBy { get; set; }
        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}
