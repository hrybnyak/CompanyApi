using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IService<T> where T: BaseDTO
    {
        T Create(T item);
        void Delete(T item);
        void Update(T item);
        IEnumerable<T> GetAll();
        T GetById(int? id);
    }
}
