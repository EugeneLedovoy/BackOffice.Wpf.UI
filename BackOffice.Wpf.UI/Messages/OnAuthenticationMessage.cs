using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class OnAuthenticationMessage
    {
        public bool IsSuccess { get; }
        public string AuthenticateMessage { get; }

        public OnAuthenticationMessage(bool isSuccess, string authenticateMessage)
        {
            AuthenticateMessage = authenticateMessage;
            IsSuccess = isSuccess;
        }
    }
}
