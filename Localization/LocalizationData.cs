namespace Jan.Localization
{
    [System.Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class LocalizationContext
    {
        public string key;
        public LocalizationItem[] value;
    }

    [System.Serializable]
    public class LocalizationData
    {
        public LocalizationContext[] items;
    }
}