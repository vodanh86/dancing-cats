using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class SaveSystemPrefs : SaveSystem
    {
        private Dictionary<string, object> _dictionarySave;
        private const string SAVE_ID_KEY = "saveID";
        private int _saveIDValue;

        public SaveSystemPrefs(SaveType saveType) : base(saveType)
        {
        }

        public override void Init()
        {
            Debug.Log("INIT SAVESYSTEM PREFS");
            _dictionarySave = Load();
        }

        public void SetString(string key, string value, bool forcePushOnServer = false) =>
            Save(key, value, forcePushOnServer);


        public void SetInt(string key, int value, bool forcePushOnServer = false) =>
            Save(key, value, forcePushOnServer);


        public void SetFloat(string key, float value, bool forcePushOnServer = false) =>
            Save(key, value, forcePushOnServer);

        public void SetBool(string key, bool value, bool forcePushOnServer = false) =>
            Save(key, value, forcePushOnServer);

        public string GetString(string key, string defaultValue = "")
        {
            if (_dictionarySave.TryGetValue(key, out object value))
                return value.ToString();
            else
            {
                SetString(key, defaultValue);
                return defaultValue;
            }
        }


        public int GetInt(string key, int defaultValue = 0)
        {
            if (_dictionarySave.TryGetValue(key, out object value))
                return Convert.ToInt32(value);
            else
            {
                SetInt(key, defaultValue);
                return defaultValue;
            }
        }


        public float GetFloat(string key, float defaultValue = 0f)
        {
            if (_dictionarySave.TryGetValue(key, out object value))
                return Convert.ToSingle(value);
            else
            {
                SetFloat(key, defaultValue);
                return defaultValue;
            }
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            if (_dictionarySave.TryGetValue(key, out object value))
                return Convert.ToBoolean(value);
            else
            {
                SetBool(key, defaultValue);
                return defaultValue;
            }
        }

        public bool HasKey(string key) => _dictionarySave.ContainsKey(key);

        public void DeleteKey(string key)
        {
            if (HasKey(key))
                _dictionarySave.Remove(key);
        }

        public void DeleteAll()
        {
            _dictionarySave.Clear();
        }


        private Dictionary<string, object> Load()
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
#endif
                _cloudStringData = null;

            if (_cloudStringData is "empty" or "nil" or "null")
                _cloudStringData = null;
            return string.IsNullOrEmpty(_cloudStringData) ? LoadLocal() : LoadData();


            Dictionary<string, object> LoadData()
            {
                var localData = LoadLocal();
                var cloudData = LoadCloud();

                return Convert.ToInt32(localData[SAVE_ID_KEY]) >= Convert.ToInt32(cloudData[SAVE_ID_KEY])
                    ? localData
                    : cloudData;
            }

            Dictionary<string, object> LoadCloud() =>
                JsonConvert.DeserializeObject<Dictionary<string, object>>(_cloudStringData);

            Dictionary<string, object> LoadLocal()
            {
                var stringData = PlayerPrefs.GetString(_keyLocalSave);
                return string.IsNullOrEmpty(stringData)
                    ? new() { { SAVE_ID_KEY, _saveIDValue } }
                    : JsonConvert.DeserializeObject<Dictionary<string, object>>(stringData);
            }
        }

        private void Save(string key, object value, bool forcePushOnServer)
        {
            _saveIDValue = Convert.ToInt32(_dictionarySave[SAVE_ID_KEY]);
            _saveIDValue++;
            _dictionarySave[SAVE_ID_KEY] = _saveIDValue;
            _dictionarySave[key] = value;
            var stringData = JsonConvert.SerializeObject(_dictionarySave);
            PlayerPrefs.SetString(_keyLocalSave, stringData);
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
             
            if (!_isOnlyLocalSave && EccentricInit.Instance.Platform != Platform.CRAZY_GAMES)
            {
                var stringDataCloud = GP_Player.GetString(KEY_SAVE);
                if (!string.IsNullOrEmpty(stringDataCloud))
                {
                    var dataCloud = JsonConvert.DeserializeObject<Dictionary<string, object>>(stringDataCloud);
                    if (Convert.ToInt32(dataCloud[SAVE_ID_KEY]) > _saveIDValue) return;
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
            }
#endif
        }
    }
}

#if GAMEDISTRIBUTION
#elif LAGGED
#elif JIO
#else
#endif