using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif
using TMPro;

namespace Eccentric
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class IconAndPriceForPurchase : MonoBehaviour
    {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        [SerializeField, HideIf(nameof(IsInAppTagType))]
        private int _inAppId;

        [SerializeField, HideIf(nameof(IsInAppIdType))]
        private string _inAppTag;

        private TextMeshProUGUI _textPrice;
        private List<FetchProducts> _products;

        private void Awake()
        {
            _textPrice = GetComponent<TextMeshProUGUI>();
        }

        private IEnumerator Start()
        {
            if (!GP_Payments.IsPaymentsAvailable()) yield break;

            yield return new WaitUntil(() => EccentricInit.Instance.InAppPurchase.FetchProductsList.Count != 0);

            SetPrice();
        }

        private string GetPrice(int idProduct)
        {
            return EccentricInit.Instance.InAppPurchase.GetPrice(idProduct);
        }

        private string GetPrice(string tagProduct)
        {
            return EccentricInit.Instance.InAppPurchase.GetPrice(tagProduct);
        }


        private void SetPrice()
        {
            var index = EccentricInit.Instance.InAppPurchase.GetIndexPlatform();
            _textPrice.text = EccentricInit.InAppIdType == InAppIdType.Id
                ? $"{GetPrice(_inAppId)} <sprite index={index}>"
                : $"{GetPrice(_inAppTag)} <sprite index={index}>";
        }

        private bool IsInAppIdType()
        {
            return EccentricInit.InAppIdType == InAppIdType.Id;
        }

        private bool IsInAppTagType()
        {
            return EccentricInit.InAppIdType == InAppIdType.Tag;
        }
#endif
    }
}