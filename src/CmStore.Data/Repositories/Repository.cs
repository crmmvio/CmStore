using CmStore.Data.Contexts;
using CmStore.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CmStore.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly AppDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet; 

        protected Repository(AppDbContext context)
        {
            DbContext = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity != null)
            {
                _ = DbSet.Remove(entity);
                await SaveChanges();
            }
        }

        public async Task<int> SaveChanges()
        {
            return await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
