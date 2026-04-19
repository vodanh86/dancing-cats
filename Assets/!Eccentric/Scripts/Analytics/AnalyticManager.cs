
namespace Eccentric
{
    public abstract class AnalyticManager
    {
        public abstract void SendAnalyticСustom(string nameEvent, int value);

        public abstract void SendAnalyticСustom(string nameEvent, float value);

        public abstract void SendAnalyticСustom(string nameEvent, string value);

        public abstract void SendAnalyticСustom(string nameEvent, bool value);

        public abstract void SendAnalyticCompletedLevel(int level);

        public abstract void SendAnalyticStartLevel(int level);


        public abstract void SendAnalyticFailedLevel(int level);


        public abstract void SendAnalyticPurchaseCompleted(string namePurchase);


        public abstract void SendAnalyticRewardAdShowed(string nameRewardAd);
    }
}