using CmStore.Data.Contexts;
using CmStore.Domain.Models.Vendedores;

namespace CmStore.Data.Repositories
{
    public class VendedorRepository : Repository<Vendedor>, IVendedorRepository
    {
        public VendedorRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
