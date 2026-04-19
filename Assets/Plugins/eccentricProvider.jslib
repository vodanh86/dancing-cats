mergeInto(LibraryManager.library, {

    ECC_ReloadPageExtern: function () {
        window.location.reload();
    },

    ECC_LanguageExtern: function () {
        let lang = getLang();
        let bufferSize = lengthBytesUTF8(lang) + 1;
        let buffer = _malloc(bufferSize);
        stringToUTF8(lang, buffer, bufferSize);
        return buffer;

    },

    ECC_IsMobileExtern: function () {
        return isMobile();
    },

    ECC_GetCurrentPlatformExtern: function () {
        let platform = getCurrentPlatform();
        let bufferSize = lengthBytesUTF8(platform) + 1;
        let buffer = _malloc(bufferSize);
        stringToUTF8(platform, buffer, bufferSize);
        return buffer;
    },

    ECC_ShowPromoGameModalExtern: function (idGp, idYa, title) {
        showPromoGameModal(idGp, idYa, UTF8ToString(title));
    },

    ECC_ShowCustomModalExtern: function (text, isCanClose) {
        showCustomModal(UTF8ToString(text), isCanClose);
    },
    ECC_ShowCollectionModalExtern: function (nameCollection) {
        showCollectionModal(UTF8ToString(nameCollection));
    },
    ECC_SetCollectionDataExtern: function (index, nameGame, link, urlBanner) {
        setCollectionData(index, UTF8ToString(nameGame), UTF8ToString(link), UTF8ToString(urlBanner));
    },
    ECC_ShowLeaderboardExtern: function (){
        showLeaderboard();
    },
    ECC_SetLeaderboardDataExtern: function (index,number,avatarUrl,name,score,isPlayer){
        setLeaderboardData(index,number,UTF8ToString(avatarUrl),UTF8ToString(name),score,isPlayer);
    },
    ECC_ShowRequestReviewExtern: function (){
        showRequestReview();
    },
    ECC_ShowLoginPanelExtern: function (){
        showLoginPanel();
    },
    ECC_IsIOSExtern: function (){
       return isIOS();
    },
    ECC_GetCurrencyIconYandexExtern: function (){
        getCurrencyIconYandex().then(currencyCode => {
            let currency = currencyCode;
            UnityInstance.SendMessage('EccentricInit', 'SetYandexCurrency', currency);
        }).catch(error => {
            console.error("Произошла ошибка:", error);
        });
    },

});
