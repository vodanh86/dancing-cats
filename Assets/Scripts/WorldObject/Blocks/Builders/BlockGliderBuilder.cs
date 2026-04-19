using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BlockGliderBuilder : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private bool _isBuilding;
    [SerializeField][Range(1, 30)] private int _blockLength = 1;
    [SerializeField] private GamePointsSphere _spherePrefab;
    [SerializeField] private BlockGliderEnd _end;
    [SerializeField] private List<GamePointsSphere> _sphereList;

    private void Update()
    {
        if (_spherePrefab == null || !_isBuilding)
            return;

        if (_end == null)
            _end = GetComponentInChildren<BlockGliderEnd>();

        foreach (var sphere in _sphereList)
        {
            DestroyImmediate(sphere.gameObject);
        }

        _sphereList.Clear();

        var multiplierindex = 0;

        if (_blockLength > 1)
        {
            multiplierindex = _blockLength - 1;
        }

        _end.transform.position = new Vector3(0, 0, this.transform.position.z) + (Vector3.forward * (_blockLength));

        for (int i = 0; i < multiplierindex; i++)
        {
            var sphere = Instantiate(_spherePrefab, this.transform);
            _sphereList.Add(sphere);
            sphere.transform.position = new Vector3(0, 0.5f, this.transform.position.z) + (Vector3.forward * (i + 1));
        }
    }
#endif
}
