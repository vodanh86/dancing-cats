using UnityEngine;

namespace Eccentric
{
    public class SetQualityGraphic
    {
        private readonly bool _isMobile;

        public SetQualityGraphic(bool isMobile)
        {
            _isMobile = isMobile;
        }
        public void SetQuality()
        {
            QualitySettings.SetQualityLevel(_isMobile ? 0 : 1);
        }
    }
}