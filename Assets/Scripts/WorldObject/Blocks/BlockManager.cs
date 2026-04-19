using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private MaterialsBook _materialBook;
    [SerializeField] private BlockMeshType _blockMeshType = BlockMeshType.Sandwich;
    [SerializeField][Range(0, 10)] private int _currentMaterialIndex = 0;

    private Block[] _blocks;

    public static BlockManager Instance;

    //public event Action<int> MaterialSwitcherTouched;
    //private SkyboxMaterialChanger _skyboxMaterialChanger;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        _blocks = GetComponentsInChildren<Block>(true);
        IEnumerable<Block> query = from block in _blocks
                                   orderby block.transform.position.z
                                   select block;
        _blocks = query.ToArray();
        //_skyboxMaterialChanger = GetComponent<SkyboxMaterialChanger>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            SetLevelMeshesType();
            SetMaterials();
        }
    }
#endif

    private void Start()
    {
        SetLevelMeshesType();
        SetMaterials();
    }

    public void SetLevelMeshesType()
    {
        if (_blocks == null)
            _blocks = GetComponentsInChildren<Block>(true);
        foreach (Block block in _blocks)
            block.MeshSwitcher?.SetLevelMeshesType(_blockMeshType);
    }

    public void SetMaterial(Material material)
    {
        RenderSettings.skybox = material;
    }

    public Block GetNextBlock(Block currentBlock)
    {
        if (currentBlock.IsLevelMaterialSwitcher)
            SetNextBlocksMaterial();

        int index = Array.IndexOf(_blocks, currentBlock);

        index++;

        index = Mathf.Clamp(index, 0, _blocks.Length - 1);

        return _blocks[index];
    }

    public void SetBlocksIconStatus(bool isActive)
    {
        foreach (Block block in _blocks)
            block.IconSwitcher?.SetIconStatus(isActive);
    }

    public void SetNextBlocksMaterial()
    {
        _currentMaterialIndex++;

        if (_currentMaterialIndex >= _materialBook.MaterialsPairs.Length)
            _currentMaterialIndex = 0;

        SetMaterials();
    }

    private void SetMaterials()
    {
        if (_materialBook)
        {
            if (_currentMaterialIndex >= _materialBook.MaterialsPairs.Length)
            {
                _currentMaterialIndex = _currentMaterialIndex % _materialBook.MaterialsPairs.Length;
            }

            MaterialsPair materialsPair = _materialBook.MaterialsPairs[_currentMaterialIndex];

            if (!Application.isPlaying)
            {
                Block[] blocks = GetComponentsInChildren<Block>();
                ApplyMaterialToBlocks(blocks);
                SetMaterial(materialsPair.SkyMaterial);
            }
            else
            {
                SetMaterial(materialsPair.SkyMaterial);
                ApplyMaterialToBlocks(_blocks);
            }
        }
    }

    private void ApplyMaterialToBlocks(Block[] blocks)
    {
        if (_blockMeshType == BlockMeshType.Summer)
            return;

        foreach (Block block in blocks)
        {
            block.MaterialSwitcher?.SetMaterials(_materialBook.MaterialsPairs[_currentMaterialIndex]);
        }

    }
}
