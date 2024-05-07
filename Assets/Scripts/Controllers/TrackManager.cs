using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class TrackManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public VideoPlayer videoPlayer;
    public RectTransform containerPanel;

    public Button playButton;
    public Button pauseButton;
    public bool autoPlay;

    public GameObject blackScreen;

    public Button fullScreenButton;
    public Button minimizeButton;
    private bool isFullscreen = false;

    Slider slider;
    bool isSlide = false;
    
    void Start()
    {
        slider = GetComponent<Slider>();
        minimizeButton.gameObject.SetActive(false);
        blackScreen.SetActive(false);

        containerPanel.sizeDelta = new Vector2(1280f, 900f);

        if (autoPlay)
        {
            PlayVideo();
        }

        
            
    }

    void Update()
    {
        if (!isSlide && videoPlayer.isPlaying)
        {
            slider.value = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
        }       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSlide = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PauseVideo();
        float frame = (float)slider.value * (float)videoPlayer.frameCount;        
        videoPlayer.frame = (long)frame;
        isSlide = false;
        PlayVideo();
    }

    public void PlayVideo()
    {
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        videoPlayer.Play();
    }

    public void PauseVideo()
    {
        playButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);

        videoPlayer.Pause();
    }

    public void ToggleFullScreen()
    {
        isFullscreen = !isFullscreen;
        //Screen.fullScreen = isFullscreen;

        blackScreen.SetActive(true);
        SetContainerRotation(-90f);
        
        containerPanel.sizeDelta = new Vector2(2560f, 1340f);

        // containerPanel.anchorMin = Vector2.zero;
        // containerPanel.anchorMax = Vector2.one;
        
        // containerPanel.sizeDelta = Vector2.zero;
        // //containerPanel.position = Vector2.zero;
        // //containerPanel.sizeDelta = new Vector2(Screen.currentResolution.height, Screen.currentResolution.width);
        // containerPanel.anchoredPosition = Vector2.zero;

        // Debug.Log("ToggleFullScreen width: " + Screen.width);
        // Debug.Log("ToggleFullScreen height: " + Screen.height);

        // // Store the original width and height
        // float originalWidth = containerPanel.rect.width;
        // float originalHeight = containerPanel.rect.height;

        // //SetContainerRotation(-90f);
        // containerPanel.rotation = Quaternion.Euler(0f, 0f, -90f);
        
        // float newWidth = containerPanel.rect.width;
        // float newHeight = containerPanel.rect.height;

        // //landscape
        // containerPanel.sizeDelta = new Vector2(newHeight, newWidth);
        //containerPanel.sizeDelta = new Vector2(containerPanel.rect.width, containerPanel.rect.height);

        //AdjustVideoSizeAndFlipButtons();

        fullScreenButton.gameObject.SetActive(false);
        minimizeButton.gameObject.SetActive(true);
    }

    void SetContainerRotation(float zRotation)
    {
        Quaternion rawImageRotation = containerPanel.transform.rotation;
        rawImageRotation.eulerAngles = new Vector3(rawImageRotation.eulerAngles.x, rawImageRotation.eulerAngles.y, zRotation);
        
        containerPanel.transform.rotation = rawImageRotation;
    }

    public void ToggleMinimizeScreen()
    {
        //videoPanel.sizeDelta = new Vector2(1280f, 900f);
        blackScreen.SetActive(false);
        SetContainerRotation(0f);
        
        containerPanel.sizeDelta = new Vector2(1280f, 900f);

        Debug.Log("ToggleMinimizeScreen width: " + Screen.width);
        Debug.Log("ToggleMinimizeScreen height: " + Screen.height);

        fullScreenButton.gameObject.SetActive(true);
        minimizeButton.gameObject.SetActive(false);
    }
}
