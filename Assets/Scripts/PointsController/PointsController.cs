using UnityEngine;

public class PointsController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private Points _pointsWallet;
    [SerializeField] private WorldObjectCollector _worldObjectCollector;
    [SerializeField] private PlayerPhaseSwitcher _phaseSwitcher;

    private PointsControllerView _view;
    private float _offsetForPerfect = 0.15f;
    private int _currentMultiplier = 0;
    private int _rewardPerHit = 1;
    private int _currentReward = 0;

    private void Awake()
    {
        _view = GetComponent<PointsControllerView>();
    }

    private void OnEnable()
    {
        _worldObjectCollector.PointsSphereCollected += OnPointsSphereCollected;
        _phaseSwitcher.BlockHited += OnBlockHited;
    }

    private void OnDisable()
    {
        _worldObjectCollector.PointsSphereCollected -= OnPointsSphereCollected;
        _phaseSwitcher.BlockHited -= OnBlockHited;
    }

    private void OnBlockHited(Vector3 playerPosition, Vector3 blockCenter)
    {
        var offset = Mathf.Abs(blockCenter.x - playerPosition.x);

        if (offset <= _offsetForPerfect)
            AccruePoints(HitType.Perfect);
        else
            AccruePoints(HitType.Good);

        _hitEffect.transform.position = new Vector3(blockCenter.x, 0.2f, playerPosition.z);

        _hitEffect.Play();
    }

    private void OnPointsSphereCollected()
    {
        AccruePoints(HitType.Perfect);
    }

    private void AccruePoints(HitType type)
    {
        switch (type)
        {
            case HitType.Perfect:
                _currentMultiplier++;
                _view.SetStatus(true, _currentMultiplier);
                break;
            case HitType.Good:
                _currentMultiplier = 1;
                _view.SetStatus(false, _currentMultiplier);
                break;
        }

        _currentReward = _currentMultiplier * _rewardPerHit;
        _pointsWallet.Add(_currentReward);
    }
}

public enum HitType
{
    Good,
    Perfect
}
