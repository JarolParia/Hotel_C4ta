using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Entities
{
    /// Represents a system user (administrator or receptionist).
    public class User
    {
        public int ID { get; set; } /// Unique identifier of the user.
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public string Rol { get; set; } /// Role of the user in the system (Admin or Recep)
    }
}
