using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Script_Test0 : MonoBehaviour
{

    public GameObject[] but;
    private Vector3[] vector_but = new Vector3[6];

    private int[] marks = new int[6];

    private int active_obj = 6;
    private int question = 1;

    private readonly int[][] profile = new int[5][];
    private readonly string[][] quest = new string[5][];

    private void Start()
    {
        for (int i = 0; i < but.Length; i++)
        {
            vector_but[i] = new Vector3(but[i].transform.position.x, but[i].transform.position.y, but[i].transform.position.z);
        }

        profile[0] = new int[6] { 1, 2, 3, 4, 5, 6 };
        profile[1] = new int[6] { 2, 3, 4, 5, 6, 1 };
        profile[2] = new int[6] { 3, 4, 5, 6, 1, 2 };
        profile[3] = new int[6] { 4, 5, 6, 1, 2, 3 };
        profile[4] = new int[6] { 5, 6, 1, 2, 3, 4 };

        quest[0] = new string[6] { "Проектировщик нанотехнологичных материалов", "Тканевый инженер", "Игромастер", "Проектировщик промышленной робототехники", "Аддитивный инженер", "Дизайнер носимых энергоустройств" };
        quest[1] = new string[6] { "Эксперт персонифицированной медицины", "Архитектор виртуальности", "Проектировщик домашних роботов", "Архитектор медоборудования", "Дизайнер дополненной реальности территорий", "Системный инженер композитных материалов" };
        quest[2] = new string[6] { "Дизайнер интерфейсов", "Оператор многофункциональных робототехнических комплексов", "Архитектор цифровой модели города", "Дизайнер-эргономист носимых устройств для безопасности", "Системный инженер композитных материалов", "Сити-фермер" };
        quest[3] = new string[6] { "Проектировщик коботов", "Программист электронных рецептов одежды", "Проектировщик доступной среды", "Рециклинг-технолог", "Молекулярный диетолог", "Консультант по безопасности личного профиля" };
        quest[4] = new string[6] { "BIM-менеджер-проектировщик", "Проектировщик инфраструктуры «Умного дома»", "Проектировщик умных материалов", "Проектант жизни медицинских учреждений", "ИТ-аудитор", "Разработчик экзоскелетов" };

        for (int j = 0; j < but.Length; j++)
        {
            but[j].transform.GetChild(0).GetComponent<Text>().text = quest[question-1][j];
        }
    }

    public void evaluate(string str)
    {
        int obj = Convert.ToInt32(str.Substring(0, 1));
        int mark = Convert.ToInt32(str.Substring(1));
        //Debug.Log(obj + "" + mark);

        marks[profile[question - 1][obj - 1] - 1] += mark;
        ClearFied(mark, false);

        //Debug.Log(profile[question - 1][obj - 1] - 1);
        but[obj - 1].GetComponent<Rigidbody2D>().simulated = true;
        but[obj - 1].transform.GetChild(7).gameObject.SetActive(true);

        //Debug.Log($"Результат: {marks[0]} {marks[1]} {marks[2]} {marks[3]} {marks[4]} {marks[5]} {marks[5]}");
        active_obj--;
        if (active_obj == 0 && question <= 5)
        {
            question++;
            if(question <=5)
            { 
                for (int j = 0; j < but.Length; j++)
                {
                    ClearFied(0, true);
                    but[j].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    but[j].GetComponent<Rigidbody2D>().simulated = false;
                    but[j].transform.GetChild(0).GetComponent<Text>().text = quest[question - 1][j];
                    but[j].transform.position = vector_but[j];
                    but[j].transform.rotation = new Quaternion(0, 0, 0, 0);
                    but[j].transform.GetChild(7).gameObject.SetActive(false);
                }
                active_obj = 6;
            }           
        }
        if (question == 6)
        {
            end();
        }      
    }
    private void ClearFied(int var, bool kolab)
    {
        if (!kolab)
        {
            for (int p = 0; p < but.Length; p++)
            {
                but[p].transform.GetChild(var).gameObject.SetActive(kolab);
            }
        }
        else
        {
            for (int p = 0; p < but.Length; p++)
            {
                for(int j = 0; j < 7; j++)
                {
                    but[p].transform.GetChild(j).gameObject.SetActive(kolab);
                }              
            }
        }
    }
    private void end()
    {
        for(int l = 0; l <= 5; l++)
        {
            Data.COEFICENT[l] = (marks[l]*100/30);
        }
        //отправка UCOEF id коэффиценты
        Data.client.Send($"%UCOEF:{Data.ID}:{Data.COEFICENT[0]}:{Data.COEFICENT[1]}:{Data.COEFICENT[2]}:{Data.COEFICENT[3]}:{Data.COEFICENT[4]}:{Data.COEFICENT[5]}");
        Data.Test_0.SetActive(false);
        Data.interactive.GameMenu();
    }
}