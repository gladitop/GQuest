using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public class Client : MonoBehaviour
{
    #region Соединение Клиент-сервер

    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private string message;

    private void Start()
    {
        ConnectedToServer();
    }
    public void ConnectedToServer()
    {
        try
        {

            //Defalt host / post values
            string host = "127.0.0.1";
            int port = 908;

            //Create the socket
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch
        {
            Debug.Log("Ну удалось подключиться к серверу");
        }
    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                socket.Client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer);
                OnIncomingData();
            }
        }
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
    private void CloseSocket()
    {     
        socketReady = false;
    }
    #endregion

    #region Получение / Отправка

    public void Send(string data) //отправка
    {
        if (!socketReady)
            return;

        socket.Client.Send(Encoding.UTF8.GetBytes(data));
        Debug.Log(data);
        writer.Flush();
    }
    private void OnIncomingData() //принятие
    {
        if (message != "")
        {
            Debug.Log(message); //Входящее сообщение
            var ch = ':'; //Разделяющий символ   
            try
            {
                var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                var arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] { ch }); //Массив аргументов
                this.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, new object[] { arguments });
            }
            catch (Exception ex) { Debug.Log(ex); }
        }
    }

    #endregion

    #region Методы

    private void BLOG(string[] arg)
    {
        Data.ErrorText.text = "Неверный логин или пароль";
        Data.interactive.Clearing_Fields(new GameObject[] { Data.M_Login });
    }
    private void BREG(string[] arg)
    {
        Data.ErrorText.text = "Данный email уже занят";
        Data.interactive.Clearing_Fields(new GameObject[] { Data.M_Registration });
    }


    public void AAA_in(string ss) //Пробный метод на вход
    {
        message = ss;
        OnIncomingData();
    }
    public void AAA_out(string st) //Пробный метод на выход
    {
        Send(st);
    }

    #endregion
}