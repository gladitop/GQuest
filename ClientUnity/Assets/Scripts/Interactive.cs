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
        Func.Clearing_Fields(new GameObject[] { Data.M_Registration });
    }
    public void GoRegistration()
    {
        Data.M_Login.SetActive(false);
        Data.M_Registration.SetActive(true);
        Func.Clearing_Fields(new GameObject[] { Data.M_Login });
    }
   

    public void TEST0()
    {
        Debug.Log("Hello, Test 0 начался!");
        Data.Test_0.GetComponent<Script_Test0>().StartVideo();
        Data.Registr_Login.gameObject.SetActive(false);
        Data.M_Programm.gameObject.SetActive(true);
        Data.Test_0.SetActive(true);
    } //Тест для определения коэффицентов

    public GameObject InfoText;
    public void GameMenu()
    {
        Data.Main_Canvas.transform.GetChild(0).gameObject.SetActive(false);
        Data.Main_Canvas.transform.GetChild(1).gameObject.SetActive(true);
        Data.GameMenu.SetActive(true);

        Debug.Log("Start Game_Menu");

        DataBase.ExecuteQueryAnswer("DELETE FROM Questions;");
        DataBase.ExecuteQueryAnswer("DELETE FROM Tests;");
        Data.client.Send($"%ULVL:{Data.ID}");
    }
    public void ChecInfo()
    {
        InfoText.GetComponent<Text>().text = $"Данные:\n" +
        $"email: {Data.EMAIL}\n" +
        $"nick: {Data.NICK}\n" +
        $"ID: {Data.ID}\n" +
        $"Level: {Data.LEVEL}\n" +
        $"\n" +
        $"Коэфиценты:\n" +
        $"Nano: {Data.COEFICENT[0]}\n" +
        $"Bio: {Data.COEFICENT[1]}\n" +
        $"IT: {Data.COEFICENT[2]}\n" +
        $"Robo: {Data.COEFICENT[3]}\n" +
        $"HiTech: {Data.COEFICENT[4]}\n" +
        $"Promdiz: {Data.COEFICENT[5]}";
    }

    int[] quest_id;
    int check_quest = 0;
    List<int> answers = new List<int>();

    public void PreLunchTest(int profile)
    {
        int countTest = int.Parse(DataBase.ExecuteScalarAnswer($"SELECT COUNT(*) FROM Tests WHERE profile == {profile};"));
        Debug.LogError("Кол-во тестов: " + countTest);

        if (countTest == 0)
        {
            Debug.LogWarning("пусто");
        }
        else if (countTest == 1)
        {
            Data.GameMenu.SetActive(false);
            Data.TestMenu.SetActive(true);
            Data.PreTestMenu.SetActive(true);

            DataTable dt = DataBase.GetTable($"SELECT * FROM Tests WHERE profile == {profile};");

            string name = Convert.ToString(dt.Rows[0][3]);
            string text = Convert.ToString(dt.Rows[0][4]);
            quest_id = Func.ConvertMassTo_int(Convert.ToString(dt.Rows[0][5]).Split(new[] { '|' }));

            Data.PreTestMenu.transform.GetChild(0).GetComponent<Text>().text = name;
            Data.PreTestMenu.transform.GetChild(1).GetComponent<Text>().text = text;
            LunchTest();
        }
        else
        {
            Debug.LogWarning("Тестов больше чем 1, эта часть ещё не доделанна!");
        }
    }

    public void LunchTest()
    {
        Data.PreTestMenu.SetActive(false);
        Data.Questions_M.SetActive(true);

        DataTable dt = DataBase.GetTable($"SELECT * FROM Questions WHERE question == {quest_id[check_quest]};");

        Data.Questions_M.transform.GetChild(0).GetComponent<Text>().text = (string)dt.Rows[0][4];
        for (int i = 0; i < 6; i++)
        {
            Data.Questions_M.transform.GetChild(i + 1).GetChild(0).GetComponent<Text>().text = (string)dt.Rows[0][i + 5];
        }

        check_quest++;
    }

    public void ChangeQuestion(int answer)
    {
        answers.Add(answer);       

        if (check_quest < quest_id.Length)
        {
            DataTable dt = DataBase.GetTable($"SELECT * FROM Questions WHERE question == {quest_id[check_quest]};");

            Data.Questions_M.transform.GetChild(0).GetComponent<Text>().text = (string)dt.Rows[0][4];
            for (int i = 0; i < 6; i++)
            {
                Data.Questions_M.transform.GetChild(i + 1).GetChild(0).GetComponent<Text>().text = (string)dt.Rows[0][i + 5];
            }
            check_quest++;
        }
        else if (check_quest == quest_id.Length) { end(); }
        
        void end()
        {
            check_quest = 0;
            int points = 0;
            Debug.Log($"{answers[0]} {answers[1]} {answers[2]} {answers[3]} {answers[4]}");

            for (int i = 0; i < quest_id.Length; i++)
            {
                DataTable dt = DataBase.GetTable($"SELECT * FROM Questions WHERE question == {quest_id[i]};");
                int[] marks = Func.ConvertMassTo_int(Convert.ToString(dt.Rows[0][11]).Split(new[] { '|' }));

                points += marks[answers[i]];
                Debug.LogWarning(marks[answers[i]]);
                
            }
            Debug.LogError(points);
            answers = new List<int>();

            Data.GameMenu.SetActive(true);
            Data.Questions_M.SetActive(false);
            Data.TestMenu.SetActive(false);
            Data.PreTestMenu.SetActive(false);
        }
    }    
}