using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Utils
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string RoleCode { get; set; }
        public string PasswordHashed { get; set; }
    }
}
