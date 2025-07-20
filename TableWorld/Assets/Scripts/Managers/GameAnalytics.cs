using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameAnalytics : SingletonDontDestroyOnLoad<GameAnalytics>
{
    [DllImport("__Internal")]
    private static extern void SendAnalyticsEvent(string eventName, string eventData);

    public void TrackEvent(string eventName, Dictionary<string, object> eventParams = null)
    {
        if (AdsManager.IsWebGL())
        {
            // Конвертируем параметры в JSON
            string jsonData = "";
            if (eventParams != null)
            {
                try
                {
                    jsonData = JsonUtility.ToJson(new SerializationWrapper(eventParams));
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to serialize params: {e}");
                }
            }

            SendAnalyticsEvent(eventName, jsonData);
        }
        else
        {
            // Логирование для редактора и других платформ
            Debug.Log($"[Analytics] {eventName}: {JsonUtility.ToJson(eventParams)}");
        }
    }

    // Вспомогательный класс для сериализации Dictionary
    [System.Serializable]
    private class SerializationWrapper
    {
        public List<string> keys = new List<string>();
        public List<object> values = new List<object>();

        public SerializationWrapper(Dictionary<string, object> dict)
        {
            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
    }
}
