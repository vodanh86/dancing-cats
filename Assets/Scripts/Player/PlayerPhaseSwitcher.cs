using Eccentric;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhaseSwitcher : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Button _wingsButton;
    [SerializeField] private Button _refuseButton;

    private BlockManager _blockManager;
    private CatJumper _catJumper;
    private CatGlider _catGlider;
    private CatSpiner _catSpiner;
    private CatWings _catWings;
    private CatFinisher _catFinisher;
    private CatFaller _catFaller;
    private PlayerGameStoper _gameStoper;
    private Phase _currentPhase;
    private Block _currentBlock;
    private Block _nextBlock;
    private bool _isSearchingNewBlock = false;

    public Phase CurrentPhace => _currentPhase;

    public event Action<Vector3, Vector3> BlockHited;

    private void Awake()
    {
        _gameStoper = GetComponent<PlayerGameStoper>();
        _catJumper = GetComponentInChildren<CatJumper>();
        _catGlider = GetComponentInChildren<CatGlider>();
        _catSpiner = GetComponentInChildren<CatSpiner>();
        _catFinisher = GetComponentInChildren<CatFinisher>();
        _catFaller = GetComponentInChildren<CatFaller>();
        _catWings = GetComponentInChildren<CatWings>();
    }

    private void OnEnable()
    {
        _wingsButton.onClick.AddListener(TryWatchAD);
        _refuseButton.onClick.AddListener(SetLostPhase);

    }

    private void OnDisable()
    {
        _wingsButton.onClick.RemoveListener(TryWatchAD);
        _refuseButton.onClick.RemoveListener(SetLostPhase);
    }

    private void Start()
    {
        _blockManager = BlockManager.Instance;
        _currentPhase = Phase.Idle;
        EnterPhase();
    }

    private void Update()
    {
        if (_isSearchingNewBlock)
        {
            TrySearchBlock();
        }
    }

    public void StartSearchingBlock()
    {
        _isSearchingNewBlock = true;
        TrySearchBlock();
    }

    private void SetLostPhase()
    {
        EccentricInit.Instance.AdManager.ShowAd();
        ChangePhase(Phase.Lost);
    }

    private void TryWatchAD()
    {
        EccentricInit.Instance.AdManager.ShowRewardAd(ActivateInvincibility);
    }

    private void ActivateInvincibility()
    {
        _gameStoper.ContinueGame(() =>
        {
            _catWings.StartToFly();

            StartSearchingBlock();
        });
    }

    private void TrySearchBlock()
    {
        _currentBlock = null;

        var targets = Physics.SphereCastAll(transform.position, 0.3f, Vector3.down, 2, _layerMask);

        if (targets.Length == 0)
        {
            if (_catWings.IsFlying)
                return;

            if (_gameStoper.IsHaveLives)
            {
                _gameStoper.UpdateWingView();
                _catWings.StartToFly();
                return;
            }
            else
            {
                ChangePhase(Phase.Pause);
            }

        }
        else
        {
            foreach (var target in targets)
            {
                if (target.collider.TryGetComponent(out _currentBlock))
                {
                    _isSearchingNewBlock = false;

                    if (this.transform.position != Vector3.zero)
                        BlockHited?.Invoke(this.transform.position, _currentBlock.transform.position);

                    SetNewPhase(_currentBlock);
                    break;
                }
            }
        }
    }

    private void SetNewPhase(Block block)
    {
        switch (block.Type)
        {
            case BlockType.Normal:
                ChangePhase(Phase.Jump);
                break;
            case BlockType.Glide:
                ChangePhase(Phase.Glide);
                break;
            case BlockType.Spin:
                ChangePhase(Phase.Spin);
                break;
            case BlockType.Finish:
                ChangePhase(Phase.Finish);
                break;
        }
    }

    public void ChangePhase(Phase phace)
    {
        ExitPhase();
        _currentPhase = phace;
        EnterPhase();
    }

    private void EnterPhase()
    {
        switch (_currentPhase)
        {
            case Phase.Idle:
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Phase.Jump:
                _nextBlock = _blockManager.GetNextBlock(_currentBlock);
                _catJumper.Jump(StartSearchingBlock, _currentBlock, _nextBlock);
                break;
            case Phase.Spin:
                _nextBlock = _blockManager.GetNextBlock(_currentBlock);
                _catSpiner.Spine(StartSearchingBlock, _currentBlock, _nextBlock);
                break;
            case Phase.Glide:
                _nextBlock = _blockManager.GetNextBlock(_currentBlock);
                _catGlider.Glide(StartSearchingBlock, _currentBlock, _nextBlock);
                break;
            case Phase.Finish:
                _catFinisher.Finish();
                this.enabled = false;
                break;
            case Phase.Pause:
                _gameStoper.PauseGame();
                _isSearchingNewBlock = false;
                break;
            case Phase.Lost:
                _catFaller.Fall();
                _gameStoper.StopGame();
                this.enabled = false;
                break;
        }
    }

    private void ExitPhase()
    {
        switch (_currentPhase)
        {
            case Phase.Idle:
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                _gameStoper.StartGame();
                break;
            default:
                break;
        }
    }
}

public enum Phase
{
    Idle,
    Jump,
    Spin,
    Glide,
    Finish,
    Lost,
    Pause,
    Immortal
}