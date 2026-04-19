using GamePush;

namespace Eccentric
{
    public class AnalyticManagerOther : AnalyticManager
    {
        public override void SendAnalyticСustom(string nameEvent, int value)
        {
            GP_Analytics.Goal(nameEvent.ToUpper(), value);
        }

        public override void SendAnalyticСustom(string nameEvent, float value)
        {
            GP_Analytics.Goal(nameEvent.ToUpper(), value.ToString("0.00"));
        }

        public override void SendAnalyticСustom(string nameEvent, string value)
        {
            GP_Analytics.Goal(nameEvent.ToUpper(), value);
        }

        public override void SendAnalyticСustom(string nameEvent, bool value)
        {
            GP_Analytics.Goal(nameEvent.ToUpper(), value.ToString());
        }

        public override void SendAnalyticCompletedLevel(int level)
        {
            GP_Analytics.Goal("LEVEL_COMPLETED", level);
        }

        public override void SendAnalyticStartLevel(int level)
        {
            GP_Analytics.Goal("LEVEL_START", level);
        }

        public override void SendAnalyticFailedLevel(int level)
        {
            GP_Analytics.Goal("LEVEL_FAILED", level);
        }

        public override void SendAnalyticPurchaseCompleted(string namePurchase)
        {
            GP_Analytics.Goal("PURCHASE_COMPLETED", namePurchase);
        }

        public override void SendAnalyticRewardAdShowed(string nameRewardAd)
        {
            GP_Analytics.Goal("REWARD_AD_SHOWED", nameRewardAd);
        }
    }
}