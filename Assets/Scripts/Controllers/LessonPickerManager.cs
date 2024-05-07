using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonPickerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text selectedLesson;
    [SerializeField] private Image lessonImage;
    [SerializeField] private Button viewMoreButton;
    [SerializeField] private List<bool> showIntroduction;
    [SerializeField] private GameObject introductionGO;
    [SerializeField] private GameObject contentGO;

    private string recentTitle;

    private void Start() 
    {
        introductionGO.SetActive(false);
        contentGO.SetActive(false);

        if (lessonImage == null)
        {
            Debug.Log("Image component was not set."); 
            return;
        }

        if (viewMoreButton == null)
        {
            Debug.LogError("TextMeshProUGUI button component is not attached.");
            return;
        }

        var lessonIndex = PlayerPrefs.GetInt("SelectedLessonIndex", 0);
        var lessonTitle = lessonIndex switch
        {
            1 => "Wika at Katangian",
            2 => "Relihiyon",
            3 => "Lahi at Pangkat-Etniko",
            _ => "Katangian Pisikal ng Daigdig"
        };

        recentTitle = selectedLesson.text;

        if (showIntroduction[lessonIndex])
        {
            introductionGO.SetActive(true);
            contentGO.SetActive(false);

            selectedLesson.text = lessonTitle;

            viewMoreButton.enabled = false;
            string spriteDirectory = lessonTitle.ToLower().Replace(" ", string.Empty).Replace("-", string.Empty);
            string spritePath = lessonTitle.ToLower().Replace(" ", "_");
            Sprite sprite = Resources.Load<Sprite>($"Sprites/{spriteDirectory}/{spritePath}");
            if (sprite != null)
            {
                lessonImage.sprite = sprite;
                viewMoreButton.enabled = true;
            }
            else
            {
                viewMoreButton.enabled = false;
                Debug.LogError($"Sprite not found at path: Assets/Resources/{spritePath}");
                return;
            }
        }
        else
        {
            introductionGO.SetActive(false);
            contentGO.SetActive(true);
        }
    }

    public void LoadLessons()
    {
        introductionGO.SetActive(false);
        contentGO.SetActive(true);

        selectedLesson.text = recentTitle;
    }
}
