using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word Data", menuName = "Create Word Image", order = 1)]
public class WordDataScript : ScriptableObject
{
    public List<Word> words;
}
