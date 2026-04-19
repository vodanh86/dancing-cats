using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialsBook", menuName = "ScriptableObjects/MaterialsBook")]
public class MaterialsBook : ScriptableObject
{
    //[SerializeField] private Material _baseSkyBox;
    [SerializeField] private MaterialsPair[] _materialsPair;

    //public Material BaseSkyBox => _baseSkyBox;
    public MaterialsPair[] MaterialsPairs => _materialsPair;

}

[Serializable]
public class MaterialsPair
{
    [SerializeField] private Material _firstBlockMaterial;
    [SerializeField] private Material _secondBlockMaterial;
    [SerializeField] private Material _skyMaterial;
    [SerializeField] private Material _rainbowMaterial;


    public Material FirstBlockMaterial => _firstBlockMaterial;
    public Material SecondBlockMaterial => _secondBlockMaterial;
    public Material SkyMaterial => _skyMaterial;
    public Material RainbowMaterial => _rainbowMaterial;
}
