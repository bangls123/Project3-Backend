using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vnext.Intern.InternDb
{
	[Table("CardComment")]
	public class CardComment : Entity<int>
	{
		[Column("CardCommentId")]
		public override int Id { get; set; }
		[Column("CardId")]
		public int CardId { get; set; }
		[Column("EmployeeId")]
		public int EmployeeId { get; set; }
		[Column("Detail")]
		public string Detail { get; set; }
		[Column("CreateDate")]
		public DateTime CreateDate { get; set; }
		[Column("UpdateDate")]
		public DateTime? UpdateDate { get; set; }


	}
}

