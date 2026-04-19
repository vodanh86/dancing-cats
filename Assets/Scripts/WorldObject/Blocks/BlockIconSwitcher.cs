using UnityEngine;
using DG.Tweening;

public class BlockIconSwitcher : MonoBehaviour
{
    [SerializeField] private BlockIconType _iconType = BlockIconType.Point;
    [Space]
    [SerializeField] private Renderer _pointMesh;
    [SerializeField] private Renderer _triangleMesh;
    [SerializeField] private Renderer _imprintMesh;

    private Renderer _currentMesh;

    private void Awake()
    {
        switch (_iconType)
        {
            case BlockIconType.Point:
                _currentMesh = _pointMesh;
                _triangleMesh.gameObject.SetActive(false);
                _imprintMesh.gameObject.SetActive(false);
                break;
            case BlockIconType.Triangle:
                _currentMesh = _triangleMesh;
                _pointMesh.gameObject.SetActive(false);
                _imprintMesh.gameObject.SetActive(false);
                break;
            case BlockIconType.Imprint:
                _currentMesh = _imprintMesh;
                _pointMesh.gameObject.SetActive(false);
                _triangleMesh.gameObject.SetActive(false);
                break;
        }
    }

    public void SetIconStatus(bool isActive)
    {
        _currentMesh.gameObject.SetActive(isActive);
    }

    public void HideIcon()
    {
        _currentMesh.material.DOFade(0f, 0.7f);
        _currentMesh.transform.DOLocalMoveY(1f, 0.7f);
    }
}

public enum BlockIconType
{
    Point,
    Triangle,
    Imprint
}
