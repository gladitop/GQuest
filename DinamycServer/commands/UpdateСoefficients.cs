using System.Net.Sockets;
using System;

namespace DinamycServer
{
    public partial class Commands
    {
        private void UCOEF(TcpClient client, string[] arg)
        {
            Database.UpdateCoefficients(Convert.ToInt64(arg[0]), $"{arg[1]}:{arg[2]}:{arg[3]}:{arg[4]}:{arg[5]}:{arg[6]}");         
        }
    }
}