using AdminGQuest.Other;
using System;
using System.Windows;

namespace AdminGQuest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Соединение к серверу

            try
            {
                Data.Client.Connect(Data.IPServer, Data.PortServer);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка к серверу: {e.Message}", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            //Загрузка настройк
            SettingsManager SManager = new SettingsManager(Data.PathSave);
            Data.Settings = SManager.Load();

            //Если есть логин и пароль, то открываем Main

            if (Data.Settings.Login != null || Data.Settings.Password != null)
            {
                Main m = new Main();
                m.Show();
                Hide();//TODO:Исправить (оптимизация), Отправить
            }
            else
            {
                InitializeComponent();//Это загрузка формы
            }
        }

        private void btlogin_Click(object sender, RoutedEventArgs e)//Вход
        {
            Data.SendServer($"%LOGA:{tblogin.Text}:{tbpass.Password}");
            if (Data.ReceiveServer() == "%LOGOD")
            {
                Main m = new Main();
                m.Show();
                Hide();
            }
        }

        private void main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
