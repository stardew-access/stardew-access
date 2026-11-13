using CrossSpeak;
using stardew_access.Translation;
using stardew_access.Utils;
using StardewValley.Menus;
using System.Runtime.InteropServices;

namespace stardew_access.ScreenReader;

// TODO Declutter this class
public class ScreenReaderImpl : IScreenReader
{
    public string prevText = "", prevTextTile = "", prevChatText = "", prevMenuText = "";
    private string menuPrefixText = "";
    private string prevMenuPrefixText = "";
    private string menuSuffixText = "";
    private IScreenReadable? prevMenuElement = null;
    private string prevMenuSuffixText = "";
    private string prevMenuElementDescription = "";
    private string menuPrefixNoQueryText = "";
    private string menuSuffixNoQueryText = "";

    public string PrevTextTile
    {
        get => prevTextTile;
        set => prevTextTile = value;
    }

    public string PrevMenuQueryText
    {
        get => prevMenuText;
        set => prevMenuText = value;
    }

    public string MenuPrefixText
    {
        get => menuPrefixText;
        set => menuPrefixText = value;
    }

    public string MenuSuffixText
    {
        get => menuSuffixText;
        set => menuSuffixText = value;
    }

    public string MenuPrefixNoQueryText
    {
        get => menuPrefixNoQueryText;
        set => menuPrefixNoQueryText = value;
    }

    public string MenuSuffixNoQueryText
    {
        get => menuSuffixNoQueryText;
        set => menuSuffixNoQueryText = value;
    }

    public BoundedQueue<string> SpokenBuffer { get; set;} = new BoundedQueue<string>(size: 20, allowDuplicacy: false);

    public void InitializeScreenReader()
    {
        Log.Info("Initializing CrossSpeak...");
        CrossSpeakManager.Instance.TrySAPI(true);
        if (!CrossSpeakManager.Instance.Initialize())
        {
            Log.Error("None of the supported screen readers is running");
            return;
        }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Log.Info($"Setting Mac TTS speech rate to {MainClass.Config.MacSpeechRate}");
                CrossSpeakManager.Instance.SetRate(MainClass.Config.MacSpeechRate);
            }
        

        string name = CrossSpeakManager.Instance.DetectScreenReader();
        if (name != null)
        {
            Log.Info($"The active screen reader driver is: {name}");
        }
        else
        {
            Log.Info("Screen reader successfully initialized.");
        }
    }

    public void CloseScreenReader() => CrossSpeakManager.Instance.Close();

    public bool Say(string text, bool interrupt, bool excludeFromBuffer = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        if (!MainClass.Config.TTS) return false;
        if (text.Contains('^')) text = text.Replace('^', '\n');

        if (!excludeFromBuffer) SpokenBuffer.Add(text);

        if (!CrossSpeakManager.Instance.Output(text, interrupt))
        {
            Log.Error($"Failed to output text: {text}");
            return false;
        }

#if DEBUG
        Log.Verbose($"Speaking(interrupt: {interrupt}) = {text}");
#endif
        return true;
    }

    public bool TranslateAndSay(string translationKey, bool interrupt, object? translationTokens = null, TranslationCategory translationCategory = TranslationCategory.Default, bool disableTranslationWarnings = false, bool excludeFromBuffer = false)
    {
        if (string.IsNullOrWhiteSpace(translationKey))
            return false;

        return Say(Translator.Instance.Translate(translationKey, translationTokens, translationCategory, disableTranslationWarnings), interrupt, excludeFromBuffer: excludeFromBuffer);
    }

    public bool SayWithChecker(string text, bool interrupt, string? customQuery = null, bool excludeFromBuffer = false)
    {
        customQuery ??= text;

        if (string.IsNullOrWhiteSpace(customQuery))
            return false;

        if (prevText == customQuery)
            return false;

        prevText = customQuery;
        return Say(text, interrupt, excludeFromBuffer: excludeFromBuffer);
    }

    public bool TranslateAndSayWithChecker(string translationKey, bool interrupt, object? translationTokens = null, TranslationCategory translationCategory = TranslationCategory.Default, string? customQuery = null, bool disableTranslationWarnings = false, bool excludeFromBuffer = false)
    {
        if (string.IsNullOrWhiteSpace(translationKey))
            return false;

        return SayWithChecker(Translator.Instance.Translate(translationKey, translationTokens, translationCategory, disableTranslationWarnings), interrupt, customQuery, excludeFromBuffer: excludeFromBuffer);
    }

    public bool SayWithMenuChecker(string text, bool interrupt, string? customQuery = null, bool excludeFromBuffer = false)
    {
        customQuery ??= text;

        if (string.IsNullOrWhiteSpace(customQuery))
            return false;

        if (prevMenuText == customQuery && prevMenuSuffixText == MenuSuffixText && prevMenuPrefixText == MenuPrefixText)
            return false;

        prevMenuText = customQuery;
        prevMenuSuffixText = MenuSuffixText;
        prevMenuPrefixText = MenuPrefixText;
        bool re = Say($"{MenuPrefixNoQueryText}{MenuPrefixText}{text}{MenuSuffixText}{MenuSuffixNoQueryText}", interrupt, excludeFromBuffer: excludeFromBuffer);
        MenuPrefixNoQueryText = "";
        MenuSuffixNoQueryText = "";

        return re;
    }

    public bool TranslateAndSayWithMenuChecker(string translationKey, bool interrupt, object? translationTokens = null, TranslationCategory translationCategory = TranslationCategory.Menu, string? customQuery = null, bool disableTranslationWarnings = false, bool excludeFromBuffer = false)
    {
        if (string.IsNullOrWhiteSpace(translationKey))
            return false;

        return SayWithMenuChecker(Translator.Instance.Translate(translationKey, translationTokens, translationCategory, disableTranslationWarnings), interrupt, customQuery, excludeFromBuffer: excludeFromBuffer);
    }

    public bool SayMenuElement(IScreenReadable element, bool interrupt = true, bool excludeFromBuffer = false)
    {
        string text = element.ScreenReaderText;
        string description = element.ScreenReaderDescription;
        
        if (prevMenuElement != null && System.Object.ReferenceEquals(element, prevMenuElement) && prevMenuText == text && prevMenuSuffixText == MenuSuffixText && prevMenuPrefixText == MenuPrefixText)
            return false;

        prevMenuElement = element;
        prevMenuSuffixText = MenuSuffixText;
        prevMenuPrefixText = MenuPrefixText;
        bool re = Say($"{MenuPrefixNoQueryText}{MenuPrefixText}{text}\n{(prevMenuElementDescription != description || !System.Object.ReferenceEquals(element, prevMenuElement) ? description : "")}{MenuSuffixText}{MenuSuffixNoQueryText}", interrupt, excludeFromBuffer: excludeFromBuffer);
        MenuPrefixNoQueryText = "";
        MenuSuffixNoQueryText = "";
        prevMenuElementDescription = description;

        return re;
    }

    public bool SayMenuElement(string text, string description = "", bool interrupt = true, string? customQuery = null, bool excludeFromBuffer = false)
    {
        customQuery ??= text;

        if (prevMenuText == customQuery && prevMenuSuffixText == MenuSuffixText && prevMenuPrefixText == MenuPrefixText)
            return false;

        prevMenuText = customQuery;
        prevMenuSuffixText = MenuSuffixText;
        prevMenuPrefixText = MenuPrefixText;
        bool re = Say($"{MenuPrefixNoQueryText}{MenuPrefixText}{text}\n{(prevMenuElementDescription != description ? description : "")}{MenuSuffixText}{MenuSuffixNoQueryText}", interrupt, excludeFromBuffer: excludeFromBuffer);
        MenuPrefixNoQueryText = "";
        MenuSuffixNoQueryText = "";
        prevMenuElementDescription = description;
        
        return re;
    }

    public bool SayWithChatChecker(string text, bool interrupt, bool excludeFromBuffer = false)
    {
        if (prevChatText == text)
            return false;

        prevChatText = text;
        return Say(text, interrupt, excludeFromBuffer: excludeFromBuffer);
    }

    public bool SayWithTileQuery(string text, int x, int y, bool interrupt, bool excludeFromBuffer = false)
    {
        string query = $"{text} x:{x} y:{y}";

        if (prevTextTile == query)
            return false;

        prevTextTile = query;
        return Say(text, interrupt, excludeFromBuffer: excludeFromBuffer);
    }

    public void Cleanup()
    {
        MainClass.ScreenReader.PrevMenuQueryText = "";
        MainClass.ScreenReader.MenuPrefixText = "";
        MainClass.ScreenReader.MenuSuffixText = "";
        prevMenuElementDescription = "";
    }
}
