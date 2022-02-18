using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservasAereas.Core.Entities;

namespace ReservasAereas.Core.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Bookings>> GetAll(string airportOrigin, string airportDestination, string airlinenName, string flightNumber);
        Task<Bookings> GetById(int id);
        Task<int> Save(Bookings bookings);
        Task Delete(int id);
    }
}
