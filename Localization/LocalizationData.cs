namespace Jan.Localizaton
{
    [System.Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class LocalizationClass
    {
        public LocalizationItem[] chloeOrder1;
    }

    [System.Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] items;
    }
}