using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Models.Interface
{
    public interface IBaseModel<T>
    {
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}