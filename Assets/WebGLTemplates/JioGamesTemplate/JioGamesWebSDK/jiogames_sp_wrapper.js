
function postScore(score) {
    console.log("JioGames: postScore() ",score);
    
    if  (window.DroidHandler) {
        window.DroidHandler.postScore(score);
    }
}

function cacheAdMidRoll(adKeyId, source) {
    if(!adKeyId || !source){
        adKeyId? null: (console.log("JioGames: cacheAdMidRoll() no adKeyId to cacheAd ",adKeyId));
        source? null : (console.log("JioGames: cacheAdMidRoll() no source to cacheAd ",source));
        return;
    }
    else{
        console.log("JioGames: cacheAdMidRoll() adKeyId : " + adKeyId + " source : " + source);
    }
    if (window.DroidHandler) {
        window.DroidHandler.cacheAd(adKeyId, source);
    }
}

function showAdMidRoll(adKeyId, source) {
    if(!adKeyId || !source){
        adKeyId? null: (console.log("JioGames: showAdMidRoll() no adKeyId to showAd ",adKeyId));
        source? null : (console.log("JioGames: showAdMidRoll() no source to showAd ",source));
        return;
    }
    else{
        console.log("JioGames: showAdMidRoll() adKeyId : " + adKeyId + " source : " + source);
    }
    if (window.DroidHandler) {
        window.DroidHandler.showAd(adKeyId, source);
    }
}

function cacheAdRewardedVideo(adKeyId, source) {
    if (!adKeyId || !source) {
        adKeyId ? null : (console.log("JioGames: cacheAdRewardedVideo() no adKeyId to cacheAd ", adKeyId));
        source ? null : (console.log("JioGames: cacheAdRewardedVideo() no source to cacheAd ", source));
        return;
    }
    else{
        console.log("JioGames: cacheAdRewardedVideo() adKeyId : " + adKeyId + " source : " + source);
    }
    if (window.DroidHandler) {
        window.DroidHandler.cacheAdRewarded(adKeyId, source);    
    }
}

function showAdRewardedVideo(adKeyId, source) {
    if (!adKeyId || !source) {
        adKeyId ? null : (console.log("JioGames: showAdRewardedVideo() no adKeyId to showAd ", adKeyId));
        source ? null : (console.log("JioGames: showAdRewardedVideo() no source to showAd ", source));
        return;
    }
    else{
        console.log("JioGames: showAdRewardedVideo() adKeyId : " + adKeyId + " source : " + source);
    }
    if (window.DroidHandler) {
        window.DroidHandler.showAdRewarded(adKeyId, source);
    }
}

function getUserProfile() {
    console.log("JioGames: getUserProfile called");
    if (window.DroidHandler) {
        window.DroidHandler.getUserProfile();
    }
}

window.onAdPrepared = function (adSpotKey) {
    console.log("JioGames: onAdPrepared "+adSpotKey.toString());
    unityInstance.SendMessage('EccentricInit', 'onAdPrepared', adSpotKey);
};
window.onAdClosed = function (data, pIsVideoCompleted, pIsEligibleForReward) {
    console.log("JioGames: onAdClosed data : "+data.toString(), "pIsVideoCompleted : "+pIsVideoCompleted+" pIsEligibleForReward : "+pIsEligibleForReward);
    var localData = data + "|" + pIsVideoCompleted + "|" + pIsEligibleForReward;
    unityInstance.SendMessage('EccentricInit', 'onAdClosed', localData);
};
window.onAdFailedToLoad = function (data, pDescription){
    console.log("JioGames: onAdFailedToLoad data : "+data.toString()+" pDescription : "+pDescription);
    var localData = data + "|" + pDescription;
    unityInstance.SendMessage('EccentricInit', 'onAdFailedToLoad', localData);
};

window.onAdReady = function (adSpotKey) { };
window.onAdClose = function (adSpotKey) { };
window.onAdMediaEnd = function (data, pSuccess, pValue) { };
window.onAdClick = function (adSpotKey) {};
window.onAdMediaCollapse = function (adSpotKey) {};
window.onAdMediaExpand = function (adSpotKey) {};
window.onAdMediaStart = function (adSpotKey) {};
window.onAdRefresh = function (adSpotKey) {};
window.onAdRender = function (adSpotKey) {};
window.onAdRender = function (adSpotKey) {};
window.onAdReceived = function (adSpotKey) {};
window.onAdSkippable = function (adSpotKey) {};
window.onAdView = function (adSpotKey) {};

window.onUserProfileResponse = function(message)
{
   console.log("JioGames: onUserProfileResponse "+[JSON.stringify(message)]);
   unityInstance.SendMessage('EccentricInit', 'onUserProfileResponse', JSON.stringify(message));
};

window.onClientPause = function () {
    console.log("JioGames: onClientPause called");
    unityInstance.SendMessage('EccentricInit', 'onClientPause');  // Set the timescale to zero 
};

window.onClientResume = function () {
    console.log("JioGames: onClientResume called");
    unityInstance.SendMessage('EccentricInit', 'onClientResume');  // Set the timescale to zero 
};

// Callback received whenever the Jio app is sent to background or brought to foreground
document.addEventListener("visibilitychange", function() {
    if (document.visibilityState === 'visible') {
       console.log("JioGames: App Visible");
       unityInstance.SendMessage('EccentricInit', 'ResumeGameSound');
    } else {  
       console.log("JioGames: App Hidden");
       unityInstance.SendMessage('EccentricInit', 'PauseGameSound');
    }
});


console.log("JioGames: SDK initialize : 1.0.0");

var capabilities = {
	'bstack:options' : {
		"deviceOrientation" : "landscape",
	}
}

window.addEventListener("keydown", KeyDown);
function KeyDown(e){
    console.log(`You KeyDown ${e.code}`);
}
window.addEventListener("keyup", KeyUp)
function KeyUp(e){
    console.log(`You KeyUp ${e.key}`);
}


let controllerIndex;
window.addEventListener("gamepadconnected", (e) => {
    console.log(
      "Gamepad connected at index : " + 
      e.gamepad.index
    );
    controllerIndex = e.gamepad.index;
    gameLoop();
  });
  window.addEventListener("gamepaddisconnected", (e) => {
    controllerIndex = null;
    console.log("Gamepad disconnected")
  });

  function gameLoop(){
    console.log("gameLoop");
    if(controllerIndex != null){
        const gamepad = navigator.getGamepads()[controllerIndex];
        const buttons = gamepad.buttons;


        console.log("buttons[12] : " + buttons[12].pressed);
        console.log("buttons[13] : " + buttons[13].pressed);
        console.log("buttons[14] : " + buttons[14].pressed);
        console.log("buttons[15] : " + buttons[15].pressed);
        console.log("buttons[0] : " + buttons[0].pressed);
        console.log("buttons[1] : " + buttons[1].pressed);
        console.log("buttons[2] : " + buttons[2].pressed);
        console.log("buttons[3] : " + buttons[3].pressed);

        Gamepad
    }
  }

  gameLoop();