using UnityEngine;

[ExecuteAlways]
public class BlockSpinerBuilder : MonoBehaviour
{
#if UNITY_EDITOR
    //[SerializeField] private bool _isBuilding;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private BlockSpiner _blockSpiner;
    [SerializeField] private BlockSpinerBuilderBone _bone;

    private int _length = 2;
    private const float _boneOffset = 0.4f;
    private const float _boneStepDistance = 1;
    private const float _colliderCenterOffset = 0.5f;
    private const float _colliderSizeOffset = 1f;
    private Vector3 _baseColliderCentre = new Vector3(0, -0.1f, 0);
    private Vector3 _baseColliderSize = new Vector3(1f, 0.15f, 1f);
    private Vector3 _baseBonePosition = new Vector3(0, 0, 0.4f);

    public void UpdateSize()
    {
        if (_blockSpiner == null)
            _blockSpiner = GetComponent<BlockSpiner>();

        if (_collider == null)
            _collider = GetComponent<BoxCollider>();

        if (_bone == null)
            _bone = GetComponentInChildren<BlockSpinerBuilderBone>();

        _length = _blockSpiner.BlockLength;

        var centreOffset = Vector3.zero;
        var sizeOffset = Vector3.zero;
        var multiplierindex = 0;
        var boneNewPosition = _baseBonePosition;

        if (_length > 1)
        {
            multiplierindex = _length - 1;

            centreOffset = new Vector3(0, 0, _colliderCenterOffset * multiplierindex);
            sizeOffset = new Vector3(0, 0, _colliderSizeOffset * multiplierindex);

            boneNewPosition = new Vector3(0, 0, _boneOffset + (_boneStepDistance * multiplierindex));
        }

        _collider.center = _baseColliderCentre + centreOffset;
        _collider.size = _baseColliderSize + sizeOffset;

        _bone.transform.localPosition = boneNewPosition;
    }

#endif
}
