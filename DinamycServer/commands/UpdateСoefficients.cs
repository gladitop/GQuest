using System;
using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void UCOEF(TcpClient client, string[] arg)
        {
            Database.UpdateCoefficients(Convert.ToInt64(arg[0]), arg[1]);
            Database.UpdateLevel(Convert.ToInt64(arg[0]), "1");
        }
    }
}