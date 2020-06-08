using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactive : MonoBehaviour
{

    public void Login()
    {
        Data.client.Send($"%LOG:{Data.LF_Email}:{Data.LF_Password}");
    }

    public void Registration()
    {
        Data.client.Send($"%REG:{Data.RF_Email}:{Data.RF_Password}:{Data.RF_Nick}");
    }


    public void GoLogin()
    {
        Data.M_Login.SetActive(true);
        Data.M_Registration.SetActive(false);
        Clearing_Fields(new GameObject[] { Data.M_Registration });
    }

    public void GoRegistration()
    {
        Data.M_Login.SetActive(false);
        Data.M_Registration.SetActive(true);
        Clearing_Fields(new GameObject[] { Data.M_Login });
    }

    public void Clearing_Fields(GameObject[] obj)
    {
        for (int i = 0; i <= obj.Length; i++)
        {
            try
            {
                for (int j = 0; j <= 2; j++) //очистит только 3 элемента
                {
                    try
                    {
                        obj[i].transform.GetChild(j).GetComponent<InputField>().text = "";
                    }
                    catch { }
                }
            }
            catch { }
        }
    } //очистка полей
}