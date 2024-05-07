using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question Data", menuName = "Create Quizzes", order = 1)]
public class QuizDataScript : ScriptableObject
{
    public List<Question> questions;
}
