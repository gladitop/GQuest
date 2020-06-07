using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DinamycServer
{
    public static class Data
    {
        public const int Port = 908;//Порт сервера
        public static List<TcpClient> TpClient = new List<TcpClient>(); //Инфа о подключённых сокетах

        public class ClientInfo //Инфо о клиенте (онлайн)
        {
            public ClientInfo(TcpClient socket, string email, string password, string nick, long id, long? point) //Инфо о клиенте
            {
                if(socket != null) {Socket = socket;}                      
                Email = email;
                Password = password;
                Nick = nick;
                Point = point;
                ID = id;
            }

            public TcpClient Socket { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Nick { get; set; }
            public long ID { get; set; }
            public long? Point { get; set; }
        }

        public class QuestionInfo
        {
            public QuestionInfo(string title, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6)
            {
                Title = title;
                Answer1 = answer1;
                Answer2 = answer2;
                Answer3 = answer3;
                Answer4 = answer4;
                Answer5 = answer5;
                Answer6 = answer6;
            }

            public string Title { get; set; }//Вопрос

            //Ответы на Вопросы
            public string Answer1 { get; set; }
            public string Answer2 { get; set; }
            public string Answer3 { get; set; }
            public string Answer4 { get; set; }
            public string Answer5 { get; set; }
            public string Answer6 { get; set; }
            //Ответы на Вопросы
        }//Инфо о ВОПРОСЕ   

        public class QuestionsInfo//Инфо о вопросАХ
        {
            public QuestionsInfo(QuestionInfo question1, QuestionInfo question2, QuestionInfo question3, QuestionInfo question4, QuestionInfo question5)
            {
                Question1 = question1;
                Question2 = question2;
                Question3 = question3;
                Question4 = question4;
                Question5 = question5;
            }

            //Вопросы
            QuestionInfo Question1 { get; set; }
            QuestionInfo Question2 { get; set; }
            QuestionInfo Question3 { get; set; }
            QuestionInfo Question4 { get; set; }
            QuestionInfo Question5 { get; set; }
            //Вопросы
        }
    }
}