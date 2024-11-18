using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    private static VideoManager _instance;
    public static VideoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("VideoManager");
                _instance = go.AddComponent<VideoManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Cache for preloaded video players
    private Dictionary<string, VideoPlayer> videoCache = new Dictionary<string, VideoPlayer>();
    private Dictionary<string, Action<VideoPlayer>> preloadCallbacks = new Dictionary<string, Action<VideoPlayer>>();

    // Cache size management
    [SerializeField] private int maxCacheSize = 5;
    private Queue<string> cacheOrder = new Queue<string>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PreloadVideo(string videoUrl, Action<VideoPlayer> onComplete)
    {
        if (string.IsNullOrEmpty(videoUrl))
        {
            Debug.LogError("Video URL is null or empty");
            return;
        }

        // Check if already cached
        if (videoCache.ContainsKey(videoUrl))
        {
            onComplete?.Invoke(videoCache[videoUrl]);
            return;
        }

        // Create new video player for preloading
        GameObject videoObject = new GameObject($"PreloadedVideo_{videoCache.Count}");
        videoObject.transform.SetParent(transform);
        VideoPlayer videoPlayer = videoObject.AddComponent<VideoPlayer>();

        // Configure video player
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.skipOnDrop = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;

        // Store callback
        preloadCallbacks[videoUrl] = onComplete;

        // Prepare video and set up completion callback
        videoPlayer.prepareCompleted += (source) => OnVideoPrepared(videoUrl, source);
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(string videoUrl, VideoPlayer source)
    {
        // Manage cache size
        if (videoCache.Count >= maxCacheSize)
        {
            string oldestUrl = cacheOrder.Dequeue();
            if (videoCache.TryGetValue(oldestUrl, out VideoPlayer oldPlayer))
            {
                Destroy(oldPlayer.gameObject);
                videoCache.Remove(oldestUrl);
            }
        }

        // Add to cache
        videoCache[videoUrl] = source;
        cacheOrder.Enqueue(videoUrl);

        // Invoke callback if exists
        if (preloadCallbacks.TryGetValue(videoUrl, out Action<VideoPlayer> callback))
        {
            callback?.Invoke(source);
            preloadCallbacks.Remove(videoUrl);
        }
    }

    public VideoPlayer GetCachedVideo(string videoUrl)
    {
        if (videoCache.TryGetValue(videoUrl, out VideoPlayer cachedPlayer))
        {
            return cachedPlayer;
        }
        return null;
    }

    public void ClearCache()
    {
        foreach (var player in videoCache.Values)
        {
            if (player != null)
            {
                Destroy(player.gameObject);
            }
        }
        videoCache.Clear();
        cacheOrder.Clear();
        preloadCallbacks.Clear();
    }

    private void OnDestroy()
    {
        ClearCache();
    }

    // Memory management
    private void OnLowMemory()
    {
        ClearCache();
    }
}