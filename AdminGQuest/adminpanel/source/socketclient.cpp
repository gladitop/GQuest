#include <head/socketclient.h>
#include <head/data.h>

void SocketClient::connect()
{
    client = new QTcpSocket();
    client->connectToHost(data::IPServer, data::PortServer);
}
