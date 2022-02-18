using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservasAereas.Core.Entities;

namespace ReservasAereas.Core.Interfaces
{
    public interface IAirlineRepository
    {
        Task<DataTable> GetAll();
        Task<Airlines> GetById(int id);
        Task<int> Save(Airlines airlines);
        Task Edit(Airlines airlines);
        Task Delete(int id);
    }
}
