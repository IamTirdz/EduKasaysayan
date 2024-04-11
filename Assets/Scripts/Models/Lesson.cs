using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lesson
{
    public string lessonTitle;
    public List<Content> options;    
}

[System.Serializable]
public class Content
{
    public string lessonText;
    public Sprite lessonImage;
    public QuestionType lessonType; 
}