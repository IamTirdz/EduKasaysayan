using UnityEngine;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private GameObject videoPlayer;
    private VideoDataPlayer videoDataPlayer;

    void Start()
    {
        videoDataPlayer = videoPlayer.GetComponent<VideoDataPlayer>();
    }

    void OnMouseDown() 
    {
        videoDataPlayer.KnobOnPressDown();
    }

    void OnMouseUp()
    {
        videoDataPlayer.KnobOnRelease();
    }

    void OnMouseDrag()
    {
        videoDataPlayer.KnobOnDrag();
    }
}
