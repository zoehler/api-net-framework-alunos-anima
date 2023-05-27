using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface IDatabaseRepositorio<T>
    {
        T GetById<T>(int id);
        List<T> Get<T>();
        int Insert<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(int id);
    }
}
