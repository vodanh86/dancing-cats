var functions = {

  SDK_Init_LAGGED: function(devID,pubID) {

    devID = UTF8ToString(devID);
    pubID = UTF8ToString(pubID);

    (function(d, s, id) {
      var js,
        fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) return;
      js = d.createElement(s);
      js.id = id;

      js.onload=function(){
  
          LaggedAPI.init(devID, pubID);
        console.warn("Lagged init with adsense");
        SendMessage("EccentricInit", "OnSdkInitCallback");
      }
        js.src = "//lagged.com/api/rev-share/lagged.js";
      

      fjs.parentNode.insertBefore(js, fjs);
    })(document, "script", "lagged-jssdk");
  },
  SDK_Init_LAGGED_Without_Adsense: function() {


    (function(d, s, id) {
      var js,
          fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) return;
      js = d.createElement(s);
      js.id = id;

      js.onload=function(){
          LaggedAPI.init();
        console.warn("Lagged init without adsense");
        SendMessage("EccentricInit", "OnSdkInitCallback");
      }
        js.src = "//lagged.com/js/v4/lagged.js";
  

      fjs.parentNode.insertBefore(js, fjs);
    })(document, "script", "lagged-jssdk");
  },

  SDK_CallHighScore_LAGGED: function(score, board_id) {

    board_id=UTF8ToString(board_id);

    var boardinfo={};
    boardinfo.score=score;
    boardinfo.board=board_id;
    LaggedAPI.Scores.save(boardinfo, function(response) {
    if(response.success) {
    console.log('high score saved')
    }else {
    console.log(response.errormsg);
    }
    });

  },

  SDK_SaveAchievement_LAGGED: function(award) {

    award=UTF8ToString(award);

    var api_awards=[];
    api_awards.push(award);
    LaggedAPI.Achievements.save(api_awards, function(response) {
    if(response.success) {
    console.log('achievement saved')
    }else {
    console.log(response.errormsg);
    }
    });

  },

  SDK_ShowAd_LAGGED: function() {

    SendMessage("EccentricInit", "PauseGameCallback");

    LaggedAPI.APIAds.show(function() {

        SendMessage("EccentricInit", "ResumeGameCallback");

    });

  },

  SDK_CheckRewardAd_LAGGED: function(){

    this.rewardAdAvailable=false;
    this.rewardAdFunction=function(){};

    LaggedAPI.GEvents.reward(function(success, showAdFn){
      if(success){
        SendMessage("EccentricInit", "RewardAdReadyCallback");
        this.rewardAdAvailable=true;
        this.rewardAdFunction=showAdFn;
      }
    }, function(success){
      if(success){

        SendMessage("EccentricInit", "RewardAdSuccessCallback");

      }else{

        SendMessage("EccentricInit", "RewardAdFailCallback");

      }
       SendMessage("EccentricInit", "ResumeGameCallback");
      this.rewardAdAvailable=false;
      this.rewardAdFunction=function(){};
    });

  },

  SDK_PlayRewardAd_LAGGED: function(){
    if(this.rewardAdAvailable){
SendMessage("EccentricInit", "PauseGameCallback");
      this.rewardAdFunction();

    }
  },

};

mergeInto(LibraryManager.library, functions);
