using UnityEngine;
using DG.Tweening;

public class Coin : WorldObject
{
    [SerializeField] private int _value;

    private Vector3 _rotation = new Vector3(0, 360, 0);
    private float _rotationTime = 2f;

    public int Value => _value;

    private void Awake()
    {
        transform.DORotate(_rotation, _rotationTime, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }

    public override void Show()
    {
        base.Show();
    }

    public void DestroyItself()
    {
        SongAudioSource.Instance.PlayOneShot(AudioKeys.Coin.ToString());
        transform.DOKill();

        TweenCallback tweenCallback = null;

        tweenCallback = () => Destroy(this.gameObject, 0.3f);

        transform.DOScale(0, 0.25f).OnComplete(tweenCallback);
    }
}
