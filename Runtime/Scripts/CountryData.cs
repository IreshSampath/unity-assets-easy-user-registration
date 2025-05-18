namespace GAG.EasyUserRegistration
{
    [System.Serializable]
    public class CountryList
    {
        public Country[] Countries;
    }

    [System.Serializable]
    public class Country
    {
        public string Name;
        public string DialCode;
        public string Code;
    }
}
