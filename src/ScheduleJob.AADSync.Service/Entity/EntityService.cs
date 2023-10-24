using  ScheduleJob.Domain.Interface;
using  ScheduleJob.Repository.Interface;
using  ScheduleJob.Service.Interface;

namespace  ScheduleJob.Service.Entity
{
    /// <summary>
    /// Class for handling base service operations.
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    public class EntityService<TE>: IEntityService<TE>
    {
        private readonly IEntityRepository<TE> _entityRepository;

        /// <summary>
        /// Constructor of <see cref="EntityService"/>
        /// </summary>
        /// <param name="entityRepository">Instance of <see cref="IEntityRepository{TE}"/></param>
        public EntityService(IEntityRepository<TE> entityRepository)
        {
            this._entityRepository = entityRepository;
        }

        /// <inheritdoc />
        public async Task<TE> Create(TE instance)
        {
            return await this._entityRepository.Create(instance);
        }

        /// <inheritdoc />
        public async Task<TE> Get(string id)
        {
            return await this._entityRepository.Get(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TE>> GetAll()
        {
            return await this._entityRepository.GetAll();
        }

        /// <inheritdoc />
        public async Task<TE> Update(TE instance)
        {
            return await this._entityRepository.Update(instance);
        }
    }
}
