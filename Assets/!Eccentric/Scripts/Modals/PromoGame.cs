using System;
using UnityEngine;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class PromoGame
    {
        private bool _isDataInstalled;
        private PromoGameData _promoGameData;
        public static bool IsCanShowPromo;

        public void Init()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            string stringPromoGameData = null;

            if (GP_Variables.Has("PROMO_GAME_DATA"))
                stringPromoGameData = GP_Variables.GetString("PROMO_GAME_DATA");


            if (String.IsNullOrEmpty(stringPromoGameData))
            {
                _isDataInstalled = false;
            }
            else
            {
                _promoGameData = Newtonsoft.Json.JsonConvert.DeserializeObject<PromoGameData>(stringPromoGameData);
                _isDataInstalled = true;
            }

            IsCanShowPromo = _isDataInstalled;
#endif
        }


        public void ShowPromoGame()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (_isDataInstalled)
                EccentricJS.ECC_ShowPromoModal(_promoGameData.IdGp, _promoGameData.IdYa, _promoGameData.Title);
#endif
        }
    }


    [Serializable]
    public class PromoGameData
    {
        public int IdGp;
        public int IdYa;
        public string Title;
    }
}