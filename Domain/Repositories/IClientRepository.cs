using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    public interface IClientRepository
    {
        List<Client> GetAllClients();
        Client GetClient(string dni);
        void RegisterClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(string dni);
    }
}
