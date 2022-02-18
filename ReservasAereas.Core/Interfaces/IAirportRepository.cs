using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservasAereas.Core.Entities;

namespace ReservasAereas.Core.Interfaces
{
    public interface IAirportRepository
    {
        Task<DataTable> GetAll();
        Task<Airports> GetById(int id);
        Task<int> Save(Airports airports);
        Task Edit(Airports airports);
        Task Delete(int id);
    }
}
