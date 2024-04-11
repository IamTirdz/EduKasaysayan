using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingUI;

    public void ShowPanel(bool isShown)
    {
        settingUI.SetActive(isShown);
    }
}
