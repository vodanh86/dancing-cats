using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockMaterialSwitcher : MonoBehaviour
{
    [SerializeField] private Renderer[] _mainRenderers;
    [SerializeField] private Renderer _subRenderer;
    //private BlockMeshType _type;

    //public void SetStartData(Renderer currentRenderer, BlockMeshType type)
    //{
    //    _mainRenderer = currentRenderer;
    //    _type = type;

    //    List<Renderer> renderers = new List<Renderer>(_mainRenderer.gameObject.GetComponentsInChildren<Renderer>());

    //    if (renderers.Count > 1)
    //    {
    //        renderers.Remove(_mainRenderer);
    //        _subRenderer = renderers.ToArray();
    //    }
    //}

    public void SetMaterials(MaterialsPair materials)
    {
        //Debug.Log("SetMaterials");
        //var newMaterialPair = new Material[2] { materials.FirstBlockMaterial, materials.SecondBlockMaterial };
        for (int i = 0; i < _mainRenderers.Length; i++)
        {
            var materialsToSet = new List<Material>();
            if (_mainRenderers[i].sharedMaterials.Length == 1)
            {
                materialsToSet.Add(materials.FirstBlockMaterial);
            }
            else {
                materialsToSet.Add(materials.FirstBlockMaterial);
                materialsToSet.Add(materials.SecondBlockMaterial);
            }
            _mainRenderers[i].sharedMaterials = materialsToSet.ToArray();

            //_mainRenderers[i].materials = newMaterialPair;
        }
        //_mainRenderer.materials = newMaterialPair;
        if (_subRenderer)
        {
            Material[] subMaterials = new Material[_subRenderer.sharedMaterials.Length];
            for (int i = 0; i < _subRenderer.sharedMaterials.Length; i++)
            {
                subMaterials[i] = materials.RainbowMaterial;
            }
            _subRenderer.sharedMaterials = subMaterials;
        }
        

        //if (_type != BlockMeshType.Cloud)
        //{

        //}
        //else
        //{
        //    if (_subRenderer != null)
        //    {
        //        Material[] rainbowMaterials = new Material[4];
        //        for (int i = 0; i < rainbowMaterials.Length; i++) {
        //            rainbowMaterials[i] = materials.RainbowMaterial;
        //        }
        //        _mainRenderer.materials = rainbowMaterials;

        //        foreach (var renderer in _subRenderer)
        //            renderer.material = materials.FirstBlockMaterial;
        //    }
        //    else
        //    {
        //        _mainRenderer.material = materials.FirstBlockMaterial;
        //    }
        //}
    }
}