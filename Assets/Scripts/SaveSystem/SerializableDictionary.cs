using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [field: Preserve]
    [SerializeField] private List<TKey> _keys = new List<TKey>();
    [field: Preserve]
    [SerializeField] private List<TValue> _values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < _keys.Count; i++)
        {
            this.Add(_keys[i], _values[i]);
        }
    }
}
