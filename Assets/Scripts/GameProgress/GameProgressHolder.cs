using UnityEngine;
using UnityEngine.SceneManagement;
using Eccentric;
using GamePush;

public class GameProgressHolder : MonoBehaviour
{
    public static GameProgressHolder Instance { get; private set; }

    [SerializeField] private ScenesBook _scenesBook;
    private SceneData _sceneData;
    public SceneData CurrentSceneData => _sceneData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateCurrentSceneData()
    {
        _sceneData = _scenesBook.GetSceneData(SceneManager.GetActiveScene().name);
    }

    public void OpenLevel()
    {
        if (!SaveSystemWithData.PlayerData.IsLevelInList(_sceneData.Id))
        {
            SaveSystemWithData.PlayerData.Progress.Add(new Progress(_sceneData.Id, 0));
            EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData);
        }
    }

    public void OpenLevel(int levelIndex)
    {
        if (!SaveSystemWithData.PlayerData.IsLevelInList(levelIndex))
        {
            SaveSystemWithData.PlayerData.Progress.Add(new Progress(levelIndex, 0));
            EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData);
        }
    }

    public void UpdateProgress(int newPoinstAmount)
    {
        var savedPoints = SaveSystemWithData.PlayerData.GetSongPointsRecord(_sceneData.Id);

        if (savedPoints == 0)
        {
            SaveSystemWithData.PlayerData.CurrentLevel++;
            GP_Analytics.Goal("LEVEL_COMPLETED_FIRST", _sceneData.Id + 1);
        }
        else
        {
            GP_Analytics.Goal("LEVEL_COMPLETED_ADDITIONAL", _sceneData.Id + 1);
        }

        if (newPoinstAmount > savedPoints)
        {
            SaveSystemWithData.PlayerData.SetSongPointsRecord(new Progress(_sceneData.Id, newPoinstAmount));

            var points = SaveSystemWithData.PlayerData.GetAllPoints();

            EccentricInit.Instance.LeaderboardManager.NewScore(points, LeaderboardManager.TypeRecordData.Set);
        }

        EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData);
    }

    public void SendLostEvent()
    {
        GP_Analytics.Goal("LEVEL_LOST", _sceneData.Id + 1);
    }

    public void SendLevelStartedEvent()
    {
        var savedPoints = SaveSystemWithData.PlayerData.GetSongPointsRecord(_sceneData.Id);

        if (savedPoints == 0)
        {
            GP_Analytics.Goal("LEVEL_STARTED_BEFORE_WIN", _sceneData.Id + 1);
        }
        else
        {
            GP_Analytics.Goal("LEVEL_STARTED_AFTER_WIN", _sceneData.Id + 1);
        }
    }

    public SceneData GetNextSceneData()
    {
        for (int i = 0; i < _scenesBook.SceneData.Length; i++)
        {
            if (SaveSystemWithData.PlayerData.IsLevelInList(_scenesBook.SceneData[i].Id))
            {
                if (SaveSystemWithData.PlayerData.GetSongPointsRecord(_scenesBook.SceneData[i].Id) == 0)
                    return _scenesBook.SceneData[i];
            }
            else
            {
                return _scenesBook.SceneData[i];
            }
        }

        //for (int i = 0; i < SaveSystemWithData.PlayerData.Progress.Count; i++)
        //{
        //    if (SaveSystemWithData.PlayerData.Progress[i].SongPoints == 0)
        //        return _scenesBook.SceneData[SaveSystemWithData.PlayerData.Progress[i].SongID];
        //}

        return _scenesBook.SceneData[0];
    }

    public bool CheckSongForAvailability(SceneData data)
    {
        if (SaveSystemWithData.PlayerData.IsLevelInList(data.Id))
            return true;
        else
            return false;
    }

    public int GetCurrentSceneScore()
    {
        return GetErnedPoints(_sceneData);
    }

    public int GetErnedPoints(SceneData data)
    {
        if (SaveSystemWithData.PlayerData.IsLevelInList(data.Id))
            return SaveSystemWithData.PlayerData.GetSongPointsRecord(data.Id);
        else
            return 0;
    }
}