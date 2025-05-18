using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static GAG.EasyUserRegistration.EasyUserRegistrationUtility;

namespace GAG.EasyUserRegistration
{
    public class EasyUserRegisterationManager : MonoBehaviour
    {
        //public enum RegistrationType
        //{
        //    Basic,
        //    Advanced
        //}
        //public enum RegistrationStatus
        //{
        //    NotStarted,
        //    InProgress,
        //    Completed,
        //    Failed
        //}
        [SerializeField] SaveDataType _saveDataType = SaveDataType.JSON;
        [SerializeField] DeployPlatform _deployPlatform = DeployPlatform.PC;

        private void OnEnable()
        {
            EasyUserRegistrationEvents.OnUserDataEntered += OnUserDataEntered;
        }
        private void OnDisable()
        {
            EasyUserRegistrationEvents.OnUserDataEntered -= OnUserDataEntered;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnUserDataEntered(User user)
        {
            SaveUser(user, GetSavePath(), _saveDataType, false);
        }

        string GetSavePath()
        {
            string fileName = "User" + (_saveDataType == SaveDataType.JSON ? ".json" : ".csv");

            string basePath = _deployPlatform switch
            {
                DeployPlatform.PC => Application.streamingAssetsPath,
                DeployPlatform.Android => Application.persistentDataPath,
                DeployPlatform.IOS => Application.persistentDataPath,
                _ => Application.persistentDataPath
            };

            return Path.Combine(basePath, fileName);
        }
    }
}