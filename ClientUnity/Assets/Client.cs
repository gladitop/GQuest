using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class Client : MonoBehaviour
{
    [Header("Окна: log|reg|bst|test")]
    public GameObject[] menu;
    [Header("Вывод сообщений")]
    public Text ErrorMessage;
    [Header("нб")]
    public GameObject LEmail;
    [Header("Score")]
    public GameObject BoxScore;
    public GameObject Counteiner;
    public Image img;

    private int id;
    private int PCount;

    //List<string> nm = new List<string>();
    //List<int> pm = new List<int>();

    private void Sortirovka(List<string> nm, List<int> pm)
    {
        //for(int i=0; i<=pm.Count; i++)
        //{
        //    int k = 0;
        //    int f = pm[i];

        //    for (int h = 0; h <= pm.Count; h++)
        //    {               
        //        if (f > pm[h])
        //        { k = h;
        //            f = pm[h]; 
        //        }   
        //    }

        //    if (pm[i] != f)
        //    {
        //        pm[k] = pm[i];
        //        pm[i] = f;
        //    }
        //}
    }

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
                Debug.Log(message);

            }
        }
    }
    public void Send(string data)
    {
        if (!socketReady)
            return;

        socket.Client.Send(Encoding.UTF8.GetBytes(data));
        writer.Flush();
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
    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
    #endregion

    #region Команды на вход и выход
    private void OnIncomingData()
    {
        if (message.Contains("%REGOOD"))
        {
            LEmail.GetComponent<InputField>().text = GameObject.Find("REmailInput").GetComponent<InputField>().text;
            GameObject.Find("REmailInput").GetComponent<InputField>().text = "";
            GameObject.Find("RPassInput").GetComponent<InputField>().text = "";
            GameObject.Find("RNickInput").GetComponent<InputField>().text = "";
            ErrorMessage.text = "";
            GoLogin();
        }
        if (message.Contains("%LOGOD"))
        {
            Match regex = Regex.Match(message, "%LOGOD:(.*):(.*):(.*)");
            Debug.Log(message);
            id = int.Parse(regex.Groups[1].Value);
            string name = regex.Groups[2].Value;
            PCount = int.Parse(regex.Groups[3].Value);

            if (PCount != 0)
            {
                menu[0].SetActive(false);
                menu[1].SetActive(false);
                menu[2].SetActive(false);
                menu[3].SetActive(false);
                ErrorMessage.text = "";
                Send($"%PCOUNT:{id.ToString()}:{PCount}");
                Debug.Log($"Данные отправленны: %PCOUNT:{id}:{PCount}");
            }
            else
            {
                menu[0].SetActive(false);
                menu[1].SetActive(false);
                menu[2].SetActive(true);
            }
            ErrorMessage.text = "";
        }  //%LOGOD:id:name:PCount
        if (message.Contains("%SCORE"))
        {
            Match regex = Regex.Match(message, "%SCORE:(.*):(.*)");
            string names = regex.Groups[1].Value;
            double points = Convert.ToDouble(regex.Groups[2].Value);
            float p = Convert.ToSingle(Math.Round(((points / 11) * 100), 1));
            //
            //nm.Add(regex.Groups[1].Value);
            //pm.Add(int.Parse(regex.Groups[2].Value));
            //
            GameObject go = Instantiate(BoxScore, Counteiner.transform);
            go.GetComponentInChildren<Text>().text = message;
            go.transform.GetChild(1).GetComponent<Text>().text = names;
            if (p == 100) { p -= 0.1f; }
            go.transform.GetChild(2).GetComponent<Text>().text = $"{p}%";
            go.transform.GetChild(3).GetComponent<Scrollbar>().size = p / 100;
            img.GetComponent<RectTransform>().sizeDelta = new Vector2(img.rectTransform.sizeDelta.x, img.rectTransform.sizeDelta.y + 50);
            Debug.Log(img.rectTransform.sizeDelta.y);
            Debug.Log($"%SCORE:{names}:{p}");
        } //%SCORE:name:points
        if (message.Contains("%BLOG"))
        {
            ErrorMessage.text = "Введены неправильные данные";
            GameObject.Find("LPassInput").GetComponent<InputField>().text = "";
        }
        if (message.Contains("%BREG"))
        {
            ErrorMessage.text = "Почта уже используется";
            GameObject.Find("REmailInput").GetComponent<InputField>().text = "";
            GameObject.Find("RPassInput").GetComponent<InputField>().text = "";
        }
    }
    public void Registration()
    {
        string email = GameObject.Find("REmailInput").GetComponent<InputField>().text;
        string pass = GameObject.Find("RPassInput").GetComponent<InputField>().text;
        string clientName = GameObject.Find("RNickInput").GetComponent<InputField>().text;
        GameObject.Find("RPassInput").GetComponent<InputField>().text = "";

        Debug.Log($"Данные отправленны: %REG:{email}:{pass}:{clientName}");
        Send($"%REG:{email}:{pass}:{clientName}");
        Task.Delay(10).Wait();
        return;
    }         // %REG:email:pass:clientName
    public void Login()
    {
        string email = GameObject.Find("LEmailInput").GetComponent<InputField>().text;
        string pass = GameObject.Find("LPassInput").GetComponent<InputField>().text;
        GameObject.Find("LPassInput").GetComponent<InputField>().text = "";

        Debug.Log($"Данные отправленны: %LOG:{email}:{pass}");
        Send($"%LOG:{email}:{pass}");
        Task.Delay(10).Wait();
        return;
    }               // %LOG:email:pass
    public void SendScore(int point)
    {
        Send($"%PCOUNT:{id}:{point}");
        Debug.Log($"Данные отправленны: %PCOUNT:{id}:{point}");
    } //%PCOUNT:id:point
    public Text ttext;
    public void SendMessage()
    {
        string mes = ttext.text;
        Debug.Log(mes);
        Send(mes);
    }
    #endregion

    public void GoLogin()
    {
        GameObject.Find("REmailInput").GetComponent<InputField>().text = "";
        GameObject.Find("RPassInput").GetComponent<InputField>().text = "";
        GameObject.Find("RNickInput").GetComponent<InputField>().text = "";
        ErrorMessage.text = "";

        menu[1].SetActive(true);
        menu[0].SetActive(false);
        menu[2].SetActive(false);
    }
    public void GoRegistrarion()
    {
        GameObject.Find("LEmailInput").GetComponent<InputField>().text = "";
        GameObject.Find("LPassInput").GetComponent<InputField>().text = "";
        ErrorMessage.text = "";
        menu[0].SetActive(true);
        menu[1].SetActive(false);
        menu[2].SetActive(false);
    }

    #region Пробный метод
    [Header("пробный текст")]
    public Text tt;
    public void proba()
    {
        message = tt.text;
        OnIncomingData();
    }
    #endregion
}