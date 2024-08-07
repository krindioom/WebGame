using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGame.Domain;

public interface IRepository<T, TId>
{
    Task<T> GetByIdAsync(TId id);

    Task CreateAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(TId id);
}
