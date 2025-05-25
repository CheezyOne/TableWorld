using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameAnalytics : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendAnalyticsEvent(string eventName, string eventData);

    public static void TrackEvent(string eventName)
    {
        if (AdsManager.IsWebGL())
            SendAnalyticsEvent(eventName, null);

        Debug.Log($"EVENT: {eventName}");
    }

    public static void TrackEvent(string eventName, Dictionary<string, object> data)
    {
        if (AdsManager.IsWebGL())
        {
            string jsonData = JsonUtility.ToJson(new SerializableDictionary(data));
            SendAnalyticsEvent(eventName, jsonData);
        }

        Debug.Log($"EVENT: {eventName} | Data: {JsonUtility.ToJson(data)}");
    }

    // ¬спомогательный класс дл€ сериализации Dictionary
    [System.Serializable]
    private class SerializableDictionary
    {
        public List<string> keys = new List<string>();
        public List<object> values = new List<object>();

        public SerializableDictionary(Dictionary<string, object> dict)
        {
            foreach (var pair in dict)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
    }
}
