using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isRotate2D;
    public float speed;
    public Button motif;
    void Start()
    {
        //Invoke(nameof(Rotate2D), 0);
        // If not assigned in inspector, try to get button on this gameobject
        if (motif != null)
        {
            //motif = GetComponent<Button>();
            motif.interactable = false;
            // Add hover enter event
            EventTrigger trigger = motif.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = motif.gameObject.AddComponent<EventTrigger>();

            // Create hover enter event
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
            trigger.triggers.Add(entryEnter);

            // Create hover exit event
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
            trigger.triggers.Add(entryExit);
            Invoke(nameof(EnableingButoon), 2);
            transform.DOScale(0, 0).SetDelay(0);

        }
    }
    
    public void EnableingButoon() { 
        motif.interactable = true;
    }
    public void EnableingButoon(bool stat)
    {
        motif.interactable = stat;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Invoke(nameof(ForcingRotate2D), 0);
        }
        //if (motif.gameObject.GetComponent<EventTrigger>() == false) { 
            
        //}
    }
    public void Rotate2D() {
        CancelInvoke(nameof(ForcingRotate2D));

        transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -359), speed, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(delegate
        {
            Invoke(nameof(Rotate2D), 0);
        });
        //transform.DOLocalRotateQuaternion(Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y,1), speed).SetEase(Ease.Linear).OnComplete(delegate {
        //    Invoke(nameof(Rotate2D), 0);
        //});

    }
    public void ForcingRotate2D()
    {
        CancelInvoke(nameof(Rotate2D));
        //transform.DOKill();
        transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0), .5f, RotateMode.FastBeyond360).OnComplete(delegate {
            transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -359), .5f, RotateMode.FastBeyond360).OnComplete(delegate {
                //Invoke(nameof(Rotate2D), 0);
            });
        });


    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        Invoke(nameof(Rotate2D), 0);
        transform.DOScale(0.005305198f, 1f);
    }

    // Called when mouse pointer exits the object
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        Invoke(nameof(ForcingRotate2D), 0);
        transform.DOScale(0, 1f).SetDelay(1f);

    }
}
