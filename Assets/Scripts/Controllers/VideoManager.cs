using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

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
