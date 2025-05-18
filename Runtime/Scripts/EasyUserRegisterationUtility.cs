using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Text;

namespace GAG.EasyUserRegistration
{
    public class EasyUserRegistrationUtility
    {
        public enum SaveDataType { JSON, CSV }
        public enum DeployPlatform { PC, Android, IOS }

        public static void SaveUser(User user, string path, SaveDataType type, bool allowDuplicates)
        {
            switch (type)
            {
                case SaveDataType.JSON:
                    SaveAsJSON(user, path);
                    break;
                case SaveDataType.CSV:
                    SaveAsCSV(user, path);
                    break;
            }
        }

        private static void SaveAsJSON(User user, string path)
        {
            try
            {
                UserList userList = new UserList();
                List<User> users = new List<User>();

                // Check if the file exists.
                if (File.Exists(path))
                {
                    // Read the existing JSON data from the file.
                    string existingJsonData = File.ReadAllText(path);

                    // Try to deserialize the JSON data into a UserList object.
                    userList = JsonUtility.FromJson<UserList>(existingJsonData);
                    if (userList != null && userList.Users != null)
                    {
                        users.AddRange(userList.Users);
                    }
                }
                else
                {
                    userList = new UserList();
                }

                // Add the new user to the list.
                users.Add(user);
                userList.Users = users.ToArray();

                // Serialize the UserList object to JSON.
                string jsonData = JsonUtility.ToJson(userList, true);

                // Write the JSON data to the file.
                File.WriteAllText(path, jsonData);

                // Use Debug.Log to confirm that the data has been saved, and where.
                Debug.Log("Saved JSON User to: " + path);
            }
            catch (System.Exception e)
            {
                // Handle any errors that might occur during the process.
                Debug.LogError("Error saving JSON: " + e.Message);
            }
        }

        private static void SaveAsCSV(User user, string path)
        {
            try
            {
                List<User> users = new List<User>();
                if (File.Exists(path))
                {
                    users = LoadFromCSV(path); // Load existing users
                }

                users.Add(user); // Add the new user

                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Id,FirstName,LastName,Username,Gender,DateOfBirth,Email,PhoneNumber,Address,City,Country,Password,ConfirmPassword,Note,ProfilePicture"); //header

                foreach (User u in users)
                {
                    string line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
                        u.Id, u.FirstName, u.LastName, u.Username, u.Gender, u.DateOfBirth, u.Email, u.PhoneNumber, u.Address, u.City, u.Country, u.Password, u.ConfirmPassword, u.Note, u.ProfilePicture);
                    csvContent.AppendLine(line);
                }
                File.WriteAllText(path, csvContent.ToString());
                Debug.Log("Saved CSV User to: " + path);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error saving CSV: " + e.Message);
            }
        }

        static List<User> LoadFromCSV(string path)
        {
            List<User> users = new List<User>();
            string[] lines = File.ReadAllLines(path);

            if (lines.Length > 1) // Skip header line
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(',');
                    if (data.Length == 15) // Ensure all fields are present
                    {
                        User user = new User
                        {
                            Id = data[0],
                            FirstName = data[1],
                            LastName = data[2],
                            Username = data[3],
                            Gender = data[4],
                            DateOfBirth = data[5],
                            Email = data[6],
                            PhoneNumber = data[7],
                            Address = data[8],
                            City = data[9],
                            Country = data[10],
                            Password = data[11],
                            ConfirmPassword = data[12],
                            Note = data[13],
                            ProfilePicture = data[14]
                        };
                        users.Add(user);
                    }
                    else
                    {
                        Debug.LogWarning("Skipping invalid CSV line: " + lines[i]);
                    }
                }
            }
            return users;
        }

    }
}
