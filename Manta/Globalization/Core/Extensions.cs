namespace Manta.Globalization.Core;

public static class Extensions
{
    public static LangMetadata GetMetadata(this LangType langType)
    {
        return langType switch
        {
            LangType.ar      => new(langType, "🌐", "Arabic", "العربية", "ar"),
            LangType.ca      => new(langType, "🇪🇸", "Catalan", "català", "ca"),
            LangType.zh_HK   => new(langType, "🇭🇰", "Chinese, Traditional (Hong Kong)", "繁體中文（香港）", "zh_HK"),
            LangType.zh_Hans => new(langType, "🇨🇳", "Chinese, Simplified (China mainland)", "简体中文", "zh_Hans"),
            LangType.zh_Hant => new(langType, "🇹🇼", "Chinese, Traditional (Taiwan)", "繁體中文（台灣）", "zh_Hant"),
            LangType.hr      => new(langType, "🇭🇷", "Croatian", "hrvatski", "hr"),
            LangType.cs      => new(langType, "🇨🇿", "Czech", "čeština", "cs"),
            LangType.da      => new(langType, "🇩🇰", "Danish", "dansk", "da"),
            LangType.nl      => new(langType, "🇳🇱", "Dutch", "Nederlands", "nl"),
            LangType.en      => new(langType, "🇺🇸", "English", "English", "en"),
            LangType.en_AU   => new(langType, "🇦🇺", "English (Australia)", "English (Australia)", "en_AU"),
            LangType.en_IN   => new(langType, "🇮🇳", "English (India)", "English (India)", "en_IN"),
            LangType.en_GB   => new(langType, "🇬🇧", "English (United Kingdom)", "English (United Kingdom)", "en_GB"),
            LangType.fi      => new(langType, "🇫🇮", "Finnish", "suomi", "fi"),
            LangType.fr      => new(langType, "🇫🇷", "French", "français", "fr"),
            LangType.fr_CA   => new(langType, "🇨🇦", "French (Canada)", "français (Canada)", "fr_CA"),
            LangType.de      => new(langType, "🇩🇪", "German", "Deutsch", "de"),
            LangType.el      => new(langType, "🇬🇷", "Greek", "Ελληνικά", "el"),
            LangType.he      => new(langType, "🇮🇱", "Hebrew", "עברית", "he"),
            LangType.hi      => new(langType, "🇮🇳", "Hindi", "हिन्दी", "hi"),
            LangType.hu      => new(langType, "🇭🇺", "Hungarian", "magyar", "hu"),
            LangType.id      => new(langType, "🇮🇩", "Indonesian", "Indonesia", "id"),
            LangType.it      => new(langType, "🇮🇹", "Italian", "italiano", "it"),
            LangType.ja      => new(langType, "🇯🇵", "Japanese", "日本語", "ja"),
            LangType.ko      => new(langType, "🇰🇷", "Korean", "한국어", "ko"),
            LangType.ms      => new(langType, "🇲🇾", "Malay", "Bahasa Melayu", "ms"),
            LangType.nb      => new(langType, "🇳🇴", "Norwegian Bokmål", "norsk bokmål", "nb"),
            LangType.pl      => new(langType, "🇵🇱", "Polish", "polski", "pl"),
            LangType.pt_BR   => new(langType, "🇧🇷", "Portuguese (Brazil)", "português (Brasil)", "pt_BR"),
            LangType.pt_PT   => new(langType, "🇵🇹", "Portuguese (Portugal)", "português (Portugal)", "pt_PT"),
            LangType.ro      => new(langType, "🇷🇴", "Romanian", "română", "ro"),
            LangType.ru      => new(langType, "🇷🇺", "Russian", "русский", "ru"),
            LangType.sk      => new(langType, "🇸🇰", "Slovak", "slovenčina", "sk"),
            LangType.sl      => new(langType, "🇸🇮", "Slovenian", "slovenščina", "sl"),
            LangType.es      => new(langType, "🇪🇸", "Spanish", "español", "es"),
            LangType.es_419  => new(langType, "🌎", "Spanish (Latin America)", "español (Latinoamérica)", "es_419"),
            LangType.es_US   => new(langType, "🇺🇸", "Spanish (United States)", "español (Estados Unidos)", "es_US"),
            LangType.sv      => new(langType, "🇸🇪", "Swedish", "svenska", "sv"),
            LangType.th      => new(langType, "🇹🇭", "Thai", "ไทย", "th"),
            LangType.tr      => new(langType, "🇹🇷", "Turkish", "Türkçe", "tr"),
            LangType.uk      => new(langType, "🇺🇦", "Ukrainian", "українська", "uk"),
            LangType.vi      => new(langType, "🇻🇳", "Vietnamese", "Tiếng Việt", "vi"),
            _                => throw new ArgumentOutOfRangeException(nameof(langType), langType, null)
        };
    }
}
