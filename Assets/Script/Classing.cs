using System.Collections.Generic;
using UnityEngine;

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
public class VideoDataWrapper
{
    public List<VideoData> videoDatas;
}