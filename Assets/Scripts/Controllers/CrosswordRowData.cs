using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordRowData : MonoBehaviour
{
    public CrosswordData[] tiles { get; private set; }

    private void Awake() 
    {
        tiles = GetComponentsInChildren<CrosswordData>();    
    }
}
