using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager : MonoBehaviour
{
    [SerializeField] private TMP_Text selectedGame;
    [SerializeField] private UnityEngine.UI.Image gameIcon;
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text selectedGameHint;

    private void Start() 
    {
        if (gameIcon == null)
        {
            Debug.Log("Image component was not set."); 
            return;
        }

        if (playButton == null)
        {
            Debug.LogError("TextMeshProUGUI button component is not attached.");
            return;
        }

        var gameTitle = PlayerPrefs.GetString("ActivityGameSelected", "know_your_label!");
        selectedGame.text = gameTitle;

        playButton.enabled = false;
        string spriteDirectory = gameTitle.ToLower().Replace(" ", string.Empty).Replace("!", string.Empty);
        string spritePath = gameTitle.ToLower().Replace(" ", "_");
        Sprite sprite = Resources.Load<Sprite>($"Sprites/{spriteDirectory}/{spritePath}");
        if (sprite != null)
        {
            // Set the sprite to the Image component
            gameIcon.sprite = sprite;
            playButton.enabled = true;
            selectedGameHint.text = GetHints(gameTitle);
        }
        else
        {
            playButton.enabled = false;
            Debug.LogError("Sprite not found at path: Assets/Resources/Sprites/" + spritePath);
            return;
        }
    }

    private string GetHints(string gameTitle)
    {
        return gameTitle switch
        {
            "Know Your Label!" => "Panuto: Basahin at unawaing mabuti ang mga sumusunod na pahayag o tanong, matalinong pindutin ang tamang kasagutan.",
            "PicTumpak" => "Panuto: Suriin ang mga larawan upang makabuo ng salita na may kinalaman sa talakayan.",
            "Kahit Konting Pagtingin" => "Panuto: Hanapin ang mga salita na may kaugnay sa heograpiyang pantao.",
            _ => "Panuto: Basahin at unawaing mabuti ang mga sumusunod na pahayag o tanong, matalinong pindutin ang tamang kasagutan."
        };
    }
}
