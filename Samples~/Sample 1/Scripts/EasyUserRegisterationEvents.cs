using System;
using System.Collections.Generic;

namespace GAG.EasyUserRegistration
{
    public class EasyUserRegistrationEvents 
    {
        public static event Action<User> OnUserDataEntered;
        public static void RaiseOnUserDataEntered(User user)
        {
            OnUserDataEntered?.Invoke(user);
        }
    }
}
