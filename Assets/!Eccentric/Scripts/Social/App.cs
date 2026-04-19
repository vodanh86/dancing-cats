using System;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class App
    {
        private void ReviewRequest()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_App.ReviewRequest();
#endif
        }

        public void ShowRequestShortcut()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (GP_App.CanAddShortcut())
            {
                GP_App.AddShortcut();
            }
#endif
        }

        public void ShowRequestReview()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (GP_Player.IsLoggedIn() && !GP_App.IsAlreadyReviewed())
            {
                EccentricJS.ECC_ShowRequestReview();
            }
#endif
        }

        public void GameReady()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_Game.GameReady();
#endif
        }
    }
}