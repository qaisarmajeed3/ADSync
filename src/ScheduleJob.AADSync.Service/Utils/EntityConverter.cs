using  ScheduleJob.Domain.Interface;

namespace  ScheduleJob.Service.Utils
{
    /// <summary>
    /// Entity converter class.
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    public class EntityConverter<TE> where TE : IEntity
    {
        protected EntityConverter()
        {
        }

        protected EntityConverter(TE entity)
        {
            if (entity != null)
            {
                foreach (var propertyInfo in entity.GetType().GetProperties().ToArray())
                {
                    if (GetType().GetProperty(propertyInfo.Name) != null)
                    {
                        GetType()?.GetProperty(propertyInfo.Name)?.SetValue(this, propertyInfo.GetValue(entity, null));
                    }
                }
            }
        }

        /// <summary>
        /// Method to map data to class.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TE? Hydrate(TE entity)
        {
            if (entity != null)
            {
                Type type = this.GetType();
                foreach (var propertyInfo in type.GetProperties().ToArray())
                {
                    if (entity.GetType().GetProperty(propertyInfo.Name) != null)
                    {
                        entity.GetType()?.GetProperty(propertyInfo.Name)?.SetValue(entity, propertyInfo.GetValue(this, null));
                    }
                }
            }

            return entity;
        }
    }
}