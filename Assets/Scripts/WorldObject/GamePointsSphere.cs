using UnityEngine;
using DG.Tweening;

public class GamePointsSphere : WorldObject
{
    public void DestroyItself()
    {
        TweenCallback tweenCallback = null;

        tweenCallback = () => Destroy(this.gameObject);

        transform.DOScale(0, 0.2f).OnComplete(tweenCallback);
    }
}
