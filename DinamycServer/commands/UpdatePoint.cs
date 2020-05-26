using System.Net.Sockets;

namespace DinamycServer
{
    public partial class Commands
    {
        private void UPOINT(TcpClient client, string[] arg)
        {
            Database.UpdatePoint(int.Parse(arg[0]), long.Parse(arg[1]));
        }
    }
}