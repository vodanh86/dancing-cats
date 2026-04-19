using UnityEngine;

public enum BlockMeshType
{
    Cloud,
    Sandwich,
    Button,
    Disk,
    Summer
}

public class BlockMeshSwitcher : MonoBehaviour
{
    [SerializeField] private BlockMaterialSwitcher _blockMaterialSwitcher;
    [SerializeField] private Renderer _sandwichMesh;
    [SerializeField] private Renderer _cloudMesh;
    [SerializeField] private Renderer _buttonMesh;
    [SerializeField] private Renderer _diskMesh;
    [SerializeField] private Renderer _summerMesh;

    private Renderer _currentMesh;
    private BlockMeshType _blockMeshType;

    private void Awake()
    {
        if (_blockMaterialSwitcher == null)
            _blockMaterialSwitcher = GetComponent<BlockMaterialSwitcher>();
    }

    public void SetLevelMeshesType(BlockMeshType type)
    {
        _blockMeshType = type;

        switch (type)
        {
            case BlockMeshType.Sandwich:
                _currentMesh = _sandwichMesh;
                break;
            case BlockMeshType.Disk:
                _currentMesh = _diskMesh;
                break;
            case BlockMeshType.Button:
                _currentMesh = _buttonMesh;
                break;
            case BlockMeshType.Cloud:
                _currentMesh = _cloudMesh;
                break;
            case BlockMeshType.Summer:
                _currentMesh = _summerMesh;
                break;
        }

        SetLevelMeshes();
    }

    private void SetLevelMeshes()
    {
        foreach (var mesh in new Renderer[5] { _sandwichMesh, _cloudMesh, _buttonMesh, _diskMesh, _summerMesh })
            mesh.gameObject.SetActive(false);

        _currentMesh.gameObject.SetActive(true);
    }
}

