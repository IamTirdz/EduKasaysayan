using TMPro;
using UnityEngine;

public class LessonUIManager : MonoBehaviour
{
    [Header("Lesson UI Manager")]
    [SerializeField] public TMP_Text lessonTitle;
    [SerializeField] public TMP_Text[] lessonText;
    [SerializeField] public UnityEngine.UI.Image[] lessonImage;
    [SerializeField] public UnityEngine.UI.Image[] lessonVideoImage;
    [SerializeField] public GameObject previousButtonPrefab;
    [SerializeField] public GameObject nextButtonPrefab;
    [SerializeField] public GameObject backToMenuButtonPrefab;
}
