using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class Func
{

    public static int[] ConvertMassTo_int(string[] str)
    {    
        int[] it = new int[str.Length];
        try
        {         
            for (int i = 0; i < str.Length; i++)
            {
                it[i] = int.Parse(str[i]);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        return it;

    }

    public static void Clearing_Fields(GameObject[] obj)
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
