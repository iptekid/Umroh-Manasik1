using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<VideoData> mainData;
    public List<VideoData> mainData2;
    public List<VideoDataSorting> sortingData;
    public TextAsset textAsset;
    public bool isSort;
    void Start()
    {
        isSort = false;
        //VideoData data = LoadFromJson();
        //mainData2.Add(data);
        LoadCourses();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public List<VideoData> courseCollections;


    public void LoadCourses()
    {
        try
        {
            if (textAsset != null)
            {
                // Since Unity's JsonUtility doesn't directly deserialize JSON arrays,
                // we need to wrap the array in a temporary object
                //string wrappedJson = "{ \"videoDatas\": " + textAsset.text + "}";
                //Debug.Log(wrappedJson);
                string wrappedJson = textAsset.text ;
                VideoDataWrapper wrapper = JsonUtility.FromJson<VideoDataWrapper>(wrappedJson);
                mainData2 = wrapper.videoDatas;

                // Log successful loading and course information
                Debug.Log($"Successfully loaded {mainData2.Count} videoDatas collections");
                PrintCourseInfo();
            }
            else
            {
                Debug.LogError("JSON TextAsset is not assigned!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading videoDatas: {e.Message}");
        }
    }

    // Helper method to print course information
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

    // Method to get all course collections
    public List<VideoData> GetAllCourses()
    {
        return mainData2;
    }

    // Method to get a specific course by index
    public VideoData GetVideoDatas(int index)
    {
        if (mainData2 != null && index >= 0 && index < mainData2.Count)
        {
            return mainData2[index];
        }
        Debug.LogWarning($"videoDatas index {index} not found");
        return null;
    }

    // Method to get a specific course by name
    public VideoData GetVideoDatasByName(string courseName)
    {
        if (mainData2 != null)
        {
            return mainData2.Find(course => course.nama.Equals(courseName, StringComparison.OrdinalIgnoreCase));
        }
        return null;
    }

    // Method to search for videos across all courses
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

    // Recursive helper method to search through nested video data
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

    public void SortingData() {
        for (int index = 0; index < mainData2.Count; index++) {
            int i;
            i = index;
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
                int j;
                j = index2;
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
                    int k;
                    k = index3;
                    VideoDataSorting v3 = new VideoDataSorting();
                    v3.parent = v.nama +" - "+v2.nama;
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
