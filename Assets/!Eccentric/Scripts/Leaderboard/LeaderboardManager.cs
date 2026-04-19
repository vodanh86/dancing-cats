using System;
using System.Collections.Generic;
using UnityEngine;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class LeaderboardManager
    {
        private static int _playerID;
        private readonly int _limitFetch = 8;
        private readonly int _limitPlayerName = 14;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private List<LeaderboardData> _players = new();
#endif
        private void Start()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GetPlayerID();
#endif
        }
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private void GetPlayerID() => _playerID = GP_Player.GetID();

        private void Fetch()
        {
#if !UNITY_EDITOR
        GP_Leaderboard.Fetch(limit: _limitFetch, showNearest: 0, withMe: WithMe.last,includeFields:"score");
#endif
        }
#endif


        public void Subscribe()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_Leaderboard.OnFetchSuccess += OnFetchSuccessHandler;
#endif
        }

        public void Unsubscribe()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_Leaderboard.OnFetchSuccess -= OnFetchSuccessHandler;
#endif
        }

        public void ShowLeaderboard()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            Fetch();
#endif
        }
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private void OnFetchSuccessHandler(string fetchTag, GP_Data data)
        {
            Debug.LogWarning("OnFetchSuccessHandler");
            _players = data.GetList<LeaderboardData>();
            EccentricJS.ECC_ShowLeaderboard();
            SetDataInItems(_players);
        }

        private void SetDataInItems(List<LeaderboardData> players)
        {
            if (players.Count == 0)
            {
                Debug.LogWarning("players empty");
                return;
            }

            if (players.Count > _limitFetch)
                players.RemoveAt(players.Count-2);
            
            
            for (int i = 0; i < _limitFetch; i++)
            {
                var namePlayer = players[i].name;
                var avatar = players[i].avatar;
                if (string.IsNullOrWhiteSpace(namePlayer))
                {
                    namePlayer = EccentricInit.Instance.LocalisationManager.GetText("hidden");
                    avatar = "EccentricData/Icons/icon_player.png";
                }

                if (namePlayer.Length > _limitPlayerName)
                {
                    namePlayer = namePlayer.Remove(_limitPlayerName);
                    namePlayer += "...";
                }
                EccentricJS.ECC_SetLeaderboardData(i, players[i].position, avatar, namePlayer,
                    players[i].score, players[i].id == _playerID);
            }
        }
#endif


        public void NewScore(float score, TypeRecordData typeRecordData)
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (typeRecordData == TypeRecordData.Set) GP_Player.SetScore(score);
            else GP_Player.AddScore(score);
#endif
        }

        public enum TypeRecordData
        {
            Add,
            Set,
        }
    }

    [Serializable]
    public class LeaderboardData
    {
        public int id;
        public int score;
        public string name;
        public int position;
        public string avatar;


        public LeaderboardData(int m_id, int m_score, string m_name, int m_position, string m_avatar)
        {
            id = m_id;
            score = m_score;
            name = m_name;
            position = m_position;
            avatar = m_avatar;
        }
    }
}