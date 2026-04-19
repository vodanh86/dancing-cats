using DG.Tweening;
using UnityEngine;

public abstract class Block : WorldObject
{
    [SerializeField] private bool _isLevelMaterialSwitcher = false;
    [SerializeField] private BlockMeshSwitcher _meshSwitcher;
    [SerializeField] private BlockMaterialSwitcher _materialSwitcher;
    [SerializeField] private BlockIconSwitcher _iconSwitcher;

    private BlockType _type;

    protected abstract BlockType InitType();

    public bool IsLevelMaterialSwitcher => _isLevelMaterialSwitcher;
    public BlockType Type => _type;
    public BlockIconSwitcher IconSwitcher => _iconSwitcher;
    public BlockMaterialSwitcher MaterialSwitcher => _materialSwitcher;
    public BlockMeshSwitcher MeshSwitcher => _meshSwitcher;

    protected virtual void Awake()
    {
        _type = InitType();

        if (_iconSwitcher == null)
            _iconSwitcher = GetComponent<BlockIconSwitcher>();

        if (_meshSwitcher == null)
            _meshSwitcher = GetComponent<BlockMeshSwitcher>();

        if (_materialSwitcher == null)
            _materialSwitcher = GetComponent<BlockMaterialSwitcher>();
    }

    public void Fall()
    {
        StopMove();
        this.transform.DOKill();
        _iconSwitcher.HideIcon();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(0.2f, 0.2f).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMoveY(-1f, 0.5f).SetEase(Ease.Linear));
    }

    private void OnDrawGizmos()
    {
        if (_isLevelMaterialSwitcher)
        {
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawSphere(new Vector3(3f, 0f, transform.position.z), 0.4f);
            Gizmos.DrawSphere(new Vector3(-3f, 0f, transform.position.z), 0.4f);
        }
    }

}

public enum BlockType
{
    Normal,
    Glide,
    Spin,
    Finish
}
