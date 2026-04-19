using System.Collections.Generic;
using UnityEngine;

public class SongSelectionScreen : MonoBehaviour
{
    [SerializeField] private ScenesBook _scenesBook;
    [SerializeField] private SongPreview _songPreviewPrefab;

    private List<SongPreview> _previews = new List<SongPreview>();

    private void Awake()
    {
        for (int i = 0; i < _scenesBook.SceneData.Length; i++)
        {
            SongPreview preview = Instantiate(_songPreviewPrefab, _songPreviewPrefab.transform.parent);
            preview.gameObject.SetActive(true);
            _previews.Add(preview);
            preview.SceneData = _scenesBook.SceneData[i];
        }
    }

    private void OnEnable()
    {
        foreach (SongPreview preview in _previews)
            preview.PlayButtonClick += OnPlayButtonClick;
    }

    private void OnDisable()
    {
        foreach (SongPreview preview in _previews)
            preview.PlayButtonClick -= OnPlayButtonClick;
    }

    public void СheckSongForAvailability()
    {
        foreach (var preview in _previews)
            if (GameProgressHolder.Instance.CheckSongForAvailability(preview.SceneData))
                preview.SetUnlockedStatus(GameProgressHolder.Instance.GetErnedPoints(preview.SceneData));
    }

    private void OnPlayButtonClick(SceneData data)
    {
        LevelLoader.Instance.LoadLevel(data);
    }
}
