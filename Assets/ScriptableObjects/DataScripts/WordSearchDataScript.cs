using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word Search", menuName = "Create Word Search", order = 1)]
public class WordSearchDataScript : ScriptableObject
{
    public List<string> words;
}
