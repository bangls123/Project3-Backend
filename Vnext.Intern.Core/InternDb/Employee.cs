using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vnext.Intern.InternDb
{
    [Table("Employee")]
    public class Employee : Entity<int>
    {
		[Column("EmployeeId")]
		public override int Id { get; set; }
		[Column("DepartmentId")]
		public int DepartmentId { get; set; }
		[Column("EmployeeName")]
		public string EmployeeName { get; set; }
		[Column("Color")]
		public string Color { get; set; }
		[Column("Email")]
		public string Email { get; set; }
		[Column("Username")]
		public string Username { get; set; }
		[Column("Password")]
		public string Password { get; set; }
		[Column("Notes")]
		public string Notes { get; set; }
		[Column("CreateBy")]
		public string CreateBy { get; set; }
		[Column("CreateDate")]
		public DateTime CreateDate { get; set; }
		[Column("UpdateBy")]
		public string UpdateBy { get; set; }
		[Column("UpdateDate")]
		public DateTime? UpdateDate { get; set; }

    }
}

