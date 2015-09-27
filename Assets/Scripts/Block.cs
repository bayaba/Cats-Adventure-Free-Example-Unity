using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Block : MonoBehaviour
{
    bool isCat = false;
    Vector3 resetPos;

    public float FallDelay = 0.5f;

    void Start()
    {
        // Stone block's FallDelay is -1f
        // Normal block's FallDelay is higher than 0f
        if (FallDelay >= 0f) transform.DOLocalMoveY(-0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "MyCat") // The cat on the block
        {
            if (!isCat) // one time for check
            {
                if (FallDelay >= 0f) // if normal block
                {
                    Invoke("FallBlock", FallDelay); // block falls into the sea FallDelay later
                }
                isCat = true;
            }
        }
    }

    void FallBlock()
    {
        CancelInvoke("FallBlock");
        transform.DOLocalMoveY(-3f, 0.5f); // DOTween PlugIN
        Destroy(gameObject, 0.5f);
    }
}
