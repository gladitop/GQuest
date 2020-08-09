using AdminGQuest.Other;
using System;
using System.Net.Sockets;
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
                Data.Client = new TcpClient();
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
                Hide();
            }
            else
            {
                InitializeComponent();//Это загрузка формы
            }
        }

        private void btlogin_Click(object sender, RoutedEventArgs e)//Вход
        {
            Function.SendServer($"%LOGA:{tblogin.Text}:{tbpass.Password}");
            tblogin.Text = Function.ReceiveServer();
            if (tblogin.Text == "%GODLOG:☼")//TODO:Какой-то херня (исправить)
            {
                Main m = new Main();
                m.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Неверный пароль!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*
            Main m = new Main();
            m.Show();
            Hide();
            */
        }

        private void main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
