using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eccentric
{
    public class LocalisationText : MonoBehaviour
    {
        [SerializeField] private string _key;

        void Start()
        {
            if(TryGetComponent(out TextMeshProUGUI textMeshProUGUI ))
                textMeshProUGUI.text = EccentricInit.Instance.LocalisationManager.GetText(_key);
            else if (TryGetComponent(out Text textLegacy))
                textLegacy.text = EccentricInit.Instance.LocalisationManager.GetText(_key);
        }
    }
}
