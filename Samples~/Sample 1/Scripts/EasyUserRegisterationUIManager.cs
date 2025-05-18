using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GAG.EasyUserRegistration
{
    public enum Gender
    {
        Male,
        Female,
    }

    public class EasyUserRegistrationUIManager : MonoBehaviour
    {
        [Header("User Registration UI")]
        [SerializeField] TMP_Text _messageText;

        [SerializeField] TMP_InputField _firstName;
        [SerializeField] TMP_InputField _lastName;

        [SerializeField] TMP_InputField _username;
        [SerializeField] TMP_Dropdown _genderDropDown;
        string _genderSelected;

        [SerializeField] TMP_Dropdown _dayDropdown;
        [SerializeField] TMP_Dropdown _monthDropdown;
        [SerializeField] TMP_Dropdown _yearDropdown;
        string _dateOfBirthSelected;

        [SerializeField] TMP_InputField _email;

        [SerializeField] TMP_Dropdown _countryDialCodesDropDown;
        List<string> _countryDialCode = new List<string>();
        string _countryDialCodeSelected;
        [SerializeField] TMP_InputField _phoneNumber;

        [SerializeField] TMP_InputField _address;
        [SerializeField] TMP_InputField _city;
        [SerializeField] TMP_InputField _country;
        [SerializeField] TMP_InputField _password;
        [SerializeField] TMP_InputField _confirmPassword;

        [SerializeField] TMP_InputField _note;

        [SerializeField] Image _userImage;
        string _imagePath;


        bool _isNullEnable = true;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            PopulateGenderOptions();
            PopulateDayOptions();
            PopulateMonthOptions();
            PopulateYearOptions();
            LoadAndSortCountryDialCodes();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnDateOfBirthChanged()
        {
            string selectedDay = _dayDropdown.options[_dayDropdown.value].text;
            //string selectedMonth = _monthDropdown.options[_monthDropdown.value].text;
            string selectedMonth = (_monthDropdown.value + 1).ToString();
            string selectedYear = _yearDropdown.options[_yearDropdown.value].text;
            _dateOfBirthSelected = $"{selectedDay} {selectedMonth} {selectedYear}";
            print(_dateOfBirthSelected);
        }

        public void OnGenderChanged()
        {
            Gender selectedGender = (Gender)_genderDropDown.value;
            _genderSelected = selectedGender.ToString();
            print(_genderSelected);
        }

        public void OnCountryDialCodeChanged()
        {
            int index = _countryDialCodesDropDown.value;
            print(_countryDialCodesDropDown.options[index].text);
            string[] countryDialCode = _countryDialCodesDropDown.options[index].text.Split('(');
            string[] countryDialCode2 = countryDialCode[1].Split(')');

            _countryDialCodeSelected = countryDialCode2[0];
            print(_countryDialCodeSelected);

        }

        public void OpenFileWindow()
        {
            _messageText.text = "<color=yellow>This feature will be available soon.</color>";
        }

        public void SublitUserData()
        {
            if (!IsValidEmail(_email.text))
            {
                _messageText.text = "<color=red>Invalid email format.</color>";
                return;
            }

            if (!IsValidPassword(_password.text))
            {
                _messageText.text = "<color=red>Password must be at least 6 characters and contain a letter and a number.</color>";
                return;
            }

            if (_password.text != _confirmPassword.text)
            {
                _messageText.text = "<color=red>Password and Confirm Password do not match.</color>";
                return;
            }

            User user = new User();
            user.Id = System.Guid.NewGuid().ToString();

            user.FirstName = ToNullIfEmpty(_firstName.text);
            user.LastName = ToNullIfEmpty(_lastName.text);
            user.Username = ToNullIfEmpty(_username.text);
            user.Gender = ToNullIfEmpty(_genderSelected);
            user.DateOfBirth = ToNullIfEmpty(_dateOfBirthSelected);
            user.Email = ToNullIfEmpty(_email.text);
            user.PhoneNumber = ToNullIfEmpty(_countryDialCodeSelected + _phoneNumber.text);
            user.Address = ToNullIfEmpty(_address.text);
            user.City = ToNullIfEmpty(_city.text);
            user.Country = ToNullIfEmpty(_country.text);
            user.Password = ToNullIfEmpty(_password.text);
            user.ConfirmPassword = ToNullIfEmpty(_confirmPassword.text);
            user.Note = ToNullIfEmpty(_note.text);
            user.ProfilePicture = ToNullIfEmpty(_imagePath);

            _messageText.text = "<color=green>User data submitted successfully.</color>";

            EasyUserRegistrationEvents.RaiseOnUserDataEntered(user);
        }

        string ToNullIfEmpty(string input)
        {
            if (_isNullEnable)
            {
                return string.IsNullOrWhiteSpace(input) ? null : input;
            }
            else
            {
                return string.IsNullOrWhiteSpace(input) ? "" : input;
            }
        }

        bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                // Simple and reliable regex
                return System.Text.RegularExpressions.Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            return System.Text.RegularExpressions.Regex.IsMatch(
                password,
                @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"); // At least 6 chars, 1 letter, 1 digit
        }

        void PopulateGenderOptions()
        {
            _genderDropDown.ClearOptions();

            var genderOptions = System.Enum.GetNames(typeof(Gender)).ToList();

            _genderDropDown.AddOptions(genderOptions);

            _genderDropDown.value = genderOptions.IndexOf(Gender.Male.ToString());
            _genderDropDown.RefreshShownValue();
        }

        void PopulateDayOptions()
        {
            _dayDropdown.ClearOptions();
            for (int day = 1; day <= 31; day++)
            {
                _dayDropdown.options.Add(new TMP_Dropdown.OptionData(day.ToString()));
            }
            _dayDropdown.value = 0;
            _dayDropdown.RefreshShownValue();
        }

        void PopulateMonthOptions()
        {
            _monthDropdown.ClearOptions();
            string[] months = new string[]
            {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
            };
            foreach (string month in months)
            {
                _monthDropdown.options.Add(new TMP_Dropdown.OptionData(month));
            }
            _monthDropdown.value = 0;
            _monthDropdown.RefreshShownValue();
        }

        void PopulateYearOptions()
        {
            _yearDropdown.ClearOptions();
            int currentYear = System.DateTime.Now.Year;
            for (int year = currentYear; year >= 1900; year--)
            {
                _yearDropdown.options.Add(new TMP_Dropdown.OptionData(year.ToString()));
            }
            _yearDropdown.value = 0;
            _yearDropdown.RefreshShownValue();
        }

        void LoadAndSortCountryDialCodes()
        {
            TextAsset json = Resources.Load<TextAsset>("CountryCodes");
            CountryList countryList = JsonUtility.FromJson<CountryList>("{\"Countries\":" + json.text + "}");

            //-----
            // Sort Name + DialCode
            //foreach (var c in countryList.Countries)
            //{
            //    string tmp = $"{c.Name} ({c.DialCode})";
            //    _countryDialCode.Add(tmp);
            //}

            //-----

            // Sort Code + DialCode
            var sortedCountries = countryList.Countries.OrderBy(c => c.Code).ToList();
            foreach (var country in sortedCountries)
            {
                string item = $"{country.Code} ({country.DialCode})";
                _countryDialCode.Add(item);
            }
            //string tmp = $"{c.Code} ({c.DialCode})";
            //_countryDialCode.Add(tmp);


            //-----

            //var sortedCountries = countryList.Countries.OrderBy(c => c.DialCode).ToList();
            //-----
            //var sortedCountries = countryList.Countries
            //.OrderBy(c => int.Parse(c.DialCode.TrimStart('+')))
            //.ToList();

            //HashSet<string> uniqueDialCodes = new HashSet<string>();

            //foreach (var c in sortedCountries)
            //{
            //    if (uniqueDialCodes.Add(c.DialCode)) // adds only if not already in the set
            //    {
            //        _countryDialCode.Add(c.DialCode);
            //    }
            //}
            //-----
            //var uniqueCountries = countryList.Countries
            //.GroupBy(c => c.DialCode)
            //.Select(g => g.First())
            //.OrderBy(c => int.Parse(c.DialCode.TrimStart('+')))
            //.ToList();

            //foreach (var c in uniqueCountries)
            //{
            //    _countryDialCode.Add(c.DialCode);
            //}
            //-----

            PopulateCountryDialCodeOptions();
        }

        void PopulateCountryDialCodeOptions()
        {
            _countryDialCodesDropDown.ClearOptions();
            _countryDialCodesDropDown.AddOptions(_countryDialCode);

            int defaultIndex = _countryDialCode.IndexOf("AE (+971)");
            //int defaultIndex = _countryDialCode.IndexOf("United Arab Emirates (+971)");
            //int defaultIndex = _countryDialCode.IndexOf("+971");

            if (defaultIndex >= 0)
            {
                _countryDialCodesDropDown.value = defaultIndex;
                _countryDialCodesDropDown.RefreshShownValue();
            }

        }
        
        void LoadUserImage()
        {
            // Load the image from the Resources folder
            Texture2D texture = Resources.Load<Texture2D>("UserImage");
            // Check if the texture was loaded successfully
            if (texture != null)
            {
                // Create a new Sprite from the texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                // Assign the sprite to the Image component
                _userImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("Failed to load user image.");
            }
        }
    }
}
