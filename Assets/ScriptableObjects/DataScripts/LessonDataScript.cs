using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lesson Data", menuName = "Create Lessons", order = 1)]
public class LessonDataScript : ScriptableObject
{
    public List<Lesson> lessons;
}
