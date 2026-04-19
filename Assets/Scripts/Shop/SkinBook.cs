using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinBook", menuName = "ScriptableObjects/SkinBook")]
public class SkinBook : ScriptableObject
{
    [SerializeField] private SkinData[] _data;

    public SkinData GetSkinData(int id)
    {
        foreach (SkinData data in _data)
            if (data.Id == id)
                return data;
        return null;
    }
}

[Serializable]
public class SkinData
{
    [SerializeField] private int _id;
    [SerializeField] private int _cost;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _isInApp;
    [SerializeField] private bool _isForReward;
    [SerializeField] private int _adsWatchedToUnlock = 3;
    [SerializeField] private bool _isEventSkin;
    [SerializeField] private string _nameID;

    public int Id => _id;
    public int Cost => _cost;
    public Sprite Icon => _icon;
    public bool IsInApp => _isInApp;
    public bool IsForReward => _isForReward;
    public int AddsWatchedToUnlock => _adsWatchedToUnlock;
    public bool IsEventSkin => _isEventSkin;
    public string NameID => _nameID;
}
