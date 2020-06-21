using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Interactive : MonoBehaviour
{
    private void Start()
    {
        Data.Main_Canvas.transform.GetChild(0).gameObject.SetActive(true);
        Data.M_Login.SetActive(true);
    }

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

    public void TEST0()
    {
        Debug.Log("Hello, Test 0 начался!");
        Data.Main_Canvas.transform.GetChild(0).gameObject.SetActive(false);
        Data.Main_Canvas.transform.GetChild(1).gameObject.SetActive(true);
        Data.Test_0.SetActive(true);
    } //Тест для определения коэффицентов

    public void GameMenu()
    {
        Data.Main_Canvas.transform.GetChild(0).gameObject.SetActive(false);
        Data.Main_Canvas.transform.GetChild(1).gameObject.SetActive(true);
        Data.GameMenu.SetActive(true);
        Debug.Log("Start Game_Menu");
        //DataBase.ExecuteQueryAnswer("DELETE FROM Questions;");
        //DataBase.ExecuteQueryAnswer("DELETE FROM Tests;");
        //Data.client.Send($"%ULVL:{Data.ID}");
        PreLunchTest(1);
    }

    private int now_id_quest = 0;
    public void PreLunchTest(int profile)
    {
        int countTest = int.Parse(DataBase.ExecuteScalarAnswer($"SELECT COUNT(*) FROM Tests WHERE profile == {profile};"));
        Debug.LogError(countTest);

        if (countTest == 0)
        {
            Debug.LogWarning("пусто");
        }
        else if(countTest == 1)
        {
            DataTable dt = DataBase.GetTable($"SELECT * FROM Tests WHERE profile == {profile};");
            string[] quest_id_string = Convert.ToString(dt.Rows[0][5]).Split(new[] { '|' });

            int[] quest_id_int = new int[quest_id_string.Length];
            for(int i = 0; i < quest_id_string.Length; i++)
            {
                quest_id_int[i] = int.Parse(quest_id_string[i]);
            }

            //LunchTest(profile, quest_id_int);
            now_id_quest = quest_id_int[0];
            ChangeQuestion();
        }   
    }
    
    private void ChangeQuestion()
    {

    }
    public void NextQuestion(int answer)
    {


    }
}
    //for (int i = 0; i < id_questions.Length; i++)
        //{
        //    for (int p = 0; p < 6; p++)
        //    {
        //        Data.TestMenu.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
        //    }               
        //}