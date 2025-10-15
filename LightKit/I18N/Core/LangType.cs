using System.Diagnostics.CodeAnalysis;

namespace LightKit.I18N.Core;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum LangType
{
    /// <summary>
    ///     Emoji: 🌐
    ///     EnglishName: Arabic
    ///     ChineseName: 阿拉伯语
    ///     NativeName: العربية
    /// </summary>
    [LangMetadata("🌐", "Arabic", "العربية", "ar")]
    ar,

    /// <summary>
    ///     Emoji: 🇪🇸
    ///     EnglishName: Catalan
    ///     ChineseName: 加泰罗尼亚语
    ///     NativeName: català
    /// </summary>
    [LangMetadata("🇪🇸", "Catalan", "català", "ca")]
    ca,

    /// <summary>
    ///     Emoji: 🇭🇰
    ///     EnglishName: Chinese, Traditional (Hong Kong)
    ///     ChineseName: 繁体中文（香港）
    ///     NativeName: 繁體中文（香港）
    /// </summary>
    [LangMetadata("🇭🇰", "Chinese, Traditional (Hong Kong)", "繁體中文（香港）", "zh_HK")]
    zh_HK,

    /// <summary>
    ///     Emoji: 🇨🇳
    ///     EnglishName: Chinese, Simplified (China mainland)
    ///     ChineseName: 简体中文
    ///     NativeName: 简体中文
    /// </summary>
    [LangMetadata("🇨🇳", "Chinese, Simplified (China mainland)", "简体中文", "zh_Hans")]
    zh_Hans,

    /// <summary>
    ///     Emoji: 🇹🇼
    ///     EnglishName: Chinese, Traditional (Taiwan)
    ///     ChineseName: 繁体中文（台湾）
    ///     NativeName: 繁體中文（台灣）
    /// </summary>
    [LangMetadata("🇹🇼", "Chinese, Traditional (Taiwan)", "繁體中文（台灣）", "zh_Hant")]
    zh_Hant,

    /// <summary>
    ///     Emoji: 🇭🇷
    ///     EnglishName: Croatian
    ///     ChineseName: 克罗地亚语
    ///     NativeName: hrvatski
    /// </summary>
    [LangMetadata("🇭🇷", "Croatian", "hrvatski", "hr")]
    hr,

    /// <summary>
    ///     Emoji: 🇨🇿
    ///     EnglishName: Czech
    ///     ChineseName: 捷克语
    ///     NativeName: čeština
    /// </summary>
    [LangMetadata("🇨🇿", "Czech", "čeština", "cs")]
    cs,

    /// <summary>
    ///     Emoji: 🇩🇰
    ///     EnglishName: Danish
    ///     ChineseName: 丹麦语
    ///     NativeName: dansk
    /// </summary>
    [LangMetadata("🇩🇰", "Danish", "dansk", "da")]
    da,

    /// <summary>
    ///     Emoji: 🇳🇱
    ///     EnglishName: Dutch
    ///     ChineseName: 荷兰语
    ///     NativeName: Nederlands
    /// </summary>
    [LangMetadata("🇳🇱", "Dutch", "Nederlands", "nl")]
    nl,

    /// <summary>
    ///     Emoji: 🇺🇸
    ///     EnglishName: English
    ///     ChineseName: 英语
    ///     NativeName: English
    /// </summary>
    [LangMetadata("🇺🇸", "English", "English", "en")]
    en,

    /// <summary>
    ///     Emoji: 🇦🇺
    ///     EnglishName: English (Australia)
    ///     ChineseName: 英语（澳大利亚）
    ///     NativeName: English (Australia)
    /// </summary>
    [LangMetadata("🇦🇺", "English (Australia)", "English (Australia)", "en_AU")]
    en_AU,

    /// <summary>
    ///     Emoji: 🇮🇳
    ///     EnglishName: English (India)
    ///     ChineseName: 英语（印度）
    ///     NativeName: English (India)
    /// </summary>
    [LangMetadata("🇮🇳", "English (India)", "English (India)", "en_IN")]
    en_IN,

    /// <summary>
    ///     Emoji: 🇬🇧
    ///     EnglishName: English (United Kingdom)
    ///     ChineseName: 英语（英国）
    ///     NativeName: English (United Kingdom)
    /// </summary>
    [LangMetadata("🇬🇧", "English (United Kingdom)", "English (United Kingdom)", "en_GB")]
    en_GB,

    /// <summary>
    ///     Emoji: 🇫🇮
    ///     EnglishName: Finnish
    ///     ChineseName: 芬兰语
    ///     NativeName: suomi
    /// </summary>
    [LangMetadata("🇫🇮", "Finnish", "suomi", "fi")]
    fi,

    /// <summary>
    ///     Emoji: 🇫🇷
    ///     EnglishName: French
    ///     ChineseName: 法语
    ///     NativeName: français
    /// </summary>
    [LangMetadata("🇫🇷", "French", "français", "fr")]
    fr,

    /// <summary>
    ///     Emoji: 🇨🇦
    ///     EnglishName: French (Canada)
    ///     ChineseName: 法语（加拿大）
    ///     NativeName: français (Canada)
    /// </summary>
    [LangMetadata("🇨🇦", "French (Canada)", "français (Canada)", "fr_CA")]
    fr_CA,

    /// <summary>
    ///     Emoji: 🇩🇪
    ///     EnglishName: German
    ///     ChineseName: 德语
    ///     NativeName: Deutsch
    /// </summary>
    [LangMetadata("🇩🇪", "German", "Deutsch", "de")]
    de,

    /// <summary>
    ///     Emoji: 🇬🇷
    ///     EnglishName: Greek
    ///     ChineseName: 希腊语
    ///     NativeName: Ελληνικά
    /// </summary>
    [LangMetadata("🇬🇷", "Greek", "Ελληνικά", "el")]
    el,

    /// <summary>
    ///     Emoji: 🇮🇱
    ///     EnglishName: Hebrew
    ///     ChineseName: 希伯来语
    ///     NativeName: עברית
    /// </summary>
    [LangMetadata("🇮🇱", "Hebrew", "עברית", "he")]
    he,

    /// <summary>
    ///     Emoji: 🇮🇳
    ///     EnglishName: Hindi
    ///     ChineseName: 印地语
    ///     NativeName: हिन्दी
    /// </summary>
    [LangMetadata("🇮🇳", "Hindi", "हिन्दी", "hi")]
    hi,

    /// <summary>
    ///     Emoji: 🇭🇺
    ///     EnglishName: Hungarian
    ///     ChineseName: 匈牙利语
    ///     NativeName: magyar
    /// </summary>
    [LangMetadata("🇭🇺", "Hungarian", "magyar", "hu")]
    hu,

    /// <summary>
    ///     Emoji: 🇮🇩
    ///     EnglishName: Indonesian
    ///     ChineseName: 印度尼西亚语
    ///     NativeName: Indonesia
    /// </summary>
    [LangMetadata("🇮🇩", "Indonesian", "Indonesia", "id")]
    id,

    /// <summary>
    ///     Emoji: 🇮🇹
    ///     EnglishName: Italian
    ///     ChineseName: 意大利语
    ///     NativeName: italiano
    /// </summary>
    [LangMetadata("🇮🇹", "Italian", "italiano", "it")]
    it,

    /// <summary>
    ///     Emoji: 🇯🇵
    ///     EnglishName: Japanese
    ///     ChineseName: 日语
    ///     NativeName: 日本語
    /// </summary>
    [LangMetadata("🇯🇵", "Japanese", "日本語", "ja")]
    ja,

    /// <summary>
    ///     Emoji: 🇰🇷
    ///     EnglishName: Korean
    ///     ChineseName: 韩语
    ///     NativeName: 한국어
    /// </summary>
    [LangMetadata("🇰🇷", "Korean", "한국어", "ko")]
    ko,

    /// <summary>
    ///     Emoji: 🇲🇾
    ///     EnglishName: Malay
    ///     ChineseName: 马来语
    ///     NativeName: Bahasa Melayu
    /// </summary>
    [LangMetadata("🇲🇾", "Malay", "Bahasa Melayu", "ms")]
    ms,

    /// <summary>
    ///     Emoji: 🇳🇴
    ///     EnglishName: Norwegian Bokmål
    ///     ChineseName: 书面挪威语
    ///     NativeName: norsk bokmål
    /// </summary>
    [LangMetadata("🇳🇴", "Norwegian Bokmål", "norsk bokmål", "nb")]
    nb,

    /// <summary>
    ///     Emoji: 🇵🇱
    ///     EnglishName: Polish
    ///     ChineseName: 波兰语
    ///     NativeName: polski
    /// </summary>
    [LangMetadata("🇵🇱", "Polish", "polski", "pl")]
    pl,

    /// <summary>
    ///     Emoji: 🇧🇷
    ///     EnglishName: Portuguese (Brazil)
    ///     ChineseName: 葡萄牙语（巴西）
    ///     NativeName: português (Brasil)
    /// </summary>
    [LangMetadata("🇧🇷", "Portuguese (Brazil)", "português (Brasil)", "pt_BR")]
    pt_BR,

    /// <summary>
    ///     Emoji: 🇵🇹
    ///     EnglishName: Portuguese (Portugal)
    ///     ChineseName: 葡萄牙语（葡萄牙）
    ///     NativeName: português (Portugal)
    /// </summary>
    [LangMetadata("🇵🇹", "Portuguese (Portugal)", "português (Portugal)", "pt_PT")]
    pt_PT,

    /// <summary>
    ///     Emoji: 🇷🇴
    ///     EnglishName: Romanian
    ///     ChineseName: 罗马尼亚语
    ///     NativeName: română
    /// </summary>
    [LangMetadata("🇷🇴", "Romanian", "română", "ro")]
    ro,

    /// <summary>
    ///     Emoji: 🇷🇺
    ///     EnglishName: Russian
    ///     ChineseName: 俄语
    ///     NativeName: русский
    /// </summary>
    [LangMetadata("🇷🇺", "Russian", "русский", "ru")]
    ru,

    /// <summary>
    ///     Emoji: 🇸🇰
    ///     EnglishName: Slovak
    ///     ChineseName: 斯洛伐克语
    ///     NativeName: slovenčina
    /// </summary>
    [LangMetadata("🇸🇰", "Slovak", "slovenčina", "sk")]
    sk,

    /// <summary>
    ///     Emoji: 🇸🇮
    ///     EnglishName: Slovenian
    ///     ChineseName: 斯洛文尼亚语
    ///     NativeName: slovenščina
    /// </summary>
    [LangMetadata("🇸🇮", "Slovenian", "slovenščina", "sl")]
    sl,

    /// <summary>
    ///     Emoji: 🇪🇸
    ///     EnglishName: Spanish
    ///     ChineseName: 西班牙语
    ///     NativeName: español
    /// </summary>
    [LangMetadata("🇪🇸", "Spanish", "español", "es")]
    es,

    /// <summary>
    ///     Emoji: 🌎
    ///     EnglishName: Spanish (Latin America)
    ///     ChineseName: 西班牙语（拉丁美洲）
    ///     NativeName: español (Latinoamérica)
    /// </summary>
    [LangMetadata("🌎", "Spanish (Latin America)", "español (Latinoamérica)", "es_419")]
    es_419,

    /// <summary>
    ///     Emoji: 🇺🇸
    ///     EnglishName: Spanish (United States)
    ///     ChineseName: 西班牙语（美国）
    ///     NativeName: español (Estados Unidos)
    /// </summary>
    [LangMetadata("🇺🇸", "Spanish (United States)", "español (Estados Unidos)", "es_US")]
    es_US,

    /// <summary>
    ///     Emoji: 🇸🇪
    ///     EnglishName: Swedish
    ///     ChineseName: 瑞典语
    ///     NativeName: svenska
    /// </summary>
    [LangMetadata("🇸🇪", "Swedish", "svenska", "sv")]
    sv,

    /// <summary>
    ///     Emoji: 🇹🇭
    ///     EnglishName: Thai
    ///     ChineseName: 泰语
    ///     NativeName: ไทย
    /// </summary>
    [LangMetadata("🇹🇭", "Thai", "ไทย", "th")]
    th,

    /// <summary>
    ///     Emoji: 🇹🇷
    ///     EnglishName: Turkish
    ///     ChineseName: 土耳其语
    ///     NativeName: Türkçe
    /// </summary>
    [LangMetadata("🇹🇷", "Turkish", "Türkçe", "tr")]
    tr,

    /// <summary>
    ///     Emoji: 🇺🇦
    ///     EnglishName: Ukrainian
    ///     ChineseName: 乌克兰语
    ///     NativeName: українська
    /// </summary>
    [LangMetadata("🇺🇦", "Ukrainian", "українська", "uk")]
    uk,

    /// <summary>
    ///     Emoji: 🇻🇳
    ///     EnglishName: Vietnamese
    ///     ChineseName: 越南语
    ///     NativeName: Tiếng Việt
    /// </summary>
    [LangMetadata("🇻🇳", "Vietnamese", "Tiếng Việt", "vi")]
    vi
}
