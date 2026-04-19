#if !GAMEDISTRIBUTION && !LAGGED && !JIO
using GamePush;
#endif

namespace Eccentric
{
    public class CollectionPanelNew
    {
        private static string KEY_COLLECTION;
        private readonly string _defaultCollectionName = "all";
        private const int _lengthName = 22;
        private const int _maxLengthCollection = 6;
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private static GamesCollectionsFetchData _gamesCollectionsFetchData;
#endif

        public void Show()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            if (_gamesCollectionsFetchData == null)
            {
#if !UNITY_EDITOR
                KEY_COLLECTION = GetNameCollection();
#else
                KEY_COLLECTION = "ALL";
#endif
                Fetch();
            }
            else
            {
                GP_GamesCollectionsOnOnGamesCollectionsFetch("",
                    _gamesCollectionsFetchData);
            }
#endif
        }
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private void Fetch() => GP_GamesCollections.Fetch(KEY_COLLECTION);
#endif
        public void Subscribe()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_GamesCollections.OnGamesCollectionsFetch += GP_GamesCollectionsOnOnGamesCollectionsFetch;
#endif
        }

        public void Unsubscribe()
        {
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
            GP_GamesCollections.OnGamesCollectionsFetch -= GP_GamesCollectionsOnOnGamesCollectionsFetch;
#endif
        }
#if !GAMEDISTRIBUTION && !LAGGED && !JIO
        private void GP_GamesCollectionsOnOnGamesCollectionsFetch(string arg0,
            GamesCollectionsFetchData collectionsFetchData)
        {
            _gamesCollectionsFetchData ??= collectionsFetchData;

            string lang = EccentricInit.Instance.Language == Language.Russian ? "ru" : "en";
            int idPublisher = EccentricInit.Instance.Publisher switch
            {
                Publisher.Eccentric => 9347,
                Publisher.Mamboo => 12906,
                _ => 0000000000,
            };
            
            string nameCollection = string.IsNullOrEmpty(collectionsFetchData.name)
                ? EccentricInit.Instance.LocalisationManager.GetText("our games")
                : collectionsFetchData.name.ToUpper();

            EccentricJS.ECC_ShowCollectionModal(nameCollection);

            var length = collectionsFetchData.games.Length > _maxLengthCollection
                ? _maxLengthCollection
                : collectionsFetchData.games.Length;
            for (int i = 0; i < length; i++)
            {
                var nameGame = collectionsFetchData.games[i].name;
                if (nameGame.Length > _lengthName)
                {
                    nameGame = nameGame.Remove(_lengthName);
                    nameGame += "...";
                }


               
                EccentricJS.ECC_SetCollectionData(i, nameGame,
                    collectionsFetchData.games[i].url,
                    $"https://s3.eponesh.com/games/files/{idPublisher}/banner_{collectionsFetchData.games[i].id}_{lang}.jpg");
            }
        }

        private string GetNameCollection() => GP_Variables.Has("KEY_COLLECTION")
            ? GP_Variables.GetString("KEY_COLLECTION").ToUpper()
            : _defaultCollectionName.ToUpper();
#endif
    }
}