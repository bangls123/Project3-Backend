using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vnext.Intern.InternDb
{
    [Table("Card")]
    public class Card : Entity<int>
    {
        [Column("CardId")]
        public override int Id { get; set; }
        [Column("CardStatusId")]
        public int CardStatusId { get; set; }
        [Column("CardTitle")]
        public string CardTitle { get; set; }
        [Column("Descriptions")]
        public string Descriptions { get; set; }
        [Column("OrderNo")]
        public int OrderNo { get; set; }
        [Column("CreateBy")]
        public string CreateBy { get; set; }
        [Column("CreateDate")]
        public DateTime CreateDate  { get; set; }
        [Column("UpdateBy")]
        public string UpdateBy { get; set; }
        [Column("UpdateDate")]
        public DateTime? UpdateDate { get; set; }
    }
}
