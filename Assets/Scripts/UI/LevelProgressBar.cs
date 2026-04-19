using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    [SerializeField] private PlayerMovement _player;
    [Space]
    [SerializeField] private LevelProgressBarStar[] _stars;
    [SerializeField] private Color _colorForActivatedStar;
    [Space]
    [SerializeField] private Slider _bar;

    private Finisher _finisher;
    private float _firstStarActivationDistance;
    private float _secondStarActivationPosition;
    private float _thirdStarActivationPosition;

    private void Start()
    {
        _finisher = Finisher.Instance;

        _bar.maxValue = _finisher.GetComponentInParent<BlockFinish>(true).transform.position.z;
        _bar.minValue = _player.transform.position.z;
        _bar.value = _bar.minValue;


        _firstStarActivationDistance = _bar.maxValue / _stars.Length;
        _secondStarActivationPosition = _bar.maxValue / _stars.Length * 2;
        _thirdStarActivationPosition = _bar.maxValue - 1;
    }

    private void Update()
    {
        _bar.value = _player.transform.position.z;

        if (!_stars[0].IsActivated)
            if (_firstStarActivationDistance <= _player.transform.position.z)
                _stars[0].Activate();

        TryToActivateStar(_stars[0], _firstStarActivationDistance);
        TryToActivateStar(_stars[1], _secondStarActivationPosition);
        TryToActivateStar(_stars[2], _thirdStarActivationPosition);
    }

    private void TryToActivateStar(LevelProgressBarStar star, float Distance)
    {
        if (!star.IsActivated)
            if (Distance <= _player.transform.position.z)
                star.Activate();
    }

}
