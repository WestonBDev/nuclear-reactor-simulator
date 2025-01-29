using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialVideoHandler : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] List<VideoClip> clipList = new List<VideoClip>();
    
    public void PickAndPlay(int lang)
    {
        /*
         * 0 eng , 1 spa, 2...
         */
        videoPlayer.clip = clipList[lang];
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        videoPlayer.clip = null;
    }
}
