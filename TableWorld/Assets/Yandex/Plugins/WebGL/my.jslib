mergeInto(LibraryManager.library, {
  // 1. GetLang - добавлена проверка на глобальный ysdk
  GetLang: function() {
    var lang = "null";
    if (typeof ysdk !== 'undefined' && ysdk.environment && ysdk.environment.i18n) {
      lang = ysdk.environment.i18n.lang;
    }
    
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  // 2. SendAnalyticsEvent - добавлены проверки модуля аналитики
SendAnalyticsEvent: function(eventName, eventData) {
  var name = UTF8ToString(eventName);
  var data = {};
  
  try {
    if (eventData) {
      var dataStr = UTF8ToString(eventData);
      data = dataStr ? JSON.parse(dataStr) : {};
    }
  } catch(e) {
    console.error("Event data parse error", e);
  }
  // Формируем идентификатор цели (ES5-совместимый)
  var goalId = name.toLowerCase();
  console.log("Trying to reach goal:", goalId );
  data.value = data.value || 0;

  // Отправка в Яндекс Метрику
  if (typeof ym === 'function') {
    try {
      // Ваш счётчик
      ym(103375417, 'reachGoal', goalId, data);
      
      // Счётчик Яндекс Игр (добавлено!)
      ym(49035923, 'reachGoal', '438179_' + goalId, data);
      
      console.log("Events sent to both counters");
    } catch(e) {
      console.error("YM error:", e);
    }
  }

  // Локальное логирование (без spread оператора)
  try {
    var events = JSON.parse(localStorage.getItem('analytics_events') || '[]');
    var eventEntry = { goal_id: goalId, timestamp: new Date().toISOString() };
    // Копируем свойства вручную
    for (var key in data) {
      if (data.hasOwnProperty(key)) {
        eventEntry[key] = data[key];
      }
    }
    events.push(eventEntry);
    localStorage.setItem('analytics_events', JSON.stringify(events));
  } catch(e) {
    console.error("Local storage error:", e);
  }
},

//3 ===============================
SubmitLeaderboardScore: function(score) {
  // Проверка глобального объекта
  if (typeof YaGames === 'undefined') {
    console.error("YaGames SDK not found. Ensure script is loaded.");
    try {
      localStorage.setItem('leaderboard_fallback', score);
    } catch (e) {
      console.error("LocalStorage error:", e);
    }
    return;
  }

  YaGames.init().then(function(ysdk) {
    // Защищённая проверка версии (без опциональной цепочки)
    var version = 'unknown';
    if (ysdk.environment && ysdk.environment.sdk && ysdk.environment.sdk.version) {
      version = ysdk.environment.sdk.version;
    }
    console.log("Yandex Games SDK v" + version);

    // Поиск API через несколько вариантов (без ?.)
    var leaderboardsAPI = ysdk.leaderboards;
    if (!leaderboardsAPI && typeof ysdk.getLeaderboards === 'function') {
      leaderboardsAPI = ysdk.getLeaderboards();
    }
    if (!leaderboardsAPI && ysdk._leaderboards) {
      leaderboardsAPI = ysdk._leaderboards;
    }

    if (!leaderboardsAPI) {
      console.warn("Leaderboards API not found. Available modules:", Object.keys(ysdk));
      try {
        localStorage.setItem('leaderboard_fallback', score);
      } catch (e) {
        console.error("LocalStorage error:", e);
      }
      return;
    }

    // Определение доступного метода
    var submitMethod = leaderboardsAPI.setLeaderboardScore;
    if (typeof submitMethod !== 'function') {
      submitMethod = leaderboardsAPI.setScore;
    }
    if (typeof submitMethod !== 'function') {
      submitMethod = leaderboardsAPI.submitScore;
    }

    if (typeof submitMethod !== 'function') {
      throw new Error("No submit method found in leaderboards API");
    }

    // Отправка данных
    return submitMethod.call(
      leaderboardsAPI,
      "HighScore",
      parseInt(score, 10),
      { scoreContext: { type: 'number' } }
    ).then(function() {
      console.log("Score submitted to Yandex Leaderboard");
    }).catch(function(error) {
      console.error("Submission error:", error);
      try {
        localStorage.setItem('leaderboard_error', JSON.stringify(error));
      } catch (e) {
        console.error("LocalStorage error:", e);
      }
    });

  }).catch(function(error) {
    console.error("SDK init failed:", error);
    // Fallback для разных случаев
    try {
      if (error.message && error.message.includes('auth')) {
        localStorage.setItem('leaderboard_needs_auth', score);
      } else {
        localStorage.setItem('leaderboard_fallback', score);
      }
    } catch (e) {
      console.error("LocalStorage error:", e);
    }
  });
},

  // 4. ShowInterAd - добавлена проверка ysdk
  ShowInterAd: function() {
    if (typeof ysdk === 'undefined' || !ysdk.adv) {
      console.error("Ads module not available");
      myGameInstance.SendMessage('AdsManager', 'OnInterAdError');
      return;
    }
    
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
          console.error('Fullscreen ad error:', error);
          myGameInstance.SendMessage('AdsManager', 'OnInterAdError');
        }
      }
    });
  },

  // 5. ShowRVAd - добавлена проверка ysdk
  ShowRVAd: function() {
    if (typeof ysdk === 'undefined' || !ysdk.adv) {
      console.error("Ads module not available");
      myGameInstance.SendMessage('AdsManager', 'OnRVError');
      return;
    }
    
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
          console.error('Rewarded video error:', e);
          myGameInstance.SendMessage('AdsManager', 'OnRVError');
        }
      }
    });
  },

  // 6. SaveExtern - добавлена обработка ошибок JSON
  SaveExtern: function(data) {
    if (typeof player === 'undefined' || player === null) {
      console.error('Player not initialized');
      return;
    }
    
    try {
      var dateString = UTF8ToString(data);
      var myobj = JSON.parse(dateString);
      player.setData(myobj)
        .then(() => console.log('Saved to server'))
        .catch(e => console.error('Save error:', e));
    } catch(e) {
      console.error('JSON parse error:', e);
    }
  },

  // 7. LoadExtern - улучшена обработка ошибок
  LoadExtern: function() {
    if (typeof player === 'undefined' || player === null) {
      console.error('Player not initialized');
      myGameInstance.SendMessage('SaveLoadSystem', 'SavesLoaded');
      return;
    }
    
    player.getData()
      .then(_date => {
        try {
          const myJSON = JSON.stringify(_date);
          myGameInstance.SendMessage('SaveLoadSystem', 'SetExternPlayerData', myJSON);
          console.log('Loaded from server');
        } catch(e) {
          console.error('Data serialization error:', e);
          myGameInstance.SendMessage('SaveLoadSystem', 'SavesLoaded');
        }
      })
      .catch(e => {
        console.error('Load error:', e);
        myGameInstance.SendMessage('SaveLoadSystem', 'SavesLoaded');
      });
  },

  // 8. PlayerInited - проверка через глобальную переменную
  PlayerInited: function() {
    return (typeof player !== 'undefined' && player !== null) ? 1 : 0;
  },

  // 9. CallApiReady - добавлена проверка ysdk
  CallApiReady: function() {
    if (typeof ysdk !== 'undefined' && ysdk.features && ysdk.features.LoadingAPI) {
      ysdk.features.LoadingAPI.ready();
    }
  },

  // 10. ShowBannerAd - добавлены проверки
  ShowBannerAd: function() {
    if (typeof ysdk === 'undefined' || !ysdk.adv) {
      console.error("Ads module not available");
      return;
    }
    
    ysdk.adv.showBannerAdv({
      callbacks: {
        onOpen: () => {
          myGameInstance.SendMessage('AdsManager', 'OnBannerAdShown');
        },
        onClose: () => {
          myGameInstance.SendMessage('AdsManager', 'OnBannerAdHidden');
        },
        onError: (error) => {
          console.error('Banner error:', error);
        }
      }
    });
  },

  // 11. HideBannerAd - добавлена проверка
  HideBannerAd: function() {
    if (typeof ysdk === 'undefined' || !ysdk.adv) {
      console.error("Ads module not available");
      return;
    }
    
    ysdk.adv.hideBannerAdv();
    myGameInstance.SendMessage('AdsManager', 'OnBannerAdHidden');
  }
});