using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenesBook", menuName = "ScriptableObjects/ScenesBook")]
public class ScenesBook : ScriptableObject
{
    [SerializeField] private SceneData[] _sceneData;

    public SceneData[] SceneData => _sceneData;

    public SceneData GetSceneData(string sceneName)
    {
        foreach (SceneData sceneData in _sceneData)
            if (sceneData.Name == sceneName)
                return sceneData;

        return null;
    }
}

[Serializable]
public class SceneData
{
    [SerializeField] private string _name;
    [SerializeField] private int _id;
    [SerializeField] private Sprite _songIcon;
    [SerializeField] private AudioKeys _songKey;
    [SerializeField] private int _currentBPM;
    [SerializeField] private bool _isEventSong;

    public string Name => _name;
    public int Id => _id;
    public Sprite SongIcon => _songIcon;
    public AudioKeys SongKey => _songKey;
    public bool IsEventSong => _isEventSong;
}
