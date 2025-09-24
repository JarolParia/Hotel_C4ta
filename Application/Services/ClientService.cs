using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    /// Service layer for managing operations related to clients.
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        // Constructor receives an implementation of IClientRepository.
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Retrieves all clients from the repository.
        public List<Client> GetAllClients() => _clientRepository.GetAllClients();

        // Retrieves a specific client based on their DNI.
        public Client GetClient(string dni) => _clientRepository.GetClient(dni);

        // Registers (creates) a new client in the system.
        public void RegisterClient(Client client) => _clientRepository.RegisterClient(client);

        // Updates the information of an existing client.
        public void UpdateClient(Client client) => _clientRepository.UpdateClient(client);

        // Deletes a client from the system using their DNI.
        public void DeleteClient(string dni) => _clientRepository.DeleteClient(dni);
    }
}
