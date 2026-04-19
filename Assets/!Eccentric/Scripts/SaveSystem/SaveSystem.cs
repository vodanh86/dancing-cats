using System;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public abstract class SaveSystem
    {
        protected const string KEY_SAVE = "save1";
        protected string _keyLocalSave = "saveLocal";
        protected static string _cloudStringData;
        protected bool _isOnlyLocalSave;
        protected DateTime _lastSaveTime;
        protected readonly TimeSpan _timeForSave;
        private readonly string _keySaveTime = "SAVE_TIME";

        protected SaveSystem(SaveType saveType)
        {

#if GAMEDISTRIBUTION
            _isOnlyLocalSave = true;  
#elif LAGGED
            _isOnlyLocalSave = true;  
#elif JIO
            _isOnlyLocalSave = true;  
#else
            _isOnlyLocalSave = saveType == SaveType.OnlyLocal;

            if (GP_Platform.Type() == GamePush.Platform.CRAZY_GAMES)
            {
                _isOnlyLocalSave = false;
                return;
            }

            if (GP_Player.IsLoggedIn())
            {
                _keyLocalSave = GP_Player.GetID().ToString();
                _timeForSave = GP_Variables.Has(_keySaveTime)
                    ? new TimeSpan(0, GP_Variables.GetInt(_keySaveTime), 0)
                    : new TimeSpan(0, 5, 0);
            }
            else
                _isOnlyLocalSave = true;
#endif


        }

        public abstract void Init();
    }
}