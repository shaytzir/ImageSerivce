using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    interface ITcpAndroidHandler
    {
        void HandleClient();
        void Close();
    }
}
