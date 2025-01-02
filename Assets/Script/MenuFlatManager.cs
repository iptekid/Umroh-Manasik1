using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using System;

public class MenuFlatManager : MonoBehaviour
{
    public bool isDebugingSkip;
    public DataManager dataManager;
    public GameObject MenusTransform;
    public VideoObjects playerBackground;
    public VideoObjects playerMain;

    public List<CanvasGroup> menus;

    public List<Button> manasikMenu;
    public List<Button> miqatMenu;
    public List<Image> niatImage, rukunUmrohImage, laranganUmrohImage, perbolehanSaatIhramImage;
    public int curNiat, curRukunUmroh, curLaranganUmroh, curPerbolehanSaatIhram;
    public List<Button> pengantarNextMenu;
    public List<Button> toiletsMenu;
    public Transform menusToilet, startMenusToilet, endMenusToilet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
        //GameObject objectsMain2 = GameObject.FindGameObjectWithTag("Player");
        //GameObject objectsMain = objectsMain2.transform.GetChild(0).GetChild(1).gameObject;
        //Debug.Log(objectsMain.transform.position);
        //MenusTransform.transform.DORotateQuaternion(objectsMain.transform.rotation, 0);
        //MenusTransform.transform.DOMove(new Vector3(objectsMain.transform.position.x + .27f, objectsMain.transform.position.y + .48f, objectsMain.transform.position.z + 2.9f), 0);
        
        //Vector3 targetDirection = objectsMain.transform.position - MenusTransform.transform.position;
        //targetDirection.y = 0;
        //Quaternion targetRotation = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(0, 0, 0);
        //transform.DORotateQuaternion(targetRotation, 0)
        //    .SetEase(Ease.OutExpo);



        dataManager = GetComponent<DataManager>();
        DoLoad();
    }
    public void DoLoad()
    {
        if (dataManager.isSort != true)
        {
            Invoke("DoLoad", .5f);
        }
        else
        {
            StartManasik();
            DoAssignButtonManasik();
            DoAssignButtonManasikMiqat();
            DoAssignButtonManasikPengantarToilet();
        }
    }
    public void DoAssignButtonManasik()
    {
        for (int i = 0; i < manasikMenu.Count; i++)
        {
            int index = i;
            if (index == 0)
            {
                //Pengantar
                manasikMenu[index].onClick.AddListener(delegate { InteractableMenu(1); });
            }
            else if (index == 1)
            {
                //Miqat
                manasikMenu[index].onClick.AddListener(delegate { InteractableMenu(7); });
            }
            else
            {
                manasikMenu[index].onClick.AddListener(delegate { DoPlayVideos(dataManager.mainData2[index].urlVideo); });
            }

        }
    }
    public void DoAssignButtonManasikMiqat()
    {
        for (int i = 0; i < miqatMenu.Count; i++)
        {
            int index = i;
            miqatMenu[index].onClick.AddListener(delegate
            {
                //DoPlayVideos(dataManager.mainData2[index].urlVideo); 
                DoPlayVideos(dataManager.mainData2[1].videoData[index].urlVideo, 7);
            });
        }
    }
    public void DoAssignButtonManasikPengantarToilet()
    {
        for (int i = 0; i < toiletsMenu.Count; i++)
        {
            int index = i;
            toiletsMenu[index].onClick.AddListener(delegate
            {
                //DoPlayVideos(dataManager.mainData2[index].urlVideo); 
                DoPlayVideos(dataManager.mainData2[0].videoData[4].videoData[index].urlVideo, 6);
            });
        }
    }

    public void StartManasik()
    {
        playerBackground.videoPlayer.Play();
        StartCoroutine(manasikPlayStart());
    }
    public IEnumerator manasikPlayStart()
    {
        playerMain.opacityAnimator.FadeOut(0);
        yield return new WaitForSeconds(.01f);
        playerMain.opacityAnimator.FadeIn(1);
        playerMain.videoPlayer.prepareCompleted += OnVideoPrepared1;
        playerMain.videoPlayer.Prepare();
    }
    public void OnVideoPrepared1(VideoPlayer vp)
    {
        // Get the video length in seconds
        float videoLength = (float)playerMain.videoPlayer.length;
        Debug.Log($"Video duration: {videoLength} seconds");
        playerMain.videoPlayer.Play();
        Debug.Log(isDebugingSkip + "ini ciopy");
        if (isDebugingSkip)
        {
            Invoke("AfterOpening", 5f);//<<-- TIME TO SKIP
        }
        else
        {
            Invoke("AfterOpening", videoLength);//<<-- TIME TO SKIP
        }
    }
    public void AfterOpening()
    {
        Debug.Log("AfterOpening called");
        //GameObject objectsMain2 = GameObject.FindGameObjectWithTag("Player");
        //GameObject objectsMain = objectsMain2.transform.GetChild(0).GetChild(1).gameObject;
        //Debug.Log(objectsMain.transform.position);
        //MenusTransform.transform.DORotateQuaternion(objectsMain.transform.rotation, 0);
        //MenusTransform.transform.DOMove(new Vector3(objectsMain.transform.position.x + .27f, objectsMain.transform.position.y + .48f, objectsMain.transform.position.z + 2.9f), 0);
        playerMain.opacityAnimator.FadeOut(1);
        playerMain.videoPlayer.Stop();
        playerMain.videoPlayer.prepareCompleted -= OnVideoPrepared1;
        InteractableMenu(0);

    }


    //DO PLAYVIDEOS
    public string curUrl;
    public void DoPlayVideos(string url)
    {
        playerMain.videoPlayer.prepareCompleted -= OnVideoPrepared1;
        playerMain.videoPlayer.prepareCompleted -= OnVideoPrepared;
        playerMain.videoPlayer.Stop();
        curUrl = url;
        DisableMenuAll();
        StartCoroutine(VideosPlays());
    }
    int curindex;
    public void DoPlayVideos(string url, int index)
    {
        playerMain.videoPlayer.prepareCompleted -= OnVideoPrepared1;
        playerMain.videoPlayer.prepareCompleted -= OnVideoPrepared;
        playerMain.videoPlayer.Stop();
        curUrl = url;
        DisableMenuAll();
        curindex = index;
        StartCoroutine(VideosPlaysP());
    }

    public IEnumerator VideosPlays()
    {
        playerMain.opacityAnimator.FadeOut(0);
        yield return new WaitForSeconds(.01f);
        playerMain.videoPlayer.url = curUrl;
        playerMain.videoPlayer.prepareCompleted += OnVideoPrepared;
        playerMain.videoPlayer.Prepare();
    }
    public IEnumerator VideosPlaysP()
    {
        playerMain.opacityAnimator.FadeOut(0);
        yield return new WaitForSeconds(.01f);
        playerMain.videoPlayer.url = curUrl;
        playerMain.videoPlayer.prepareCompleted += OnVideoPreparedP;
        playerMain.videoPlayer.Prepare();
    }
    public void OnVideoPrepared(VideoPlayer vp)
    {
        // Get the video length in seconds
        float videoLength = (float)playerMain.videoPlayer.length;
        Debug.Log($"Video duration: {videoLength} seconds");
        playerMain.opacityAnimator.FadeIn(1);
        playerMain.videoPlayer.Play();
        if (isDebugingSkip)
        {
            Invoke("AfterOpening", 5f);//<<-- TIME TO SKIP
        }
        else
        {
            Invoke("AfterOpening", videoLength);//<<-- TIME TO SKIP
        }
    }
    public void OnVideoPreparedP(VideoPlayer vp)
    {
        // Get the video length in seconds
        float videoLength = (float)playerMain.videoPlayer.length;
        Debug.Log($"Video duration: {videoLength} seconds");
        playerMain.opacityAnimator.FadeIn(1);
        playerMain.videoPlayer.Play();
        if (isDebugingSkip)
        {
            Invoke("AfterOpening", 5f);//<<-- TIME TO SKIP
        }
        else
        {
            Invoke("AfterOpening", videoLength);//<<-- TIME TO SKIP
        }
    }
    public void AfterVideoComplete()
    {
        playerMain.opacityAnimator.FadeOut(1);
        playerMain.videoPlayer.Stop();
        playerMain.videoPlayer.prepareCompleted -= OnVideoPrepared;
        InteractableMenu(0);
    }
    public void AfterVideoComplete2()
    {
        playerMain.opacityAnimator.FadeOut(1);
        playerMain.videoPlayer.Stop();
        playerMain.videoPlayer.prepareCompleted -= OnVideoPrepared;
        InteractableMenu(curindex);
    }

    public void InteractableMenu(int index)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].blocksRaycasts = false;
            menus[i].interactable = false;
            menus[i].DOFade(0, 1);
        }
        for (int i = 0; i < menus.Count; i++)
        {
            int j = i;
            if (j != index)
            {
                menus[j].blocksRaycasts = false;
                menus[j].interactable = false;
                menus[j].DOFade(0, 1);
            }
            else
            {
                menus[j].DOFade(1, 1).SetDelay(1).OnComplete(delegate
                {
                    menus[j].blocksRaycasts = true;
                    menus[j].interactable = true;
                });

            }
        }

    }
    public void DisableMenuAll()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            int j = i;
            menus[j].blocksRaycasts = false;
            menus[j].interactable = false;
            menus[j].DOFade(0, .5f);

        }


    }

    public void PengantarCalls(int index)
    {
        int a = index + 2;
        InteractableMenu(a);
        curNiat = curLaranganUmroh = curRukunUmroh = curPerbolehanSaatIhram = -1;
        switch (index)
        {
            case 0:
                Niat();
                pengantarNextMenu[0].gameObject.SetActive(true);
                break;
            case 1:
                RukunUmroh();
                pengantarNextMenu[1].gameObject.SetActive(true);

                break;
            case 2:
                LaranganUmroh();
                pengantarNextMenu[2].gameObject.SetActive(true);

                break;
            case 3:
                PerbolehanSaatIhram();
                pengantarNextMenu[3].gameObject.SetActive(true);

                break;
            case 4:
                Toilet();
                break;
        }
    }




    public void Niat()
    {
        curNiat++;
        if (curNiat >= niatImage.Count - 1)
        {
            curNiat = niatImage.Count - 1;
            pengantarNextMenu[0].gameObject.SetActive(false);

        }
        if (curNiat < 0)
        {
            curNiat = 0;
        }
        for (int i = 0; i < niatImage.Count; i++)
        {
            if (i != curNiat)
            {
                niatImage[i].DOFade(0, .5f);
            }
            else
            {
                niatImage[i].DOFade(1, .5f);
            }
        }
    }
    public void RukunUmroh()
    {
        curRukunUmroh++;
        if (curRukunUmroh >= rukunUmrohImage.Count - 1)
        {
            curRukunUmroh = rukunUmrohImage.Count - 1;
            pengantarNextMenu[1].gameObject.SetActive(false);

        }
        if (curRukunUmroh < 0)
        {
            curRukunUmroh = 0;
        }
        for (int i = 0; i < rukunUmrohImage.Count; i++)
        {
            if (i != curRukunUmroh)
            {
                rukunUmrohImage[i].DOFade(0, .5f);
            }
            else
            {
                rukunUmrohImage[i].DOFade(1, .5f);
            }
        }
    }
    public void LaranganUmroh()
    {
        curLaranganUmroh++;
        if (curLaranganUmroh >= laranganUmrohImage.Count - 1)
        {
            curLaranganUmroh = laranganUmrohImage.Count - 1;
            pengantarNextMenu[2].gameObject.SetActive(false);

        }
        if (curLaranganUmroh < 0)
        {
            curLaranganUmroh = 0;
        }
        for (int i = 0; i < laranganUmrohImage.Count; i++)
        {
            if (i != curLaranganUmroh)
            {
                laranganUmrohImage[i].DOFade(0, .5f);
            }
            else
            {
                laranganUmrohImage[i].DOFade(1, .5f);
            }
        }
    }
    public void PerbolehanSaatIhram()
    {
        curPerbolehanSaatIhram++;
        if (curPerbolehanSaatIhram >= perbolehanSaatIhramImage.Count - 1)
        {
            curPerbolehanSaatIhram = perbolehanSaatIhramImage.Count - 1;
            pengantarNextMenu[3].gameObject.SetActive(false);

        }
        if (curPerbolehanSaatIhram < 0)
        {
            curPerbolehanSaatIhram = 0;
        }
        for (int i = 0; i < perbolehanSaatIhramImage.Count; i++)
        {
            if (i != curPerbolehanSaatIhram)
            {
                laranganUmrohImage[i].DOFade(0, .5f);
            }
            else
            {
                laranganUmrohImage[i].DOFade(1, .5f);
            }
        }
    }

    public void Toilet()
    {
        menusToilet.DOScale(Vector3.zero, 0);
        menusToilet.DOLocalMove(startMenusToilet.localPosition, 0).OnComplete(delegate
        {
            menusToilet.DOScale(new Vector3(0.001529483f, 0.001529483f, 0.001529483f), .5f).SetDelay(2);
            menusToilet.DOLocalMove(endMenusToilet.localPosition, .5f).SetDelay(2).OnComplete(delegate
            {


            });
        });
    }
    public void ChangeScene() {
        SceneChanges sceneChanges = GameObject.FindGameObjectWithTag("Scenes").GetComponent<SceneChanges>();
        sceneChanges.LoadSceneAsync("roombake");


    }


}
[System.Serializable]
public class VideoObjects
{
    public OpacityAnimator opacityAnimator;
    public VideoPlayer videoPlayer;
}
