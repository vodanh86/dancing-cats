using System;
using System.Collections.Generic;
using Eccentric;

[Serializable]
public class PlayerData : SaveData
{
    public int CurrentLevel;
    public List<Progress> Progress;
    public int CurrencyAmount;
    public List<int> SkinsID;
    public List<RewardSkins> RewardSkins;
    public Dictionary<GameEventType, int> GameEventsData;

    public PlayerData()
    {
        SaveID = 0;
        CurrentLevel = 0;
        CurrencyAmount = 0;
        SkinsID = new List<int>(11) { 0 };
        Progress = new List<Progress>(20);
        GameEventsData = new();
    }

    public int GetAllPoints()
    {
        int points = 0;

        foreach (var song in Progress)
            points += song.SongPoints;

        return points;
    }

    public bool IsLevelInList(int songID)
    {
        Progress song = GetSongProgress(songID);

        if (song != null)
            return true;
        else
            return false;
    }

    public void SetSongPointsRecord(Progress progress)
    {
        Progress song = GetSongProgress(progress.SongID);
        song.SongPoints = progress.SongPoints;
    }

    public int GetSongPointsRecord(int songID)
    {
        Progress song = GetSongProgress(songID);

        if (song != null)
            return song.SongPoints;
        else
            return 0;
    }

    public bool IsSkinInList(int skinID)
    {
        RewardSkins skin = GetRewardSkins(skinID);

        if (skin != null)
            return true;
        else
            return false;
    }

    public void SetSkinADWatched(RewardSkins rewardSkins)
    {
        RewardSkins skin = GetRewardSkins(rewardSkins.SkinID);
        skin.RewardWatched = rewardSkins.RewardWatched;
    }

    public int GetSkinADWatched(int skinID)
    {
        RewardSkins skin = GetRewardSkins(skinID);

        if (skin != null)
            return skin.RewardWatched;
        else
            return 0;
    }

    private Progress GetSongProgress(int songID)
    {
        if (Progress == null)
            Progress = new List<Progress>(20);

        foreach (var song in Progress)
            if (song.SongID == songID)
                return song;

        return null;
    }

    private RewardSkins GetRewardSkins(int skinID)
    {
        if (RewardSkins == null)
            RewardSkins = new List<RewardSkins>(20);

        foreach (var skin in RewardSkins)
            if (skin.SkinID == skinID)
                return skin;

        RewardSkins newSkin = new RewardSkins(skinID, 0);
        RewardSkins.Add(newSkin);

        return newSkin;
    }
}

[Serializable]
public class Progress
{
    public int SongID;
    public int SongPoints;

    public Progress(int songID, int songPoint)
    {
        SongID = songID;
        SongPoints = songPoint;
    }
}

[Serializable]
public class RewardSkins
{
    public int SkinID;
    public int RewardWatched;

    public RewardSkins(int skinID, int rewardWatched)
    {
        SkinID = skinID;
        RewardWatched = rewardWatched;
    }
}
