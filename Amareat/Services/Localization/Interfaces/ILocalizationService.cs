using System;
namespace Amareat.Services.Localization.Interfaces
{
    public interface ILocalizationService
    {
        string GetResource(string key);

        string GetTwoLetterISOLanguageName();
    }
}
