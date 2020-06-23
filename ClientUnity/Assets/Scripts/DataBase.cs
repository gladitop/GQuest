using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

static class DataBase
{
    private const string fileName = "db.db";

    private static SqliteConnection connection;
    private static SqliteCommand command;

    #region Путь к бд, открытие, закрытие бд.

    private static string GetDatabasePath()
    {
    #if UNITY_EDITOR
        return Path.Combine(Application.streamingAssetsPath, fileName);
    #elif UNITY_STANDALONE
        string filePath = Path.Combine(Application.dataPath, fileName);
    if(!File.Exists(filePath)) UnpackDatabase(filePath);
        return filePath;
    #elif UNITY_ANDROID
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) UnpackDatabase(filePath);
        return filePath;
    #endif
    } //Возвращает путь к БД, eсли её нет в нужной папке на андроиде, то копирует её с исходного apk файла
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);
    } //Распаковывает базу данных в указанный путь.   

    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + GetDatabasePath());
        command = new SqliteCommand(connection);
        connection.Open();
    } //открывает подключение к БД.  
    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    } //закрывает подключение к БД

    #endregion

    #region методы запроса

    public static void ExecuteQueryAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    } // выполняет запрос query(INSERT, UPDATE, DELETE)
   
    public static string ExecuteScalarAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    } //возвращает значение 1 строки 1 столбца, если оно имеется.(SELECT)
 
    public static DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    } //возвращает таблицу, которая является результатом выборки запроса.(SELECT)

    #endregion

    //примеры
    //DataTable info = DataBase.GetTable("SELECT * FROM Scores ORDER BY score DESC;");
    //string str = info.Rows[0][1].ToString(); выбираем из таблицы(строк)
    //string str = DataBase.ExecuteScalarAnswer($"SELECT nickname FROM Player WHERE id_player = {idBestPlayer};"); получаем строку
}