using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    /// Defines the contract for retrieving, creating, updating, and deleting clients from the system.
    public interface IClientRepository
    {
        List<Client> GetAllClients(); /// Retrieves all clients from the data source.
        Client GetClient(string dni); /// Retrieves a specific client by their unique DNI.
        void RegisterClient(Client client); /// Registers (creates) a new client in the system.
        void UpdateClient(Client client); /// Updates the details of an existing client.
        void DeleteClient(string dni); /// Deletes a client from the system using their DNI.
    }
}
