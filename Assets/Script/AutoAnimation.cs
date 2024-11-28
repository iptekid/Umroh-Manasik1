using UnityEngine;
using DG.Tweening;

public class AutoAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isRotate2D;
    public float speed;
    void Start()
    {
            Invoke(nameof(Rotate2D), 0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            Invoke(nameof(ForcingRotate2D), 0);

        }
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
        transform.DOKill();
        transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0), .5f, RotateMode.FastBeyond360).OnComplete(delegate {
            transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -359), 2, RotateMode.FastBeyond360).OnComplete(delegate {
                Invoke(nameof(Rotate2D), 0);
            });
        });


    }
}
