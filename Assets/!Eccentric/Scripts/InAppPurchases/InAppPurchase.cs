using System;
using System.Collections.Generic;
using System.Linq;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class InAppPurchase
    {
        public event Action<string> OnPurchaseEvent;
        public event Action<string> OnFetchEvent;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        public List<FetchProducts> FetchProductsList = new();
#endif
        public string CurrencyOfPlatformYandex;
        private const string YANDEX_TEST_CURRENCY = "TST";

        public void Init()
        {
            if (EccentricInit.Instance.Platform == Platform.YANDEX)
                EccentricJS.ECC_GetCurrencyIconYandex();
        }


        public void Subscribe()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_Payments.OnFetchPlayerPurchases += OnFetchPlayerPurchasesHandler;
            GP_Payments.OnFetchProducts += OnFetchProductsHandler;
            OnPurchaseEvent += Consume;
            OnFetchEvent += Consume;
#endif
        }
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private void OnFetchProductsHandler(List<FetchProducts> productsList)
        {
            FetchProductsList = productsList;
        }

        private void OnFetchPlayerPurchasesHandler(List<FetchPlayerPurchases> productList)
        {
            if (productList.Count < 1) return;
            foreach (var item in productList)
            {
                OnFetchEvent?.Invoke(EccentricInit.InAppIdType == InAppIdType.Id
                    ? item.productId.ToString()
                    : item.tag);
            }
        }
#endif


        public void Unsubscribe()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_Payments.OnFetchPlayerPurchases -= OnFetchPlayerPurchasesHandler;
            GP_Payments.OnFetchProducts -= OnFetchProductsHandler;
            OnPurchaseEvent -= Consume;
            OnFetchEvent -= Consume;
#endif
        }

        public void Purchase(string idOrTag)
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (GP_Player.IsLoggedIn())
            {
                GP_Payments.Purchase(idOrTag, onPurchaseSuccess: s => OnPurchaseEvent?.Invoke(idOrTag));
            }
            else
            {
                EccentricJS.ECC_ShowLoginPanel();
            }
#endif
        }

        public void FetchPurchases()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_Payments.Fetch();
#endif
        }

        private void Consume(string idOrTag)
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_Payments.Consume(idOrTag);
#endif
        }

        public string GetPrice(int idProduct, bool isWithIcon = false)
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (isWithIcon)
            {
                var index = GetIndexPlatform();

                return $"{GetPriceWithoutIcon(idProduct).ToString()} <sprite index={index}>";
            }
            else
            {
                var price = FetchProductsList.Count > 0
                    ? FetchProductsList.First(product => product.id == idProduct).price
                    : 010;
                return price.ToString();
            }

#else
            return "";
#endif
        }

        public string GetPrice(string tagProduct, bool isWithIcon = false)
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (isWithIcon)
            {
                var index = GetIndexPlatform();

                return $"{GetPriceWithoutIcon(tagProduct).ToString()} <sprite index={index}>";
            }
            else
            {
                var price = FetchProductsList.Count > 0
                    ? FetchProductsList.First(product => product.tag == tagProduct).price
                    : 010;
                return price.ToString();
            }
#else
            return "";
#endif
        }

        public int GetIndexPlatform()
        {
            var index = EccentricInit.Instance.Platform switch
            {
                Platform.VK => 0,
                Platform.YANDEX => 1,
                Platform.KONGREGATE => 2,
                Platform.OK => 3,
                _ => 4,
            };
            if (index != 1) return index;
            return CurrencyOfPlatformYandex.ToUpper() == YANDEX_TEST_CURRENCY ? 5 : index;
        }

        private int GetPriceWithoutIcon(int idProduct)
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            return FetchProductsList.Count > 0
                ? FetchProductsList.First(product => product.id == idProduct).price
                : 0;
#else
            return 0;
#endif
        }

        private int GetPriceWithoutIcon(string tagProduct)
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            return FetchProductsList.Count > 0
                ? FetchProductsList.First(product => product.tag == tagProduct).price
                : 010;
#else
            return 0;
#endif
        }
    }
}