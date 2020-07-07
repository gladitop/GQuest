using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    #region Соединение Клиент-сервер

    private bool unityReady = true;
    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private List<string> comands = new List<string>();
    private bool bmassmsg = false;
    private string massmsg = "";
    private GameObject Indicator;

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
                    //Debug.Log("Не удалось подключиться к серверу");
                    goto end;
                }
            }
            else
            {
                try
                {
                    socket.Client.Send(new byte[1]);
                    //Debug.Log("пинг");
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
            string host = "37.29.78.130";
            //string host = "127.0.0.1";
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
            //Debug.Log("Ну удалось подключиться к серверу");
        }
    }

    private void Update()
    {
    end:
        if (socketReady)
        {
            GameObject.Find("Indicator").GetComponent<Image>().color = Color.green;

            if (stream.DataAvailable) //тут считываються сообщения и добавляються в commands list
            {
                byte[] buffer = new byte[1024];
                int i = socket.Client.Receive(buffer);
                if (i <= 1) goto end;

                string message = Encoding.UTF8.GetString(buffer, 0, i);

                if(message.Contains("%CMSG"))
                {
                    bmassmsg = true;
                }
                else if (message.Contains("%EMSG"))
                {
                    bmassmsg = false;

                    string[] cmds = massmsg.Split(new[] { '☼' });
                    foreach (string cmd in cmds)
                    {
                        if (cmd.Length > 1)
                        {
                            comands.Add(cmd);
                        }
                    }
                }
                else if (bmassmsg)
                {
                    massmsg += message;
                }
                else
                {
                    string[] cmds = message.Split(new[] { '☼' });

                    foreach (string cmd in cmds)
                    {
                        if (cmd.Length > 1)
                        {
                            comands.Add(cmd);
                        }
                    }
                }
            }
        }
        else { GameObject.Find("Indicator").GetComponent<Image>().color = Color.red; }

        OnIncomingData();
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
        if (comands.Count != 0)
        {
            string message = comands[0];
            comands.Remove(message);

            Debug.Log("Входящая команда: " + message); //Входящее сообщение

            var ch = ':'; //Разделяющий символ
            try
            {
                var command = message.Substring(1, message.IndexOf(ch) - 1);//Команда 
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
        Func.Clearing_Fields(new GameObject[] { Data.M_Login });

        Data.ID = arg[0];
        Data.EMAIL = arg[1];
        Data.NICK = arg[2];
        Data.COEFICENT = Func.ConvertMassTo_int(arg[3].Split(new[] { '|' }));
        Data.LEVEL = int.Parse(arg[4]);     

        if (Data.COEFICENT.Length < 6)
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
        Func.Clearing_Fields(new GameObject[] { Data.M_Login });
    }
    private void BREG(string[] arg)
    {
        Data.ErrorText.text = "Данный email уже занят";
        Func.Clearing_Fields(new GameObject[] { Data.M_Registration });
    }

    private void TEST(string[] arg)
    {
        try
        {
            DataBase.ExecuteQueryAnswer($"INSERT INTO Tests (profile, test, name, text, questions) VALUES({arg[0]}, {arg[1]}, '{arg[2]}', '{arg[3]}', '{arg[4]}');");
            Debug.Log($"Тест добавлен: {arg[0]} | {arg[1]}");
        }
        catch (Exception ex)
        {
            Debug.Log($"Ошибка при добавлении теста: {ex}");
        }
    }

    private void QUEST(string[] arg)
    {
        try
        {
            DataBase.ExecuteQueryAnswer($"INSERT INTO Questions (profile, test, question, text, v1, v2, v3, v4, v5, v6, marks) VALUES({arg[0]}, {arg[1]}, {arg[2]}, '{arg[3]}', '{arg[4]}', '{arg[5]}', '{arg[6]}', '{arg[7]}', '{arg[8]}', '{arg[9]}', '{arg[10]}');");
            Debug.Log($"вопрос добавлен: {arg[0]} | {arg[1]}| {arg[2]}");
        }
        catch (Exception ex)
        {
            Debug.Log($"Ошибка при добавлении вопроса: {ex}");
        }
    }

    #endregion
}