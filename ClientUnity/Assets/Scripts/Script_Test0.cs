using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

public class Script_Test0 : MonoBehaviour
{
    public GameObject VideoCanvas;

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
        quest[3] = new string[6] { "Проектировщик роботов", "Программист электронных рецептов одежды", "Проектировщик доступной среды", "Рециклинг-технолог", "Молекулярный диетолог", "Консультант по безопасности личного профиля" };
        quest[4] = new string[6] { "BIM-менеджер-проектировщик", "Проектировщик инфраструктуры «Умного дома»", "Проектировщик умных материалов", "Проектант жизни медицинских учреждений", "ИТ-аудитор", "Разработчик экзоскелетов" };

        for (int j = 0; j < but.Length; j++)
        {
            but[j].transform.GetChild(0).GetComponent<Text>().text = quest[question-1][j];
        }
    }
    public void SSstartVideo()
    {
        VideoCanvas.SetActive(true);
        StartVideo();
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
                SSstartVideo();
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
        Data.COEFICENT = new int[6];
        for (int l = 0; l <= 5; l++)
        {         
            Data.COEFICENT[l] = (marks[l] * 100 / 30);           
        }

        Debug.Log("Сумма: " + (Data.COEFICENT[0] + Data.COEFICENT[1] + Data.COEFICENT[2] + Data.COEFICENT[3] + Data.COEFICENT[4] + Data.COEFICENT[5]));      
        Data.client.Send($"%UCOEF:{Data.ID}:{Data.COEFICENT[0]}|{Data.COEFICENT[1]}|{Data.COEFICENT[2]}|{Data.COEFICENT[3]}|{Data.COEFICENT[4]}|{Data.COEFICENT[5]}");//отправка id коэффиценты
        Data.LEVEL = 1;
        Data.Test_0.SetActive(false);
        Data.interactive.GameMenu();
    }

    VideoPlayer videoPlayer;

    void StartVideo()
    {
        GameObject camera = GameObject.Find("Camera");
        videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = true;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 1F;
        videoPlayer.url = "https://r17---sn-n8v7knez.googlevideo.com/videoplayback?expire=1592944531&ei=MhPyXsTqO-uy-gbtsJeQBA&ip=51.254.35.77&id=90aaaa543ffee8ab&itag=22&source=youtube&requiressl=yes&vprv=1&mime=video%2Fmp4&ratebypass=yes&dur=9.520&lmt=1471339053546408&fvip=3&c=WEB&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRQIhAOLaLXQaoW6cjobTeHas80QOMjVHQ-UuqM2BLlmrWGpbAiA4iLHV9PgqMXamX96sYWvm9mK2-z85BmgRvWUGw2F-HA%3D%3D&contentlength=1634661&video_id=kKqqVD_-6Ks&title=%D0%AF+-+%D0%A7%D0%B0%D0%BF%D0%B0%D0%B5%D0%B2%21+%D0%90+%D1%82%D1%8B+%D0%9A%D1%82%D0%BE+%D1%82%D1%8B+%D1%82%D0%B0%D0%BA%D0%BE%D0%B9%21%21%21&rm=sn-25gks7s&req_id=df41d598ad76a3ee&ipbypass=yes&cm2rm=sn-gvnuxaxjvh-2x1l7e,sn-gvnuxaxjvh-bvw67s&redirect_counter=3&cms_redirect=yes&mh=og&mip=95.152.54.36&mm=30&mn=sn-n8v7knez&ms=nxu&mt=1592922860&mv=m&mvi=16&pl=18&lsparams=ipbypass,mh,mip,mm,mn,ms,mv,mvi,pl&lsig=AG3C_xAwRQIhALaydxxCrL3fDlKDBPUvaK0QRwyd28qJ0wi4KaceFEjRAiAnL2ttBGGPZw7wM4IeXmo6Lx42_aPQp6ivE4vrAZeKiA%3D%3D";
        videoPlayer.frame = 0;
        videoPlayer.isLooping = true;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.errorReceived += ErrorDownload;

        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
    }
    void ErrorDownload(UnityEngine.Video.VideoPlayer vp, string message)
    {
        StartVideo();
        VideoCanvas.transform.GetChild(2).GetComponent<Text>().text = "Ошибка при загрузке, пробуем загрузить снова";
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Stop();
        Debug.Log("стоп");
        VideoCanvas.SetActive(false);
    }
}
