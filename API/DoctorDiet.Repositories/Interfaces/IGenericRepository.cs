using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Repository.Interfaces
{
    public interface IGenericRepository<T,Y>
    {
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        T GetByID(Y id);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Update(T entity, params string[] properties);
        void Delete(Y id);
    }
}
