using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Exceptions
{
    public class NoSufficientFundsException : Exception
    {
        public NoSufficientFundsException(string message) : base(message)
        {
            
        }
    }
}
