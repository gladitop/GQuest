using UnityEngine;
using UnityEngine.UI;

public class GovnoTest : MonoBehaviour
{
    [Header("Раунд, вопросы")]
    public Text PText;
    public Text QuestionText;
    [Header("Текст вопроса на кнопках")]
    public Text[] but;
    [Header("Объект-Клиент")]
    public GameObject Client_Main;

    private int question = 1; //текучий вопрос
    private int points = 0;  //кол-во набранных очков

    public void Start()
    {
        PText.text = ($"Вопрос {question}/5");
        QuestionText.text = "Cколько вам лет?";
        but[0].text = "10 - 13";
        but[1].text = "13 - 16";
        but[2].text = "16 - 88";
        but[3].text = "меньше месяца";
    } //1 вопрос

    void Test(int button)
    {
        if      (question == 1)
        {
            QuestionText.text = "Ваш пол:";
            but[0].text = "мужчина";
            but[1].text = "женщина";
            but[2].text = "не знаю";
            but[3].text = "лучше вам не знать";
            if (button == 1) {points += 1; question++;}
            if (button == 2) {points += 0; question++;}
            if (button == 3) {points += 0; question++;}
            if (button == 4) {points += 2; question++;}
        }    //Переход на 2 вопрос
        else if (question == 2)
        {
            QuestionText.text = "Что бы вы выбрали в качестве защиты от маньяка?";
            but[0].text = "бухло и подругу";
            but[1].text = "нож";
            but[2].text = "сына маминой подруги";
            but[3].text = "Iphone";
            if (button == 1) {points += 0; question++;}
            if (button == 2) {points += 0; question++;}
            if (button == 3) {points += 1; question++;}
            if (button == 4) {points += 2; question++;}
        }   //Переход на 3 вопрос
        else if (question == 3)
        {
            QuestionText.text = "Как вы зарабатываете?";
            but[0].text = "что такое работать?";
            but[1].text = "участвую в кастинге с 5 чернокожими незнакомцами";
            but[2].text = "работаю не покладая рук";
            but[3].text = "беру деньги у родителей";
            if (button == 1) {points += 2; question++;}
            if (button == 2) {points += 0; question++;}
            if (button == 3) {points += 1; question++;}
            if (button == 4) {points += 3; question++;}
        }  //Переход на 4 вопрос
        else if (question == 4)
        {
            QuestionText.text = "Понравился ли тест?";
            but[0].text = "конечно";
            but[1].text = "нет";
            but[2].text = "а-а-а-а-а";
            but[3].text = "могли и лучше";
            if (button == 1) {points += 1; question++;}
            if (button == 2) {points += 2; question++;}
            if (button == 3) {points += 0; question++;}
            if (button == 4) {points += 2; question++;}
        } //Переход на 5 вопрос
        else if (question == 5)
        {
            if (button == 1) {points += 0; question++;}
            if (button == 2) {points += 0; question++;}
            if (button == 3) {points += 1; question++;}
            if (button == 4) {points += 2; question++;}
            End();
        }
        if (question <= 5) { PText.text = ($"Вопрос {question}/5"); }
    } //2-5 вопрос

    public void But1() {Test(1);}  
    public void But2() {Test(2);}   
    public void But3() {Test(3);}
    public void But4() {Test(4);}
 
    private void End()
    {
        //Client_Main.GetComponent<Client>().SendScore(points);
        this.gameObject.SetActive(false);
    }
}