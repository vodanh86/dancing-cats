using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldObjectShower : MonoBehaviour
{
    [SerializeField] private WorldObject[] _objects;
    [SerializeField] private float _showDistance;

    private Camera _camera;
    private int _currentBlock = 3;
    private float _currentDistance;

    private void Awake()
    {
        _objects = GetComponentsInChildren<WorldObject>();
    }

    private void Start()
    {
        IEnumerable<WorldObject> query = from worldObject in _objects
                                         orderby worldObject.transform.position.z
                                         select worldObject;

        _objects = query.ToArray();

        _camera = Camera.main;

        _currentBlock = GetNumberOfVisiableObjects();

        for (int i = _currentBlock; i < _objects.Length; i++)
        {
            _objects[i].Hide();
            _objects[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _currentDistance = _objects[_currentBlock].transform.position.z - _camera.transform.position.z;

        if (_currentDistance < _showDistance)
        {
            _objects[_currentBlock].gameObject.SetActive(true);
            _objects[_currentBlock].Show();
            _currentBlock++;
        }

        if (_currentBlock > _objects.Length - 1)
            this.enabled = false;
    }

    private int GetNumberOfVisiableObjects()
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            if ((_objects[i].transform.position.z - _camera.transform.position.z) > _showDistance)
            {
                return i;
            }
            else
            {
                _objects[i].StartMove();
            }
        }

        return 1;
    }
}
