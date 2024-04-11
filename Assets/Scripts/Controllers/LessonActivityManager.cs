using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonActivityManager : MonoBehaviour
{
    [SerializeField] private TMP_Text selectedLesson;
    [SerializeField] private UnityEngine.UI.Image lessonImage;
    [SerializeField] private Button viewMoreButton;

    private void Start() 
    {
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

        var lessonTitle = PlayerPrefs.GetString("SelectedLesson", "Wika at Katangian");
        selectedLesson.text = lessonTitle;

        viewMoreButton.enabled = false;
        string spriteDirectory = lessonTitle.ToLower().Replace(" ", string.Empty).Replace("-", string.Empty);
        string spritePath = lessonTitle.ToLower().Replace(" ", "_");
        Sprite sprite = Resources.Load<Sprite>($"Sprites/{spriteDirectory}/{spritePath}");
        if (sprite != null)
        {
            // Set the sprite to the Image component
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
}
