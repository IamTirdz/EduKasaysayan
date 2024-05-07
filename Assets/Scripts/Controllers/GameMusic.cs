using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField] private string musicName;

    void Start() 
    {
        AudioManager.instance.PlayMusic(musicName);
    }
}
