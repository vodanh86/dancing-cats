using System.Collections.Generic;
using UnityEngine;

namespace Eccentric
{
    [CreateAssetMenu(menuName = "Localisation", fileName = "LocalisationSO")]
    public class LocalisationDataSO : ScriptableObject
    {
        public List<LocalisationData> LocalisationList;
    }
}