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

    public Thread threadLOG;
    private void Start()
    {
        threadLOG = new Thread(ServerLog);
        threadLOG.IsBackground = true;
        threadLOG.Start();
    }

    private void ServerLog() // проверка на подключение к серверу
    {
        while (true)
        {
        end:
            Task.Delay(1200).Wait(); //Задержа до повторного подключения           
            if (!socketReady)
            {
                try
                {
                    ConnectedToServer();
                }
                catch
                {
                    Debug.Log("Не удалось подключиться к серверу");
                    goto end;
                }
            }
            else
            {
                try
                {
                    socket.Client.Send(new byte[1]);
                    Debug.Log("пинг");
                }
                catch
                {
                    Debug.Log("Разрыв с сервером");
                    CloseSocket();
                    goto end;
                }
            }
        }
    }

    public void ConnectedToServer()
    {
        try
        {
            //Defalt host / post values
            //string host = "37.29.78.130";
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
        end:
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                int i = socket.Client.Receive(buffer);
                if (i == 1) goto end;
                message = Encoding.UTF8.GetString(buffer, 0, i);
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
        threadLOG.Suspend(); //Для пинговки сервера
        Task.Delay(100).Wait();

        socket.Client.Send(Encoding.UTF8.GetBytes(data));
        Debug.Log(data);
        writer.Flush();

        Task.Delay(100).Wait();
        threadLOG.Resume(); //Для пинговки сервера
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

    private void LOGOOD(string[] arg)
    {
        Data.interactive.Clearing_Fields(new GameObject[] { Data.M_Login });

        Data.ID = arg[0];
        Data.EMAIL = arg[1];
        Data.NICK = arg[2];
        Data.COEF = arg[3];
        Data.LEVEL = int.Parse(arg[4]);
  
        if (Data.COEF == "0")
        {
            Data.interactive.TEST0();
        }
        else
        {
            Data.interactive.GameMenu();
        }

    }
    private void REGOOD(string[] arg)
    {
        Data.LF_Email = Data.RF_Email;
        Data.interactive.GoLogin();
    }

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