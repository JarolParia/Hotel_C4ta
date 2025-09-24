using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public List<Client> GetAllClients() => _clientRepository.GetAllClients();
        public Client GetClient(string dni) => _clientRepository.GetClient(dni);
        public void RegisterClient(Client client) => _clientRepository.RegisterClient(client);
        public void UpdateClient(Client client) => _clientRepository.UpdateClient(client);
        public void DeleteClient(string dni) => _clientRepository.DeleteClient(dni);
    }
}
