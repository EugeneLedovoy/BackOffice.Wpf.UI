using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class On2FAMessage
    {
        public On2FAMessage(bool isSuccess, string authenticateMessage)
        {
            IsSuccess = isSuccess;
            AuthenticateMessage = authenticateMessage;
        }

        public bool IsSuccess { get; }
        public string AuthenticateMessage { get; }

    }
}
