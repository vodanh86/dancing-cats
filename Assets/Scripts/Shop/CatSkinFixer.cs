using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSkinFixer : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _baseSkin;
    [SerializeField] private SkinnedMeshRenderer _targetSkin;

    private void Start()
    {
        for (int i = 0; i < _baseSkin.bones.Length; i++)
        {
            _targetSkin.bones[i] = _baseSkin.bones[i];
        }
    }
}
