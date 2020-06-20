using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private void Start()
    {
        // Получаем отсортированную таблицу лидеров
        DataTable scoreboard = MyDataBase.GetTable("SELECT * FROM Scores ORDER BY score DESC;");
        // Получаем id лучшего игрока
        int idBestPlayer = int.Parse(scoreboard.Rows[0][1].ToString());
        // Получаем ник лучшего игрока
        string nickname = MyDataBase.ExecuteQueryWithAnswer($"SELECT nickname FROM Player WHERE id_player = {idBestPlayer};");
        Debug.Log($"Лучший игрок {nickname} набрал {scoreboard.Rows[0][2].ToString()} очков.");
    }
}
