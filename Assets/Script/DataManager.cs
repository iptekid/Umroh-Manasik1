using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections;

public class DataManager : MonoBehaviour
{
    public List<VideoData> mainData;
    public List<VideoData> mainData2;
    public List<VideoDataSorting> sortingData;
    public bool isSort;
    private string dataUrl = "https://sabr.iptec.id/data.txt";

    void Start()
    {
        isSort = false;
        StartCoroutine(LoadCoursesFromUrl());
    }

    private IEnumerator LoadCoursesFromUrl()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(dataUrl))
        {
            // Send the request and wait for response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error loading data: {webRequest.error}");
            }
            else
            {
                try
                {
                    string jsonData = webRequest.downloadHandler.text;
                    VideoDataWrapper wrapper = JsonUtility.FromJson<VideoDataWrapper>(jsonData);
                    mainData2 = wrapper.videoDatas;

                    Debug.Log($"Successfully loaded {mainData2.Count} videoDatas collections");
                    PrintCourseInfo();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing data: {e.Message}");
                }
            }
        }
    }

    private void PrintCourseInfo()
    {
        for (int i = 0; i < mainData2.Count; i++)
        {
            VideoData vd = mainData2[i];
            Debug.Log($"videoDatas {i + 1}: {vd.nama}");
            Debug.Log($"Description: {vd.description}");
            Debug.Log($"Number of videos: {vd.videoData.Count}");
            Debug.Log("-------------------");
        }
        SortingData();
    }

    public List<VideoData> GetAllCourses()
    {
        return mainData2;
    }

    public VideoData GetVideoDatas(int index)
    {
        if (mainData2 != null && index >= 0 && index < mainData2.Count)
        {
            return mainData2[index];
        }
        Debug.LogWarning($"videoDatas index {index} not found");
        return null;
    }

    public VideoData GetVideoDatasByName(string courseName)
    {
        if (mainData2 != null)
        {
            return mainData2.Find(course => course.nama.Equals(courseName, StringComparison.OrdinalIgnoreCase));
        }
        return null;
    }

    public List<VideoData> SearchVideos(string searchTerm)
    {
        List<VideoData> results = new List<VideoData>();
        if (mainData2 == null) return results;

        foreach (var vd in mainData2)
        {
            SearchInVideoData(vd, searchTerm, results);
        }
        return results;
    }

    private void SearchInVideoData(VideoData videoData, string searchTerm, List<VideoData> results)
    {
        if (videoData.nama.ToLower().Contains(searchTerm.ToLower()) ||
            videoData.description.ToLower().Contains(searchTerm.ToLower()))
        {
            results.Add(videoData);
        }

        if (videoData.videoData != null)
        {
            foreach (var subVideo in videoData.videoData)
            {
                SearchInVideoData(subVideo, searchTerm, results);
            }
        }
    }

    public void SortingData()
    {
        for (int index = 0; index < mainData2.Count; index++)
        {
            int i = index;
            VideoDataSorting v = new VideoDataSorting();
            v.parent = "Main Menu";
            v.nama = mainData2[i].nama;
            v.hasVideo = mainData2[i].hasVideo;
            v.urlVideo = mainData2[i].urlVideo;
            v.hasAudio = mainData2[i].hasAudio;
            v.urlAudio = mainData2[i].urlAudio;
            v.description = mainData2[i].description;
            List<VideoDataSorting> vVDS = new List<VideoDataSorting>();
            v.videoData = vVDS;

            for (int index2 = 0; index2 < mainData2[i].videoData.Count; index2++)
            {
                int j = index2;
                VideoDataSorting v2 = new VideoDataSorting();
                v2.parent = v.nama;
                v2.nama = mainData2[i].videoData[j].nama;
                v2.hasVideo = mainData2[i].videoData[j].hasVideo;
                v2.urlVideo = mainData2[i].videoData[j].urlVideo;
                v2.hasAudio = mainData2[i].videoData[j].hasAudio;
                v2.urlAudio = mainData2[i].videoData[j].urlAudio;
                v2.description = mainData2[i].videoData[j].description;
                vVDS.Add(v2);
                List<VideoDataSorting> vVDS2 = new List<VideoDataSorting>();
                v2.videoData = vVDS2;

                for (int index3 = 0; index3 < mainData2[i].videoData[j].videoData.Count; index3++)
                {
                    int k = index3;
                    VideoDataSorting v3 = new VideoDataSorting();
                    v3.parent = v.nama + " - " + v2.nama;
                    v3.nama = mainData2[i].videoData[j].videoData[k].nama;
                    v3.hasVideo = mainData2[i].videoData[j].videoData[k].hasVideo;
                    v3.urlVideo = mainData2[i].videoData[j].videoData[k].urlVideo;
                    v3.hasAudio = mainData2[i].videoData[j].videoData[k].hasAudio;
                    v3.urlAudio = mainData2[i].videoData[j].videoData[k].urlAudio;
                    v3.description = mainData2[i].videoData[j].videoData[k].description;
                    vVDS2.Add(v3);
                }
            }
            sortingData.Add(v);
        }
        isSort = true;
    }
}