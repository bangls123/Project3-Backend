using Abp.EntityFramework;
using System.Data.Entity;

using Vnext.Intern.InternDb;

namespace Vnext.Intern.EntityFramework.InternDb
{
    public class InternDbDbContext : AbpDbContext
    {
        public virtual IDbSet<Employee> Employees { get; set; }
        public virtual IDbSet<CardStatus> CardStatuss { get; set; }
        public virtual IDbSet<Department> Departments { get; set; }
        public virtual IDbSet<Card> Cards { get; set; }
        public virtual IDbSet<CardLabel> CardLabels { get; set; }
        public virtual IDbSet<CardMember> CardMembers { get; set; }
        public virtual IDbSet<Label> Labels { get; set; }
        public virtual IDbSet<CardComment> CardComments { get; set; }
        public InternDbDbContext()
            : base("InternDb")
        {

        }

        public InternDbDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}

