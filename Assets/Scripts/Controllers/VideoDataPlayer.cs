using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoDataPlayer : MonoBehaviour
{
    [SerializeField] private GameObject cinemaPlayer;
    [SerializeField] private GameObject buttonPlay;
    [SerializeField] private GameObject buttonPause;
    [SerializeField] private GameObject knob;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject progressBackground;

    private float maxKnobValue;
    private float newKnobX;
    private float maxKnobX;
    private float minKnobX;
    private float knobPosY;
    private float simpleKnobValue;
    private float knobValue;
    private float progressBarWidth;
    private bool knobIsDragging;
    private bool videoIsJumping = false;
    private bool videoIsPlaying = false;
    private VideoPlayer videoPlayer;

    void Start()
    {
        knobPosY = knob.transform.localPosition.y;
        videoPlayer = GetComponent<VideoPlayer>();
        buttonPause.SetActive(true);
        buttonPlay.SetActive(false);
        videoPlayer.frame = (long)100;
        progressBarWidth = progressBackground.GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    void Update()
    {
        if (!knobIsDragging && !videoIsJumping)
        {
            if (videoPlayer.frameCount > 0)
            {
                float progress = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
                progressBar.transform.localScale = new Vector3(progressBarWidth * progress, progressBar.transform.localScale.y, 0);
                knob.transform.localPosition = new Vector2(progressBar.transform.localPosition.x + (progressBarWidth * progress), knob.transform.localPosition.y);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(pos));

            if (hitCollider != null && hitCollider.CompareTag(buttonPause.tag))
            {
                PlayVideo();
            }

            if (hitCollider != null && hitCollider.CompareTag(buttonPlay.tag))
            {
                print("play button");
                PlayVideo();
            }
        }
    }
    
    public void KnobOnPressDown()
    {
        StopVideo();
        minKnobX = progressBar.transform.localPosition.x;
        maxKnobX = minKnobX + progressBarWidth;
    }    

    public void KnobOnRelease()
    {
        knobIsDragging = false;
        CalcKnobSimpleValue();
        PlayVideo();
        JumpVideo();
        StartCoroutine(DelayedSetVideoIsJumping(false));
    }

    IEnumerator DelayedSetVideoIsJumping(bool isJumping)
    {
        yield return new WaitForSeconds(2);
        SetVideoIsJumping(isJumping);
    }

    public void KnobOnDrag()
    {
        knobIsDragging = true;
        videoIsJumping = true;
        Vector3 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        knob.transform.position = new Vector2(currentPosition.x, currentPosition.y);
        newKnobX = knob.transform.localPosition.x;

        if (newKnobX > maxKnobX)
        {
            newKnobX = maxKnobX;
        }

        if (newKnobX < minKnobX)
        {
            newKnobX = minKnobX;
        }

        knob.transform.localPosition = new Vector2(newKnobX, knobPosY);
        CalcKnobSimpleValue();
        progressBar.transform.localScale = new Vector3(simpleKnobValue * progressBarWidth, progressBar.transform.localScale.y, 0);
    }

    private void ButtonPlayVideo()
    {
        if (videoIsPlaying)
        {
            StopVideo();
        }
        else
        {
            PlayVideo();
        }
    }

    private void PlayVideo()
    {
        videoIsPlaying = true;
        videoPlayer.Play();
        buttonPause.SetActive(true);
        buttonPlay.SetActive(false);
    }

    private void StopVideo()
    {
        videoIsPlaying = false;
        videoPlayer.Pause();
        buttonPause.SetActive(false);
        buttonPlay.SetActive(true);
    }

    private void CalcKnobSimpleValue()
    {
        maxKnobValue = maxKnobX - minKnobX;
        knobValue = knob.transform.localPosition.x - minKnobX;
        simpleKnobValue = knobValue / maxKnobValue;
    }

    private void JumpVideo()
    {
        var frame = videoPlayer.frameCount * simpleKnobValue;
        videoPlayer.frame = (long)frame;
    }

    private void SetVideoIsJumping(bool isJumping)
    {
        videoIsJumping = isJumping;
    }
}
