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
using TMPro.EditorUtilities;

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
        string host = "77.108.206.67";
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

        Debug.Log(arg[3]);

        if (arg[3] != null)
        {
            Debug.Log("7");
            M_Program.transform.GetChild(0).gameObject.SetActive(true);
            M_Program.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("8");
            M_Program.transform.GetChild(0).gameObject.SetActive(false);
            M_Program.transform.GetChild(1).gameObject.SetActive(true);
            DeleteAllBox();
            Send("%SCORE");
        }
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
        if ((nick != "") && (email != "") && (pass != ""))
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
        M_Registr.transform.GetChild(0).GetComponent<InputField>().text = "";
        M_Registr.transform.GetChild(1).GetComponent<InputField>().text = "";
        M_Registr.transform.GetChild(2).GetComponent<InputField>().text = "";

        M_Login.SetActive(true);
    }
    private void BREG()
    {
        ErrorText.text = "Данный email уже занят";
        M_Registr.transform.GetChild(1).GetComponent<InputField>().text = "";
        M_Registr.transform.GetChild(2).GetComponent<InputField>().text = "";
    }

    #endregion

    #region занесение объектов в таблицу

    [Space]
    public Image Conteiner;
    public GameObject[] box;
    private void SpavnBoxPrefabs(string type, GameObject box, object[] arg)
    {
        GameObject so = Instantiate(box, Conteiner.transform);
        if (type == "score")
        {
            so.transform.GetChild(0).GetComponent<Text>().text = (string)arg[0];
            so.transform.GetChild(1).GetComponent<Text>().text = $"{(float)arg[1] * 100}%";
            so.transform.GetChild(2).GetComponent<Scrollbar>().size = (float)arg[2];
        }
        else
        {
            foreach (object argument in arg) Debug.Log((string)argument);
            for (int k = 0; k < arg.Length; k++)
            {
                so.transform.GetChild(k).GetComponent<Text>().text = Convert.ToString(arg[k]);
            }
        }

        Conteiner.GetComponent<RectTransform>().sizeDelta = new Vector2(Conteiner.rectTransform.sizeDelta.x, Conteiner.rectTransform.sizeDelta.y + 165);
    }
    private void DeleteAllBox()
    {
        Conteiner.GetComponent<RectTransform>().sizeDelta = new Vector2(Conteiner.rectTransform.sizeDelta.x, 0);
        GameObject[] OldObj = GameObject.FindGameObjectsWithTag("conteiner");
        foreach (GameObject obj in OldObj) { Destroy(obj); }
    }

    #endregion

    private bool TakingMessage = true;
    private void MES(string[] arg)
    {
        if(TakingMessage)
        {
            SpavnBoxPrefabs(null, box[0], arg);
        }     
    }
    private void SCORE(string[] arg)
    {
        string name = arg[0];
        double point = Convert.ToDouble(arg[1]);
        float p = Convert.ToSingle(Math.Round(((point / 11) * 100), 1));

        if (p == 100) { p -= 0.1f; }
        p /= 100;

        object[] argument = new object[] { name, p, p /= 100 };

        SpavnBoxPrefabs("score", box[1], argument);
    }

    public Text MesText;
    public void SendMessage()
    {
        Send($"%MSG:{ClientInfo[2]}:{MesText.text}");
    }

    public void GoMessage()
    {
        DeleteAllBox();
        TakingMessage = true;
    }
    public void GoScore()
    {
        DeleteAllBox();
        TakingMessage = false;
        Send("%SCORE");
    }
    public void Exit()
    {
        DeleteAllBox();
        TakingMessage = false;
        M_Program.SetActive(false);
        M_Program.transform.GetChild(1).gameObject.SetActive(false);
        M_Login.SetActive(true);
    }

    public void But_Proba(string ss)
    {
        message = ss;
        OnIncomingData();
    }
     
    #endregion
}