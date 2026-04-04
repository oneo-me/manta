using System.Diagnostics.CodeAnalysis;

namespace Manta.Globalization.Core;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum LangType
{
    /// <summary>
    ///     Emoji: 🌐
    ///     EnglishName: Arabic
    ///     ChineseName: 阿拉伯语
    ///     NativeName: العربية
    /// </summary>
    ar,

    /// <summary>
    ///     Emoji: 🇪🇸
    ///     EnglishName: Catalan
    ///     ChineseName: 加泰罗尼亚语
    ///     NativeName: català
    /// </summary>
    ca,

    /// <summary>
    ///     Emoji: 🇭🇰
    ///     EnglishName: Chinese, Traditional (Hong Kong)
    ///     ChineseName: 繁体中文（香港）
    ///     NativeName: 繁體中文（香港）
    /// </summary>
    zh_HK,

    /// <summary>
    ///     Emoji: 🇨🇳
    ///     EnglishName: Chinese, Simplified (China mainland)
    ///     ChineseName: 简体中文
    ///     NativeName: 简体中文
    /// </summary>
    zh_Hans,

    /// <summary>
    ///     Emoji: 🇹🇼
    ///     EnglishName: Chinese, Traditional (Taiwan)
    ///     ChineseName: 繁体中文（台湾）
    ///     NativeName: 繁體中文（台灣）
    /// </summary>
    zh_Hant,

    /// <summary>
    ///     Emoji: 🇭🇷
    ///     EnglishName: Croatian
    ///     ChineseName: 克罗地亚语
    ///     NativeName: hrvatski
    /// </summary>
    hr,

    /// <summary>
    ///     Emoji: 🇨🇿
    ///     EnglishName: Czech
    ///     ChineseName: 捷克语
    ///     NativeName: čeština
    /// </summary>
    cs,

    /// <summary>
    ///     Emoji: 🇩🇰
    ///     EnglishName: Danish
    ///     ChineseName: 丹麦语
    ///     NativeName: dansk
    /// </summary>
    da,

    /// <summary>
    ///     Emoji: 🇳🇱
    ///     EnglishName: Dutch
    ///     ChineseName: 荷兰语
    ///     NativeName: Nederlands
    /// </summary>
    nl,

    /// <summary>
    ///     Emoji: 🇺🇸
    ///     EnglishName: English
    ///     ChineseName: 英语
    ///     NativeName: English
    /// </summary>
    en,

    /// <summary>
    ///     Emoji: 🇦🇺
    ///     EnglishName: English (Australia)
    ///     ChineseName: 英语（澳大利亚）
    ///     NativeName: English (Australia)
    /// </summary>
    en_AU,

    /// <summary>
    ///     Emoji: 🇮🇳
    ///     EnglishName: English (India)
    ///     ChineseName: 英语（印度）
    ///     NativeName: English (India)
    /// </summary>
    en_IN,

    /// <summary>
    ///     Emoji: 🇬🇧
    ///     EnglishName: English (United Kingdom)
    ///     ChineseName: 英语（英国）
    ///     NativeName: English (United Kingdom)
    /// </summary>
    en_GB,

    /// <summary>
    ///     Emoji: 🇫🇮
    ///     EnglishName: Finnish
    ///     ChineseName: 芬兰语
    ///     NativeName: suomi
    /// </summary>
    fi,

    /// <summary>
    ///     Emoji: 🇫🇷
    ///     EnglishName: French
    ///     ChineseName: 法语
    ///     NativeName: français
    /// </summary>
    fr,

    /// <summary>
    ///     Emoji: 🇨🇦
    ///     EnglishName: French (Canada)
    ///     ChineseName: 法语（加拿大）
    ///     NativeName: français (Canada)
    /// </summary>
    fr_CA,

    /// <summary>
    ///     Emoji: 🇩🇪
    ///     EnglishName: German
    ///     ChineseName: 德语
    ///     NativeName: Deutsch
    /// </summary>
    de,

    /// <summary>
    ///     Emoji: 🇬🇷
    ///     EnglishName: Greek
    ///     ChineseName: 希腊语
    ///     NativeName: Ελληνικά
    /// </summary>
    el,

    /// <summary>
    ///     Emoji: 🇮🇱
    ///     EnglishName: Hebrew
    ///     ChineseName: 希伯来语
    ///     NativeName: עברית
    /// </summary>
    he,

    /// <summary>
    ///     Emoji: 🇮🇳
    ///     EnglishName: Hindi
    ///     ChineseName: 印地语
    ///     NativeName: हिन्दी
    /// </summary>
    hi,

    /// <summary>
    ///     Emoji: 🇭🇺
    ///     EnglishName: Hungarian
    ///     ChineseName: 匈牙利语
    ///     NativeName: magyar
    /// </summary>
    hu,

    /// <summary>
    ///     Emoji: 🇮🇩
    ///     EnglishName: Indonesian
    ///     ChineseName: 印度尼西亚语
    ///     NativeName: Indonesia
    /// </summary>
    id,

    /// <summary>
    ///     Emoji: 🇮🇹
    ///     EnglishName: Italian
    ///     ChineseName: 意大利语
    ///     NativeName: italiano
    /// </summary>
    it,

    /// <summary>
    ///     Emoji: 🇯🇵
    ///     EnglishName: Japanese
    ///     ChineseName: 日语
    ///     NativeName: 日本語
    /// </summary>
    ja,

    /// <summary>
    ///     Emoji: 🇰🇷
    ///     EnglishName: Korean
    ///     ChineseName: 韩语
    ///     NativeName: 한국어
    /// </summary>
    ko,

    /// <summary>
    ///     Emoji: 🇲🇾
    ///     EnglishName: Malay
    ///     ChineseName: 马来语
    ///     NativeName: Bahasa Melayu
    /// </summary>
    ms,

    /// <summary>
    ///     Emoji: 🇳🇴
    ///     EnglishName: Norwegian Bokmål
    ///     ChineseName: 书面挪威语
    ///     NativeName: norsk bokmål
    /// </summary>
    nb,

    /// <summary>
    ///     Emoji: 🇵🇱
    ///     EnglishName: Polish
    ///     ChineseName: 波兰语
    ///     NativeName: polski
    /// </summary>
    pl,

    /// <summary>
    ///     Emoji: 🇧🇷
    ///     EnglishName: Portuguese (Brazil)
    ///     ChineseName: 葡萄牙语（巴西）
    ///     NativeName: português (Brasil)
    /// </summary>
    pt_BR,

    /// <summary>
    ///     Emoji: 🇵🇹
    ///     EnglishName: Portuguese (Portugal)
    ///     ChineseName: 葡萄牙语（葡萄牙）
    ///     NativeName: português (Portugal)
    /// </summary>
    pt_PT,

    /// <summary>
    ///     Emoji: 🇷🇴
    ///     EnglishName: Romanian
    ///     ChineseName: 罗马尼亚语
    ///     NativeName: română
    /// </summary>
    ro,

    /// <summary>
    ///     Emoji: 🇷🇺
    ///     EnglishName: Russian
    ///     ChineseName: 俄语
    ///     NativeName: русский
    /// </summary>
    ru,

    /// <summary>
    ///     Emoji: 🇸🇰
    ///     EnglishName: Slovak
    ///     ChineseName: 斯洛伐克语
    ///     NativeName: slovenčina
    /// </summary>
    sk,

    /// <summary>
    ///     Emoji: 🇸🇮
    ///     EnglishName: Slovenian
    ///     ChineseName: 斯洛文尼亚语
    ///     NativeName: slovenščina
    /// </summary>
    sl,

    /// <summary>
    ///     Emoji: 🇪🇸
    ///     EnglishName: Spanish
    ///     ChineseName: 西班牙语
    ///     NativeName: español
    /// </summary>
    es,

    /// <summary>
    ///     Emoji: 🌎
    ///     EnglishName: Spanish (Latin America)
    ///     ChineseName: 西班牙语（拉丁美洲）
    ///     NativeName: español (Latinoamérica)
    /// </summary>
    es_419,

    /// <summary>
    ///     Emoji: 🇺🇸
    ///     EnglishName: Spanish (United States)
    ///     ChineseName: 西班牙语（美国）
    ///     NativeName: español (Estados Unidos)
    /// </summary>
    es_US,

    /// <summary>
    ///     Emoji: 🇸🇪
    ///     EnglishName: Swedish
    ///     ChineseName: 瑞典语
    ///     NativeName: svenska
    /// </summary>
    sv,

    /// <summary>
    ///     Emoji: 🇹🇭
    ///     EnglishName: Thai
    ///     ChineseName: 泰语
    ///     NativeName: ไทย
    /// </summary>
    th,

    /// <summary>
    ///     Emoji: 🇹🇷
    ///     EnglishName: Turkish
    ///     ChineseName: 土耳其语
    ///     NativeName: Türkçe
    /// </summary>
    tr,

    /// <summary>
    ///     Emoji: 🇺🇦
    ///     EnglishName: Ukrainian
    ///     ChineseName: 乌克兰语
    ///     NativeName: українська
    /// </summary>
    uk,

    /// <summary>
    ///     Emoji: 🇻🇳
    ///     EnglishName: Vietnamese
    ///     ChineseName: 越南语
    ///     NativeName: Tiếng Việt
    /// </summary>
    vi
}
