using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Classing
{
    
}

[System.Serializable]
public class VideoData {
    public string nama;
    public bool hasVideo;
    public string urlVideo;
    public bool hasAudio;
    public string urlAudio;
    public string description;
    public List<VideoData> videoData;
}

[System.Serializable]
public class VideoDataSorting
{
    public string parent;
    public string nama;
    public bool hasVideo;
    public string urlVideo;
    public bool hasAudio;
    public string urlAudio;
    public string description;
    public Button button;
    public List<VideoDataSorting> videoData;
}
[System.Serializable]
public class VideoDataWrapper
{
    public List<VideoData> videoDatas;
}
public interface IOpacityAnimator
{
    void FadeTo(float targetOpacity, float duration);
    void FadeIn(float duration);
    void FadeOut(float duration);
    void SetOpacity(float opacity);
    void Show();
    void Hide();
    void StartPingPong(float minOpacity, float maxOpacity, float duration);
    void StopAnimation();
}