namespace CmStore.Domain.Models.Base
{
    public class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid(); 
        }

        public Guid Id { get; set; }
    }
}
