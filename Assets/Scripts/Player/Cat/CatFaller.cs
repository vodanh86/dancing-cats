using UnityEngine;
using DG.Tweening;

public class CatFaller : MonoBehaviour
{
    public void Fall()
    {
        transform.DOLocalMoveY(-7f, 2f).SetEase(Ease.Linear);
    }
}
