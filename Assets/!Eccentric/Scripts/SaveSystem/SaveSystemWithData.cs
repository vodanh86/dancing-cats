using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Console = System.Console;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class SaveSystemWithData : SaveSystem
    {
        public static PlayerData PlayerData;


        public SaveSystemWithData(SaveType saveType) : base(saveType)
        {
        }

        public override void Init()
        {
            Debug.Log("INIT SAVESYSTEM WITH DATA");

            PlayerData = Load<PlayerData>();
        }

        public T Load<T>() where T : SaveData, new()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (!_isOnlyLocalSave && EccentricInit.Instance.Platform != Platform.CRAZY_GAMES)
            {
                try
                {
                    _cloudStringData = GP_Player.GetString(KEY_SAVE);
                }
                catch (Exception e)
                {
                    Debug.LogError($"CLOUD LOAD ERROR: {e}");
                }
            }
            else
                _cloudStringData = null;

            if (IsValidJson(_cloudStringData) && !_cloudStringData!.Contains("SaveID"))
            {
                var localData = LoadLocal();
                return localData.SaveID > 0 ? localData : LoadCloud();
            }

            return IsValidJson(_cloudStringData) ? LoadData() : LoadLocal();
#else
            return LoadLocal();
#endif


            T LoadData()
            {
                var localData = LoadLocal();
                var cloudData = LoadCloud();

                return localData.SaveID >= cloudData.SaveID ? localData : cloudData;
            }


            T LoadCloud()
            {
                if (IsValidJson(_cloudStringData))
                {
                    return JsonConvert.DeserializeObject<T>(_cloudStringData);
                }

                return new();
            }

            T LoadLocal()
            {
                var localStringData = PlayerPrefs.GetString(_keyLocalSave);
                return IsValidJson(localStringData)
                    ? JsonConvert.DeserializeObject<T>(localStringData)
                    : new();
            }
        }

        public void Save<T>(T data, bool forcePushOnServer = false) where T : SaveData
        {
            data.SaveID++;

            var stringData = JsonConvert.SerializeObject(data);

            if (!_isOnlyLocalSave && EccentricInit.Instance.Platform != Platform.CRAZY_GAMES)
            {
                SaveLocal();
                SaveCloud();
            }
            else
                SaveLocal();


            void SaveLocal()
            {
                PlayerPrefs.SetString(_keyLocalSave, stringData);
            }

            void SaveCloud()
            {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
                var stringDataCloud = GP_Player.GetString(KEY_SAVE);
                if (IsValidJson(stringDataCloud))
                {
                    var dataCloud = JsonConvert.DeserializeObject<T>(stringDataCloud);
                    if (dataCloud.SaveID > data.SaveID) return;
                }

                GP_Player.Set(KEY_SAVE, stringData);
                if (forcePushOnServer)
                {
                    GP_Player.Sync(true);
                }
                else
                {
                    var currentTime = GP_Server.Time();
                    if (currentTime.Subtract(_lastSaveTime) >= _timeForSave)
                    {
                        _lastSaveTime = currentTime;
                        GP_Player.Sync();
                    }
                }
#endif
            }
        }

        private bool IsValidJson(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                Debug.LogWarning("IsValidJson: false - IsNullOrWhiteSpace");
                return false;
            }

            jsonString = jsonString.Trim();


            if ((jsonString.StartsWith("{") && jsonString.EndsWith("}")) ||
                (jsonString.StartsWith("[") && jsonString.EndsWith("]")))
            {
                try
                {
                    var token = JToken.Parse(jsonString);

                    if (token.Type == JTokenType.Object && !token.HasValues)
                    {
                        Debug.LogWarning("IsValidJson: false - json has no value or object");
                        return false;
                    }
                    Debug.LogWarning("IsValidJson: true");
                    return true;
                }
                catch (JsonReaderException)
                {
                    Debug.LogWarning("IsValidJson: false - json cannot parse");
                    return false;
                }
            }
            Debug.LogWarning("IsValidJson: false - json doesn't starts or ends with { or [ symbols");
            return false;
        }
    }

    public class SaveData
    {
        public int SaveID;


        public SaveData()
        {
            SaveID = 0;
        }
    }
}