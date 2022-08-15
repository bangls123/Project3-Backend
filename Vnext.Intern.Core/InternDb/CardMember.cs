using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vnext.Intern.InternDb
{
    [Table("CardMember")]
    public class CardMember : Entity<int>
    {
        [Column("CardMemberId")]
        public override int Id { get; set; }
        [Column("CardId")]
        public int CardId { get; set; }
        [Column("EmployeeId")]
        public int EmployeeId { get; set; }
        [Column("CreateBy")]
        public string CreateBy { get; set; }
        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}
