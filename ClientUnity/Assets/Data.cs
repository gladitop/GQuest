using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    #region ClientInfo

    public static string ID { get; set; }
    public static string EMAIL { get; set; }
    public static string NICK { get; set; }
    public static string COEF { get; set; }

    #endregion

    #region Interactive

    public static GameObject Main_Canvas;
    public static Client client;
    public static Interactive interactive;
    public static GameObject M_Login;
    public static GameObject M_Registration;
    public static Text ErrorText;

    private void Start()
    {
        Main_Canvas = GameObject.Find("Main_Canvas");
        client = GameObject.Find("Client_Main").GetComponent<Client>();//Скрипт Client
        interactive = GameObject.Find("Client_Main").GetComponent<Interactive>();//Скрипт Interactive
        M_Login = Main_Canvas.transform.GetChild(0).GetChild(0).gameObject;
        M_Registration = Main_Canvas.transform.GetChild(0).GetChild(1).gameObject;
        ErrorText = GameObject.Find("ErrorText").GetComponent<Text>();
    } //Определение переменных

    #region LoginFields
    public static string LF_Email
    {     
        get { return M_Login.transform.GetChild(0).GetComponent<InputField>().text; }
        set { M_Login.transform.GetChild(0).GetComponent<InputField>().text = value; }
    }
    public static string LF_Password
    {
        get { return M_Login.transform.GetChild(1).GetComponent<InputField>().text; }
    }
    #endregion

    #region RegistrationFields
    public static string RF_Email
    {
        get { return M_Registration.transform.GetChild(1).GetComponent<InputField>().text; }
    }
    public static string RF_Password
    {
        get { return M_Registration.transform.GetChild(2).GetComponent<InputField>().text; }
    }
    public static string RF_Nick
    {
        get { return M_Registration.transform.GetChild(0).GetComponent<InputField>().text; }
    }
    #endregion

    #endregion
}

