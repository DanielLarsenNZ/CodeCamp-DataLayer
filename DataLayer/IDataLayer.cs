using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataLayer<T> where T : Model
    {
        Task<T> Create(T item);
        Task<T> Get(string id);
        Task<IEnumerable<T>> GetAll();
        Task Remove(T item);
        Task<T> Update(T item);
    }
}