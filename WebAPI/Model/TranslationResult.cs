namespace WebAPI.Model;

// مدل برای درخواست ترجمه
public class TranslationRequest
{
    public string OriginalText { get; set; }
    public string InputLanguage { get; set; }
    public string OutputLanguage { get; set; }
}

// مدل برای پاسخ ترجمه
public class TranslationResponse
{
    public List<string> Translations { get; set; }
}
