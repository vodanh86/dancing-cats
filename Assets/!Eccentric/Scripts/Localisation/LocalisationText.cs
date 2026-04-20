using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eccentric
{
    public class LocalisationText : MonoBehaviour
    {
        [SerializeField] private string _key;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => EccentricInit.Instance != null && EccentricInit.Instance.LocalisationManager != null);

            if(TryGetComponent(out TextMeshProUGUI textMeshProUGUI ))
                textMeshProUGUI.text = EccentricInit.Instance.LocalisationManager.GetText(_key);
            else if (TryGetComponent(out Text textLegacy))
                textLegacy.text = EccentricInit.Instance.LocalisationManager.GetText(_key);
        }
    }
}
