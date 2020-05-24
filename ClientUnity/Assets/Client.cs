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
            var ch = ':'; //Разделяющий символ
            try
            {
                var command = message.Substring(1, message.IndexOf(ch) - 1); //Команда 
                try
                {
                    var arguments = message.Substring(message.IndexOf(ch) + 1).Split(new[] { ch }); //Массив аргументов
                    this.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, new object[] { arguments });
                }
                catch (Exception ex) { Debug.Log(ex); }
            }
            catch
            {
                try
                {
                    var command = message.Substring(1);//Команда без аргументов
                    this.GetType().GetMethod(command, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, null);
                }
                catch (Exception ex) { Debug.Log(ex); }
            }

        }
    }

    #endregion

    public GameObject M_Login;
    public GameObject M_Registr;
    public GameObject M_Program;
    [Space]
    public Text ErrorText;
    [Space]
    public Image Conteiner;
    public GameObject Box_Score;

    public string[] ClientInfo; //email id nick point

    #region Команды

        #region Login/Registr

    public void SendLog()
    {
        ErrorText.text = "";
        string email = M_Login.transform.GetChild(0).GetComponent<InputField>().text;
        string pass = M_Login.transform.GetChild(1).GetComponent<InputField>().text;
        Send($"%LOG:{email}:{pass}");
    }
    public void GoToRegistratioon()
    {
        ErrorText.text = "";
        M_Login.SetActive(false);
        M_Login.transform.GetChild(0).GetComponent<InputField>().text = "";
        M_Login.transform.GetChild(1).GetComponent<InputField>().text = "";
        M_Registr.SetActive(true);
    }
    private void LOGOOD(string[] arg)
    {
        ClientInfo = arg;
        Debug.Log(arg[0] + " " + arg[1] + " " + arg[2] + " " + arg[3]);
        ErrorText.text = "";
        M_Login.SetActive(false);
        M_Login.transform.GetChild(0).GetComponent<InputField>().text = "";
        M_Login.transform.GetChild(1).GetComponent<InputField>().text = "";
        M_Program.SetActive(true);
    }
    private void BLOG()
    {
        ErrorText.text = "Неверный логин или пароль";
        M_Login.transform.GetChild(1).GetComponent<InputField>().text = "";
    }

    public void SendReg()
    {
        ErrorText.text = "";
        string nick = M_Registr.transform.GetChild(0).GetComponent<InputField>().text;
        string email = M_Registr.transform.GetChild(1).GetComponent<InputField>().text;
        string pass = M_Registr.transform.GetChild(2).GetComponent<InputField>().text;
        if ((nick != "")&&(email != "")&&(pass!= ""))
        {
            Send($"%REG:{email}:{pass}:{nick}");
        }
        else
        {
            ErrorText.text = "Вы ввели не все данные";
        }
    }
    public void GoToLogining()
    {
        ErrorText.text = "";
        M_Registr.SetActive(false);
        M_Registr.transform.GetChild(0).GetComponent<InputField>().text = "";
        M_Registr.transform.GetChild(1).GetComponent<InputField>().text = "";
        M_Registr.transform.GetChild(2).GetComponent<InputField>().text = "";
        M_Login.SetActive(true);
    }
    private void REGOOD()
    {
        ErrorText.text = "";
        M_Registr.SetActive(false);
        M_Login.transform.GetChild(0).GetComponent<InputField>().text = M_Registr.transform.GetChild(1).GetComponent<InputField>().text;
        M_Login.SetActive(true);
    }
    private void BREG()
    {
        ErrorText.text = "Данный email уже занят";
        M_Registr.transform.GetChild(1).GetComponent<InputField>().text = "";
        M_Registr.transform.GetChild(2).GetComponent<InputField>().text = "";
    }

        #endregion

    private void SCORE(string[] arg)
    {
        string name = arg[0];
        double point = Convert.ToDouble(arg[1]);
        float p = Convert.ToSingle(Math.Round(((point / 11) * 100), 1));

        GameObject so = Instantiate(Box_Score, Conteiner.transform);
        Conteiner.GetComponent<RectTransform>().sizeDelta = new Vector2(Conteiner.rectTransform.sizeDelta.x, Conteiner.rectTransform.sizeDelta.y + 65);

        so.transform.GetChild(0).GetComponent<Text>().text = name;
        if (p == 100) { p -= 0.1f; }
        so.transform.GetChild(1).GetComponent<Text>().text = $"{p}%";
        so.transform.GetChild(2).GetComponent<Scrollbar>().size = p / 100;
    }
     
    #endregion
}