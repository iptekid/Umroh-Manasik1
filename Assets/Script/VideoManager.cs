using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class VideoManager : MonoBehaviour
{

    public DataManager dataManager;
    //public GameObject buttonPrefabs;
    //public Transform mainMenu, sub2Menu, sub3Menu;
    public List<Button> mainMenu, sub2MenuPengantar, sub2MenuMiqat, sub3Menu;
    public List<float> videossLenght;

    public VideoPlayer videoPlayer;
    public List<VideoClip> videoss;
    public GameObject transition;
    public GameObject mainvideo;
    void Start()
    {


        dataManager = GetComponent<DataManager>();
        Invoke(nameof(WaitSort), .5f);
        //transition.GetComponent<MeshRenderer>().material.mainTexture = null;
        for (int i = 0; i < videoss.Count; i++) {
            float getLenghts = (float)videoss[i].length;
            videossLenght.Add(getLenghts);
        }

        transition.GetComponent<OpacityAnimator>().FadeIn(0);
        mainvideo.GetComponent<OpacityAnimator>().FadeIn(0);
        transition.GetComponent<OpacityAnimator>().FadeOut(.5f);
        TempVidPlay(0);
        //GetComponent<ButtonManager>().Invoke("PlayMainMenu", videossLenght[0]);
        //Invoke(nameof(Highlights), videossLenght[0]);
        //TempVidPlay(0);
    }
    void Highlights() { 
        transition.GetComponent<OpacityAnimator>().FadeOut(.5f);
    }
    public void WaitSort() {
        //transition.GetComponent<MeshRenderer>().enabled = false;
        if (dataManager.isSort)
        {
            Attaching();
        }
        else {
            Invoke(nameof(WaitSort), .5f);
        }
    
    }
    public void Attaching() {
        for (int index = 0; index < dataManager.sortingData.Count; index++)
        {
            int i;
            i = index;
            dataManager.sortingData[i].button = mainMenu[i];
            dataManager.sortingData[i].button.name = dataManager.sortingData[i].nama;
            dataManager.sortingData[i].button.transform.GetChild(0).GetComponent<Text>().text = dataManager.sortingData[i].nama;
            

            for (int index2 = 0; index2 < dataManager.sortingData[i].videoData.Count; index2++)
            {
                int j;
                j = index2;
                List<Button> sub = new List<Button>();
                if (i == 0)
                {
                    sub = sub2MenuPengantar;
                }
                else if (i == 1) { 
                    sub = sub2MenuMiqat;
                }
                dataManager.sortingData[i].videoData[j].button = sub[j];
                dataManager.sortingData[i].videoData[j].button.name = dataManager.sortingData[i].videoData[j].nama;
                dataManager.sortingData[i].videoData[j].button.transform.GetChild(0).GetComponent<Text>().text = dataManager.sortingData[i].videoData[j].nama;

                for (int index3 = 0; index3 < dataManager.sortingData[i].videoData[j].videoData.Count; index3++)
                {
                    int k;
                    k = index3;

                    dataManager.sortingData[i].videoData[j].videoData[k].button = sub3Menu[k];
                    dataManager.sortingData[i].videoData[j].videoData[k].button.name = dataManager.sortingData[i].videoData[j].videoData[k].nama;
                    dataManager.sortingData[i].videoData[j].videoData[k].button.transform.GetChild(0).GetComponent<Text>().text = dataManager.sortingData[i].videoData[j].videoData[k].nama;
                }

            }
        }
    }

    public void TempVidPlay(int i) {
        if (i == 0)
        {
            //GetComponent<ButtonManager>().Invoke(nameof(ButtonManager.ResetingAll(true)), 0);
        }
        else {
            GetComponent<ButtonManager>().Invoke("ResetingAll", 0);
        }
        OpacitySphere(true);
        videoPlayer.Stop();
        videoPlayer.clip = videoss[i];
        videoPlayer.Play();
        float v = videossLenght[i];
        if (i == 0)
        {
            v = 15;
        }
        Invoke(nameof(WaitVidEnd), v);

    }
    public void OpacitySphere(bool isStart) {

        if (isStart)
        {
            transition.GetComponent<MeshRenderer>().enabled = true;
            transition.GetComponent<OpacityAnimator>().ActionPlays(0, 1, 2);
            mainvideo.GetComponent<OpacityAnimator>().FadeIn(.4f);
            Invoke(nameof(alt), 3);

        }
        else {
            transition.GetComponent<OpacityAnimator>().ActionPlays(0, 1,2);
            mainvideo.GetComponent<OpacityAnimator>().FadeOut(.4f);
            Invoke(nameof(alt), 3);

        }

    }
    public void alt()
    {

        transition.GetComponent<MeshRenderer>().enabled = false;


    }
    public void WaitVidEnd() { 
        //transition.GetComponent<MeshRenderer>().enabled = false;

        OpacitySphere(false);
        GetComponent<ButtonManager>().Invoke("PlayMainMenu", 0);

    }

}