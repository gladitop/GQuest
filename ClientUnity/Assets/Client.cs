using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq;

public class Client : MonoBehaviour
{
    #region Соединение Клиент-сервер
    private bool socketReady;
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
        //if already connection, igmore this function
        if (socketReady)
            return;

        //Defalt host / post values
        string host = "127.0.0.1";
        int port = 908;


        //Create the socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);//Это только для общего чата получать
            reader = new StreamReader(stream);//Или для беседы
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
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
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
    #endregion

    #region Получение / Отправка

    public void Send(string data)
    {
        if (!socketReady)
            return;

        socket.Client.Send(Encoding.UTF8.GetBytes(data));
        Debug.Log(data);
        writer.Flush();
    }
    private void OnIncomingData()
    {
        Debug.Log(message);
        if (message != "")
        {
            try
            {
                var ch = ':'; //Разделяющий символ
                try
                {
                    var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                    Debug.Log("Команда: " + command);
                    try
                    {
                        var arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] { ch }); //Массив аргументов
                        Debug.Log("Аргументы:");
                        foreach (var s in arguments) Debug.Log(s);

                        this.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, new object[] { arguments });
                    }
                    catch (Exception ex) { Debug.Log(ex); }
                }
                catch
                {
                    try 
                    { 
                        var command = message.Substring(1);
                        Debug.Log(command);
                        this.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, null);
                    }
                    catch (Exception ex) { Debug.Log(ex); }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nОшибка при поиске команды:\n----------\n" + ex + "\n----------");
            }
        }
    }

    #endregion

    public GameObject M_Login;
    public GameObject M_Registr;
    public Text ErrorText;


    #region Команды

    #region Login/Registr

        public void SendLog()
        {
            string email = M_Login.transform.GetChild(0).GetComponent<InputField>().text;
            string pass = M_Login.transform.GetChild(1).GetComponent<InputField>().text;
            Send($"%LOG:{email}:{pass}");
        }
        private void LOGOOD(string[] arg)
        {
            Debug.Log("Удачный login");
        }
        private void BLOG()
        {
            Debug.Log("Неудачный login");
            ErrorText.text = "Неверный логин или пароль";       
        }

        private void REGOOD()
        {
            Debug.Log("Good!");
        }
        private void BREG()
        {
            Debug.Log("Good!");
        }

        #endregion
     
    #endregion

    public void TestMessageInput()
    {
        
    }
}
//string email = GameObject.Find("REmailInput").GetComponent<InputField>().text;