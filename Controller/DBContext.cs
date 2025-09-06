using Microsoft.Data.SqlClient;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Controller
{
    public class DBContext
    {
        protected readonly string _ConnectionString;

        public DBContext()
        {
           _ConnectionString = ConfigurationManager.ConnectionStrings["HotelC4TA"].ConnectionString;
        }
    }
}
