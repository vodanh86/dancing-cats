using System;
using Eccentric;

public class GameEventData
{
    private int _timeSpent;
    private GameEventType _eventType;

    public bool IsTimeLeft => _timeSpent >= TargetTime;
    public readonly int TargetTime;

    public bool GotSkin => _timeSpent == -1;

    public int TimeLeftMinutes
    {
        get
        {
            int value = (TargetTime - _timeSpent) / 60;
            if ((TargetTime - _timeSpent) % 60 != 0)
                value++;

            return Math.Clamp(value, 0, int.MaxValue);
        }
    }

    public GameEventData(int targetTime, GameEventType eventType)
    {
        _timeSpent = 0;
        _eventType = eventType;
        TargetTime = targetTime;
    }

    public void Save()
    {
        SaveSystemWithData.PlayerData.GameEventsData[_eventType] = _timeSpent;
        EccentricInit.Instance.SaveSystemWithData.Save(SaveSystemWithData.PlayerData);
    }

    public void Load()
    {
        if (SaveSystemWithData.PlayerData.GameEventsData.ContainsKey(_eventType))
        {
            _timeSpent = SaveSystemWithData.PlayerData.GameEventsData[_eventType];
        }
        else
        {
            Save();
        }
    }

    public void AddTime(int value)
    {
        _timeSpent += value;
        Save();
    }

    public void GetSkin()
    {
        _timeSpent = -1;
        Save();
    }
}
