using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

using Vnext.Intern.EntityFramework.InternDb;

namespace Vnext.Intern.EntityFramework.Repositories
{
    public abstract class InternRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<InternDbDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected InternRepositoryBase(IDbContextProvider<InternDbDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class InternRepositoryBase<TEntity> : InternRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected InternRepositoryBase(IDbContextProvider<InternDbDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}

