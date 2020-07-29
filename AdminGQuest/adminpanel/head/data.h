#ifndef DATA_H
#define DATA_H
#include <QApplication>
#include <head/socketclient.h>

class data
{
public:
    static QString IPServer;//Ip сервера
    static int PortServer;//Порт сервера
    static QString Version;//Версия клиента
    static SocketClient client;//Наш сокет в месте с API (create Gladi)
};

#endif // DATA_H
