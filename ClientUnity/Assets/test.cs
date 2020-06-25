using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class test : MonoBehaviour
{
    public GameObject[] but;
    float posX;
    float posY;
    private void Start()
    {
        posX = but[0].transform.position.x;
        posY = but[0].transform.position.y;
        print(posX);
        but[0].transform.localPosition = new Vector2(posX + 10,posY);
        print(posX);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 vect = Input.mousePosition;

            if (vect.y > (1280 - 1280 / 6))
            {
                print(vect);
            }
            try
            {
                but[0].transform.localPosition = new Vector3(vect.x/2,vect.y/2,vect.z);
            }
            catch { }
            
        }
        
    }
}

//public Text tt;
//private void Start()
//{
//    // 0-Nano | 1-Bio | 2-IT | 3-Robo | 4-HiTech | 5-PromDiz
//    tt.text = $"Данные:\n" +
//        $"email: {Data.EMAIL}\n" +
//        $"nick: {Data.NICK}\n" +
//        $"ID: {Data.ID}\n" +
//        $"Level: {Data.LEVEL}\n" +
//        $"\n" +
//        $"Коэфиценты:\n" +
//        $"Nano: {Data.COEFICENT[0]}\n" +
//        $"Bio: {Data.COEFICENT[1]}\n" +
//        $"IT: {Data.COEFICENT[2]}\n" +
//        $"Robo: {Data.COEFICENT[3]}\n" +
//        $"HiTech: {Data.COEFICENT[4]}\n" +
//        $"Promdiz: {Data.COEFICENT[5]}";
//}