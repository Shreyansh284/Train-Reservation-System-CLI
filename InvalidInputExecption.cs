using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Reservation_System_CLI
{
    internal class InvalidInputExecption : Exception
    {
        public InvalidInputExecption(string message) : base(message)
        { }

    }
}
