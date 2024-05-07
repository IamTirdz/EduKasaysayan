using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingUI;

    public void ShowPanel(bool isShown)
    {
        settingUI.SetActive(isShown);
    }
}
