using Amazon;
using Amazon.S3.Transfer;
using AWSFileUploader.Models;
using AWSFileUploader.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AWSFileUploader.ViewModels
{
    public class AWSBucketUploadViewModel : INotifyPropertyChanged
    {
        private string _region;
        private string _accessKey;
        private string _secretAccessKey;
        private string _bucketName;
        private string _filePath;
        private bool _connectionStatus;
        private List<string> _regionOptions;
        private string _selectedRegion;
        private bool _isUploading;
        private int _uploadProgress;
        private Settings settingToSave;
        private bool _saveDataLocally;

        public AWSBucketUploadViewModel()
        {
            // Populate region options
            _regionOptions = new List<string>(RegionEndpoint.EnumerableAllRegions
                .Select(r => r.SystemName));
            // Initialize commands
            CheckConnectionCommand = new RelayCommand(CheckConnection, CanCheckConnection);
            UploadFileCommand = new RelayCommand(UploadFile, CanUploadFile);
            BrowseFileCommand = new RelayCommand(BrowseFile);

            // Load saved credentials
            LoadSavedCredentials();
        }

        public bool SaveDataLocally
        {
            get { return _saveDataLocally; }
            set
            {
                if (_saveDataLocally != value)
                {
                    _saveDataLocally = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> RegionOptions
        {
            get { return _regionOptions; }
            set
            {
                _regionOptions = value;
                OnPropertyChanged();
            }
        }

        public string SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                _selectedRegion = value;
                OnPropertyChanged();
            }
        }

        public string AccessKey
        {
            get { return _accessKey; }
            set
            {
                _accessKey = value;
                OnPropertyChanged();
            }
        }

        public string SecretAccessKey
        {
            get { return _secretAccessKey; }
            set
            {
                _secretAccessKey = value;
                OnPropertyChanged();
            }
        }

        public string BucketName
        {
            get { return _bucketName; }
            set
            {
                _bucketName = value;
                OnPropertyChanged();
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public bool ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        public bool IsUploading
        {
            get { return _isUploading; }
            set
            {
                _isUploading = value;
                OnPropertyChanged();
            }
        }

        public int UploadProgress
        {
            get { return _uploadProgress; }
            set
            {
                _uploadProgress = value;
                OnPropertyChanged();
            }
        }

        public ICommand CheckConnectionCommand { get; set; }
        public ICommand UploadFileCommand { get; set; }
        public ICommand BrowseFileCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SaveCredentials()
        {
            if (!SaveDataLocally)
            {
                return;
            }
            settingToSave = new Settings
            {
                AccessKey = _accessKey,
                SecretAccessKey = _secretAccessKey,
                Region = _selectedRegion,
                BucketName = _bucketName ?? string.Empty
            };
            SettingsHelper.SaveSettings(settingToSave);
        }

        private void CheckConnection(object parameter)
        {
            AwsService awsService = new AwsService(_selectedRegion, _accessKey, _secretAccessKey);
            ConnectionStatus = awsService.CheckConnection();

            if (ConnectionStatus && SaveDataLocally)
            {
                SaveCredentials();
            }
        }

        private bool CanCheckConnection(object parameter)
        {
            return !string.IsNullOrEmpty(_accessKey) && !string.IsNullOrEmpty(_secretAccessKey) && !string.IsNullOrEmpty(_selectedRegion);
        }

        private async void UploadFile(object parameter)
        {
            AwsService awsService = new AwsService(_selectedRegion, _accessKey, _secretAccessKey);
            IsUploading = true;

            await Task.Run(() =>
            {
                SaveCredentials();

                awsService.UploadFile(_bucketName, _filePath, OnUploadProgressUpdated);
            });

            IsUploading = false;
        }

        private void OnUploadProgressUpdated(object sender, UploadProgressArgs e)
        {
            Console.WriteLine(e.PercentDone.ToString());
            UploadProgress = e.PercentDone;
        }

        private bool CanUploadFile(object parameter)
        {
            return !string.IsNullOrEmpty(_bucketName) && !string.IsNullOrEmpty(_filePath);
        }

        private void BrowseFile(object parameter)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        private void LoadSavedCredentials()
        {
            settingToSave = SettingsHelper.LoadSettings();
            if (settingToSave != null)
            {
                AccessKey = settingToSave.AccessKey;
                SecretAccessKey = settingToSave.SecretAccessKey;
                BucketName = settingToSave.BucketName;
                SelectedRegion = settingToSave.Region;
            }
        }

    }
}
