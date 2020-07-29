#include <head/login.h>
#include <head/socketclient.h>
#include <ui_login.h>
#include <QMessageBox>
#include <head/data.h>
#include <QTcpSocket>

login::login(QWidget *parent)//Загрузка самого приложения
    : QMainWindow(parent)
    , ui(new Ui::login)
{
    ui->setupUi(this);
}

login::~login()//Закрытие
{
    delete ui;
}


void login::on_pushButton_clicked()
{
    if(ui->lineEdit->text().size() >= 10)
    {
        QMessageBox::critical(this, windowTitle(), "Ошибка: Слишком длинный логин!");
    }
    else if(ui->lineEdit_2->text().size() >= 10)
    {
        QMessageBox::critical(this, windowTitle(), "Ошибка: Слишком длинный пароль!");
    }
    else if (ui->lineEdit->text() == "")
    {
        QMessageBox::critical(this, windowTitle(), "Ошибка: Без логина нельзя!");
    }
    else if (ui->lineEdit_2->text() == "")
    {
        QMessageBox::critical(this, windowTitle(), "Ошибка: Без пароля нельзя!");
    }
    else
    {
        try {
        SocketClient client;
        QMessageBox::critical(this, "", "Ошибка45334!");
        client.connect();
        QMessageBox::critical(this, "", "Ошибка45334534567!");
        } catch (...) {//TODO:Исправить!
            QMessageBox::critical(this, "", "Ошибка: Без пароля нельз234!");
        }
    }
}
