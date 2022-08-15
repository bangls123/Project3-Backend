using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Vnext.Intern.InternDb
{
    [Table("Department")]
    public class Department :Entity<int>
    {
        [Column("DepartmentId")]
        public override int Id { get; set; }
        [Column("DepartmentName")]
        public string DepartmentName { get; set; }
        [Column("Notes")]
        public string Notes { get; set; }
        [Column("CreateBy")]
        public string CreateBy { get; set; }
        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}
