using System.Diagnostics.CodeAnalysis;

namespace Axe.I18N.Core;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum LangType
{
    /// <summary>
    ///     EnglishName: Arabic
    ///     ChineseName: 阿拉伯语
    ///     NativeName: العربية
    /// </summary>
    [LangMetadata("Arabic", "العربية", "ar")]
    ar,

    /// <summary>
    ///     EnglishName: Catalan
    ///     ChineseName: 加泰罗尼亚语
    ///     NativeName: català
    /// </summary>
    [LangMetadata("Catalan", "català", "ca")]
    ca,

    /// <summary>
    ///     EnglishName: Chinese, Traditional (Hong Kong)
    ///     ChineseName: 繁体中文（香港）
    ///     NativeName: 繁體中文（香港）
    /// </summary>
    [LangMetadata("Chinese, Traditional (Hong Kong)", "繁體中文（香港）", "zh_HK")]
    zh_HK,

    /// <summary>
    ///     EnglishName: Chinese, Simplified (China mainland)
    ///     ChineseName: 简体中文
    ///     NativeName: 简体中文
    /// </summary>
    [LangMetadata("Chinese, Simplified (China mainland)", "简体中文", "zh_Hans")]
    zh_Hans,

    /// <summary>
    ///     EnglishName: Chinese, Traditional (Taiwan)
    ///     ChineseName: 繁体中文（台湾）
    ///     NativeName: 繁體中文（台灣）
    /// </summary>
    [LangMetadata("Chinese, Traditional (Taiwan)", "繁體中文（台灣）", "zh_Hant")]
    zh_Hant,

    /// <summary>
    ///     EnglishName: Croatian
    ///     ChineseName: 克罗地亚语
    ///     NativeName: hrvatski
    /// </summary>
    [LangMetadata("Croatian", "hrvatski", "hr")]
    hr,

    /// <summary>
    ///     EnglishName: Czech
    ///     ChineseName: 捷克语
    ///     NativeName: čeština
    /// </summary>
    [LangMetadata("Czech", "čeština", "cs")]
    cs,

    /// <summary>
    ///     EnglishName: Danish
    ///     ChineseName: 丹麦语
    ///     NativeName: dansk
    /// </summary>
    [LangMetadata("Danish", "dansk", "da")]
    da,

    /// <summary>
    ///     EnglishName: Dutch
    ///     ChineseName: 荷兰语
    ///     NativeName: Nederlands
    /// </summary>
    [LangMetadata("Dutch", "Nederlands", "nl")]
    nl,

    /// <summary>
    ///     EnglishName: English
    ///     ChineseName: 英语
    ///     NativeName: English
    /// </summary>
    [LangMetadata("English", "English", "en")]
    en,

    /// <summary>
    ///     EnglishName: English (Australia)
    ///     ChineseName: 英语（澳大利亚）
    ///     NativeName: English (Australia)
    /// </summary>
    [LangMetadata("English (Australia)", "English (Australia)", "en_AU")]
    en_AU,

    /// <summary>
    ///     EnglishName: English (India)
    ///     ChineseName: 英语（印度）
    ///     NativeName: English (India)
    /// </summary>
    [LangMetadata("English (India)", "English (India)", "en_IN")]
    en_IN,

    /// <summary>
    ///     EnglishName: English (United Kingdom)
    ///     ChineseName: 英语（英国）
    ///     NativeName: English (United Kingdom)
    /// </summary>
    [LangMetadata("English (United Kingdom)", "English (United Kingdom)", "en_GB")]
    en_GB,

    /// <summary>
    ///     EnglishName: Finnish
    ///     ChineseName: 芬兰语
    ///     NativeName: suomi
    /// </summary>
    [LangMetadata("Finnish", "suomi", "fi")]
    fi,

    /// <summary>
    ///     EnglishName: French
    ///     ChineseName: 法语
    ///     NativeName: français
    /// </summary>
    [LangMetadata("French", "français", "fr")]
    fr,

    /// <summary>
    ///     EnglishName: French (Canada)
    ///     ChineseName: 法语（加拿大）
    ///     NativeName: français (Canada)
    /// </summary>
    [LangMetadata("French (Canada)", "français (Canada)", "fr_CA")]
    fr_CA,

    /// <summary>
    ///     EnglishName: German
    ///     ChineseName: 德语
    ///     NativeName: Deutsch
    /// </summary>
    [LangMetadata("German", "Deutsch", "de")]
    de,

    /// <summary>
    ///     EnglishName: Greek
    ///     ChineseName: 希腊语
    ///     NativeName: Ελληνικά
    /// </summary>
    [LangMetadata("Greek", "Ελληνικά", "el")]
    el,

    /// <summary>
    ///     EnglishName: Hebrew
    ///     ChineseName: 希伯来语
    ///     NativeName: עברית
    /// </summary>
    [LangMetadata("Hebrew", "עברית", "he")]
    he,

    /// <summary>
    ///     EnglishName: Hindi
    ///     ChineseName: 印地语
    ///     NativeName: हिन्दी
    /// </summary>
    [LangMetadata("Hindi", "हिन्दी", "hi")]
    hi,

    /// <summary>
    ///     EnglishName: Hungarian
    ///     ChineseName: 匈牙利语
    ///     NativeName: magyar
    /// </summary>
    [LangMetadata("Hungarian", "magyar", "hu")]
    hu,

    /// <summary>
    ///     EnglishName: Indonesian
    ///     ChineseName: 印度尼西亚语
    ///     NativeName: Indonesia
    /// </summary>
    [LangMetadata("Indonesian", "Indonesia", "id")]
    id,

    /// <summary>
    ///     EnglishName: Italian
    ///     ChineseName: 意大利语
    ///     NativeName: italiano
    /// </summary>
    [LangMetadata("Italian", "italiano", "it")]
    it,

    /// <summary>
    ///     EnglishName: Japanese
    ///     ChineseName: 日语
    ///     NativeName: 日本語
    /// </summary>
    [LangMetadata("Japanese", "日本語", "ja")]
    ja,

    /// <summary>
    ///     EnglishName: Korean
    ///     ChineseName: 韩语
    ///     NativeName: 한국어
    /// </summary>
    [LangMetadata("Korean", "한국어", "ko")]
    ko,

    /// <summary>
    ///     EnglishName: Malay
    ///     ChineseName: 马来语
    ///     NativeName: Bahasa Melayu
    /// </summary>
    [LangMetadata("Malay", "Bahasa Melayu", "ms")]
    ms,

    /// <summary>
    ///     EnglishName: Norwegian Bokmål
    ///     ChineseName: 书面挪威语
    ///     NativeName: norsk bokmål
    /// </summary>
    [LangMetadata("Norwegian Bokmål", "norsk bokmål", "nb")]
    nb,

    /// <summary>
    ///     EnglishName: Polish
    ///     ChineseName: 波兰语
    ///     NativeName: polski
    /// </summary>
    [LangMetadata("Polish", "polski", "pl")]
    pl,

    /// <summary>
    ///     EnglishName: Portuguese (Brazil)
    ///     ChineseName: 葡萄牙语（巴西）
    ///     NativeName: português (Brasil)
    /// </summary>
    [LangMetadata("Portuguese (Brazil)", "português (Brasil)", "pt_BR")]
    pt_BR,

    /// <summary>
    ///     EnglishName: Portuguese (Portugal)
    ///     ChineseName: 葡萄牙语（葡萄牙）
    ///     NativeName: português (Portugal)
    /// </summary>
    [LangMetadata("Portuguese (Portugal)", "português (Portugal)", "pt_PT")]
    pt_PT,

    /// <summary>
    ///     EnglishName: Romanian
    ///     ChineseName: 罗马尼亚语
    ///     NativeName: română
    /// </summary>
    [LangMetadata("Romanian", "română", "ro")]
    ro,

    /// <summary>
    ///     EnglishName: Russian
    ///     ChineseName: 俄语
    ///     NativeName: русский
    /// </summary>
    [LangMetadata("Russian", "русский", "ru")]
    ru,

    /// <summary>
    ///     EnglishName: Slovak
    ///     ChineseName: 斯洛伐克语
    ///     NativeName: slovenčina
    /// </summary>
    [LangMetadata("Slovak", "slovenčina", "sk")]
    sk,

    /// <summary>
    ///     EnglishName: Slovenian
    ///     ChineseName: 斯洛文尼亚语
    ///     NativeName: slovenščina
    /// </summary>
    [LangMetadata("Slovenian", "slovenščina", "sl")]
    sl,

    /// <summary>
    ///     EnglishName: Spanish
    ///     ChineseName: 西班牙语
    ///     NativeName: español
    /// </summary>
    [LangMetadata("Spanish", "español", "es")]
    es,

    /// <summary>
    ///     EnglishName: Spanish (Latin America)
    ///     ChineseName: 西班牙语（拉丁美洲）
    ///     NativeName: español (Latinoamérica)
    /// </summary>
    [LangMetadata("Spanish (Latin America)", "español (Latinoamérica)", "es_419")]
    es_419,

    /// <summary>
    ///     EnglishName: Spanish (United States)
    ///     ChineseName: 西班牙语（美国）
    ///     NativeName: español (Estados Unidos)
    /// </summary>
    [LangMetadata("Spanish (United States)", "español (Estados Unidos)", "es_US")]
    es_US,

    /// <summary>
    ///     EnglishName: Swedish
    ///     ChineseName: 瑞典语
    ///     NativeName: svenska
    /// </summary>
    [LangMetadata("Swedish", "svenska", "sv")]
    sv,

    /// <summary>
    ///     EnglishName: Thai
    ///     ChineseName: 泰语
    ///     NativeName: ไทย
    /// </summary>
    [LangMetadata("Thai", "ไทย", "th")]
    th,

    /// <summary>
    ///     EnglishName: Turkish
    ///     ChineseName: 土耳其语
    ///     NativeName: Türkçe
    /// </summary>
    [LangMetadata("Turkish", "Türkçe", "tr")]
    tr,

    /// <summary>
    ///     EnglishName: Ukrainian
    ///     ChineseName: 乌克兰语
    ///     NativeName: українська
    /// </summary>
    [LangMetadata("Ukrainian", "українська", "uk")]
    uk,

    /// <summary>
    ///     EnglishName: Vietnamese
    ///     ChineseName: 越南语
    ///     NativeName: Tiếng Việt
    /// </summary>
    [LangMetadata("Vietnamese", "Tiếng Việt", "vi")]
    vi
}
