namespace GAG.EasyUserRegistration
{
    [System.Serializable]
    public class UserList
    {
        public User[] Users;
    }

    [System.Serializable]
    public class User
    {
        public string Id;
        public string FirstName;
        public string LastName;
        public string Username;
        public string Gender;
        public string DateOfBirth;
        public string Email;
        public string PhoneNumber;
        public string Address;
        public string City;
        public string Country;
        public string Password;
        public string ConfirmPassword;
        public string Note;
        public string ProfilePicture;
    }
}

