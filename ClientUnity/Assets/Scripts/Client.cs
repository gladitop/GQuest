using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Client : MonoBehaviour
{
    #region Соединение Клиент-сервер

    private bool unityReady = true;
    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private string message;

    public Thread threadLOG;
    private void Start()
    {
        unityReady = true;
        threadLOG = new Thread(ServerLog);
        threadLOG.IsBackground = true;
        threadLOG.Start();
    }

    private void ServerLog() // проверка на подключение к серверу
    {
        if (!unityReady) return;
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
        unityReady = false;
        CloseSocket();
    }
    private void OnDisable()
    {
        unityReady = false;
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

        string[] coef = arg[3].Split(new[] { '|' });
        for(int i = 0; i < coef.Length; i++)
        {
            Data.COEFICENT[i] = int.Parse(coef[i]);
            Debug.Log(coef[i]);
        }

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

    /*private void LVL(string[] arg)
    {
        for(int i = 0; i < arg.Length; i++)
        {
            string[] str_arg = arg[i].Split(new[] { '|' });
            int[] int_arg = new int[str_arg.Length];

            for (int p = 0; p < str_arg.Length; p++)
            {
                int_arg[p] = int.Parse(str_arg[p]);
            }

            pf_lvl_id.Add(int_arg);
        }
    }*/

    List<string[]> test = new List<string[]>();

    private int it = 0;
    private void TEST(string[] arg)
    {
        if(it != int.Parse(arg[0]))
        {
            for(int i = 0; i < int.Parse(arg[0]); i++)
            {
                test.Add(new string[] { });
                it++;
            }
        }
        else
        {
            test.Add(new string[3] { arg[0], arg[1], arg[3] });
        }   
    }

    List<string[][][]> question = new List<string[][][]>();
    List<List<List<string>>> q3 = new List<List<List<string>>>();
    List<string> q1 = new List<string>();
    List<List<string[]>> q2 = new List<List<string[]>>();

    private void QUEST(string[] arg)
    {
        q1.Add("");
        string[] xfirf = { "" };

       
    }

    #endregion
}
//Debug.LogWarning(ag);