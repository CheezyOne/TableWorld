using UnityEngine;
using System.Linq;
using TMPro;

public class LanguageSetter : MonoBehaviour
{
    [SerializeField]
    private TranslateStruct[] translates = new TranslateStruct[2]
    {
        new TranslateStruct(AllLanguages.ru,""),
        new TranslateStruct(AllLanguages.en,"")
    };
    [SerializeField] private TMP_Text text;

    private void OnEnable()
    {
        LanguageSystem.OnLanguageSetted += SetTranslate;
    }

    private void OnDisable()
    {
        LanguageSystem.OnLanguageSetted -= SetTranslate;
    }

    private void Start()
    {
        SetTranslate();
    }

    private void SetTranslate()
    {
        if (text == null || translates == null || translates.Length <= 0)
            return;

        string targetLanguage = LanguageSystem.DEFAULT_LANGUAGE;

        if (AdsManager.IsWebGL())
        {
            targetLanguage = LanguageSystem.Instance?.GetGameLanguage ?? LanguageSystem.DEFAULT_LANGUAGE;
        }
        else
        {
            targetLanguage = Resources.Load<LanguageConfig>(nameof(LanguageConfig)).TargetLanguage.ToString();
        }

        string translatedTxt = "";

        TranslateStruct translate = translates.FirstOrDefault(x => x.SystemLanguage.ToString().Equals(targetLanguage));

        if (translate.Equals(default))
            return;

        translatedTxt = translate.text;

        if (text != null)
            text.text = translatedTxt;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        text = GetComponent<TMP_Text>();
        SetTranslate();
    }
#endif
}