using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using Amareat.Helpers;
using Amareat.Services.Localization.Interfaces;

namespace Amareat.Services.Localization.Implementations
{
    public class LocalizationService : ILocalizationService
    {
        static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() => new ResourceManager(ConstantGlobal.ResourcesPath, typeof(LocalizationService).GetTypeInfo().Assembly));

        public LocalizationService()
        {
        }

        public string GetResource(string key)
        {
            var ci = Thread.CurrentThread.CurrentUICulture;
            string translation = key;

            try
            {
                translation = resmgr.Value.GetString(key, ci);

                if (translation == null)
                    translation = key;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LocalizationService: GetResource Exception {ex.Message}");
            }

            return translation;
        }

        public string GetTwoLetterISOLanguageName()
        {
            var currentCulture = CultureInfo.InstalledUICulture;
            return currentCulture.TwoLetterISOLanguageName;
        }
    }
}
