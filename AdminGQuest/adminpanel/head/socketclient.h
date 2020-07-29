#ifndef SOCKETCLIENT_H
#define SOCKETCLIENT_H
#include <QTcpSocket>

class SocketClient
{
public:
    void connect();//Подключение
    void disconnect();//Отключение
private:
    QTcpSocket* client;//Сам клиент
};

#endif // SOCKETCLIENT_H
