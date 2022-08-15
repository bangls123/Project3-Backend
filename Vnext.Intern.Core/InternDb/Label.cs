using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vnext.Intern.InternDb
{
    [Table("Label")]
    public class Label : Entity<int>
    {
        [Column("LabelId")]
        public override int Id { get; set; }
        [Column("LabelName")]
        public string LabelName { get; set; }
        [Column("Color")]
        public string Color { get; set; }
        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
        [Column("CreateBy")]
        public string CreateBy { get; set; }
    }
}
