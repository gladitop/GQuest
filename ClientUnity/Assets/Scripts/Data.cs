using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    #region ClientInfo

    public static string ID { get; set; }
    public static string EMAIL { get; set; }
    public static string NICK { get; set; }
    public static int LEVEL { get; set; }
    public static int[] COEFICENT = new int[6]; // 0-Nano | 1-Bio | 2-IT | 3-Robo | 4-HiTech | 5-PromDiz

    #endregion

    #region Interactive

    //Главные объекты
    public static GameObject Main_Canvas { get { return GameObject.Find("Main_Canvas"); } }
    public static GameObject Client_Main { get { return GameObject.Find("Client_Main"); } }

    //Скрипты
    public static Client client { get { return Client_Main.GetComponent<Client>(); } }
    public static Interactive interactive { get { return Client_Main.GetComponent<Interactive>(); } }


    public static GameObject Registr_Login { get { return Main_Canvas.transform.Find("Registr_Login").gameObject; } }
    public static GameObject M_Programm { get { return Main_Canvas.transform.Find("M_Programm").gameObject; } }

    public static GameObject M_Login { get { return Registr_Login.transform.Find("M_Login").gameObject; } }
    public static GameObject M_Registration { get { return Registr_Login.transform.Find("M_Registr").gameObject; } }
    public static Text ErrorText { get { return Registr_Login.transform.Find("ErrorText").gameObject.GetComponent<Text>(); } }

    public static GameObject Test_0 { get { return M_Programm.transform.Find("TEST_0").gameObject; } }
    
    public static GameObject GameMenu { get { return M_Programm.transform.Find("GameMenu").gameObject; } }
    public static GameObject TestMenu { get { return M_Programm.transform.Find("Test").gameObject; } }

    public static GameObject PreTestMenu { get { return TestMenu.transform.Find("PreTest").gameObject; } }
    public static GameObject Questions_M { get { return TestMenu.transform.Find("Questions").gameObject; } }

    #endregion

    #region LoginFields_RegistrationFields

    public static string LF_Email
    {     
        get { return M_Login.transform.GetChild(0).GetComponent<InputField>().text; }
        set { M_Login.transform.GetChild(0).GetComponent<InputField>().text = value; }
    }
    public static string LF_Password
    {
        get { return M_Login.transform.GetChild(1).GetComponent<InputField>().text; }
    }

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
}