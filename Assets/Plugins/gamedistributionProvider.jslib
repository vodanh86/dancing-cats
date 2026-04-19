var functions = {
  SDK_Init_GD: function(gameKey) {
    gameKey = UTF8ToString(gameKey);
    window["GD_OPTIONS"] = {
      debug: false, // Enable debugging console. This will set a value in local storage as well, remove this value if you don't want debugging at all. You can also call it by running gdsdk.openConsole() within your browser console.
      gameId: gameKey, // Your gameId which is unique for each one of your games; can be found at your Gamedistribution.com account.
      onEvent: function(event) {
        switch (event.name) {
        case "SDK_READY":
        SendMessage("EccentricInit", "OnReadySDKCallback");
        break;
          case "SDK_GAME_START":
            SendMessage("EccentricInit", "ResumeGameCallback");
            break;
          case "SDK_GAME_PAUSE":
            SendMessage("EccentricInit", "PauseGameCallback");
            break;
          case "SDK_REWARDED_WATCH_COMPLETE":
            SendMessage("EccentricInit", "RewardedCompleteCallback");
            break;
          case "SDK_ERROR":
            break;
        }
      }
    };
    (function(d, s, id) {
      var js,
        fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) return;
      js = d.createElement(s);
      js.id = id;
      js.src = "//html5.api.gamedistribution.com/main.min.js";
      fjs.parentNode.insertBefore(js, fjs);
    })(document, "script", "gamedistribution-jssdk");
  },

  SDK_PreloadAd_GD: function() {
    if (
      typeof gdsdk !== "undefined" &&
      typeof gdsdk.preloadAd !== "undefined"
    ) {
      gdsdk.preloadAd(gdsdk.AdType.Rewarded)
        .then(function(response){
          SendMessage("EccentricInit", "PreloadRewardedVideoCallback",1);
        })
        .catch(function(error){
          SendMessage("EccentricInit", "PreloadRewardedVideoCallback",0);
        });
    }
  },

  SDK_ShowAd_GD: function(adType) {
    if (typeof gdsdk !== "undefined" && typeof gdsdk.showAd !== "undefined") {
      adType=UTF8ToString(adType)||gdsdk.AdType.Interstitial;

      gdsdk.showAd(adType)
      .then(function(response){
        if(adType===gdsdk.AdType.Rewarded){
          SendMessage("EccentricInit", "RewardedVideoSuccessCallback");
        }
      })
      .catch(function(error){
        if(adType===gdsdk.AdType.Rewarded){
          SendMessage("EccentricInit", "RewardedVideoFailureCallback");
        }
      });
    }
  },

  SDK_SendEvent_GD : function(options) {
    options = UTF8ToString(options);
    if (typeof gdsdk !== "undefined" && typeof gdsdk.sendEvent !== "undefined" && typeof options !== "undefined") {
      gdsdk.sendEvent(options)
      .then(function(response){
       
        console.log("Game event post message sent Succesfully...")
        
      })
      .catch(function(error){
        console.log(error.message)
      });
    }
  } 
};

mergeInto(LibraryManager.library, functions);