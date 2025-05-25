mergeInto(LibraryManager.library, {

GetLang : function(){
  var lang = ysdk != null ? ysdk.environment.i18n.lang : "null";
  var bufferSize = lengthBytesUTF8(lang) + 1;
  var buffer = _malloc(bufferSize);
  stringToUTF8(lang, buffer, bufferSize);
  return buffer;
},

    SendAnalyticsEvent: function(eventName, eventData) {
        if (typeof YaGames !== 'undefined') {
            var name = UTF8ToString(eventName);
            var data = eventData ? JSON.parse(UTF8ToString(eventData)) : null;
            
            YaGames.init().then(ysdk => {
                ysdk.analytics.reachGoal(name, data || {});
                console.log("Event sent:", name, data);
            });
        }
    }

    SubmitLeaderboardScore: function(score) {
        if (typeof YaGames !== 'undefined') {
            YaGames.init().then(ysdk => {
                ysdk.leaderboards.setLeaderboardScore(
                    "HighScore", 
                    score, 
                    { 
                        scoreContext: { type: 'number' } 
                    }
                ).then(() => {
                    console.log("Score submitted:", score);
                }).catch(error => {
                    console.error("Leaderboard error:", error);
                });
            });
        }
    },

ShowInterAd : function(){
  ysdk.adv.showFullscreenAdv({
      callbacks: {
          onOpen: () => {
            myGameInstance.SendMessage('AdsManager', 'OnInterAdOpen');
          },  
          onClose: function(wasShown) {
            window.focus();
            myGameInstance.SendMessage('AdsManager', 'OnInterAdClose');
          },
          onError: function(error) {
            console.log('FULLSCREEN AD ERROR.');
          }
      }
  })
},

ShowRVAd : function(){
  ysdk.adv.showRewardedVideo({
      callbacks: {
          onOpen: () => {
            console.log('Video ad open.');
            myGameInstance.SendMessage('AdsManager', 'OnRVOpen');
          },
          onRewarded: () => {
            console.log('Rewarded!');
            myGameInstance.SendMessage('AdsManager', 'OnRVReward');
          },
          onClose: () => {
            console.log('Video ad closed.');
            window.focus();
            myGameInstance.SendMessage('AdsManager', 'OnRVClose');
          }, 
          onError: (e) => {
            console.log('Error while open video ad:', e);
          }
      }
  })
},

SaveExtern : function(date){
  if(player != null)
  {
    var dateString = UTF8ToString(date);
    var myobj = JSON.parse(dateString);
    player.setData(myobj);
    console.log('SAVED TO SERVER.');
  }
  else
  {
    console.log('CANNOT SAVE TO SERVER.');
  }
},

LoadExtern : function(){
  if(player != null)
  {
    player.getData().then(_date => {
      const myJSON = JSON.stringify(_date);
      myGameInstance.SendMessage('SaveLoadSystem', 'SetExternPlayerData', myJSON);
      console.log('LOADED FROM SERVER.');
    });
  }
  else
  {
    myGameInstance.SendMessage('SaveLoadSystem', 'SavesLoaded');
    console.log('CANNOT LOAD FROM SERVER.');
  }
},

PlayerInited : function(){
  return player != null;
},

CallApiReady : function(){
  if(ysdk != null)
  {
    ysdk.features.LoadingAPI.ready();
  }
},

ShowBannerAd : function() {
  if (ysdk) {
    ysdk.adv.showBannerAdv({
      callbacks: {
        onOpen: () => {
          myGameInstance.SendMessage('AdsManager', 'OnBannerAdShown');
        },
        onClose: () => {
          myGameInstance.SendMessage('AdsManager', 'OnBannerAdHidden');
        },
        onError: (error) => {
          console.log('Banner error:', error);
        }
      }
    });
  }
},

HideBannerAd : function() {
  if (ysdk) {
    ysdk.adv.hideBannerAdv();
    myGameInstance.SendMessage('AdsManager', 'OnBannerAdHidden');
  }
},

});