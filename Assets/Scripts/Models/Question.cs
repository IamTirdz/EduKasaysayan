using UnityEngine;

[System.Serializable]
public class Question
{
    public string questionText;
    public Sprite questionImage;
    public string[] options;
    public string correctAnswer;
    public QuestionType questionType;    
}