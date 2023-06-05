# AWS File Uploader

AWS File Uploader is a WPF application that allows you to upload files to Amazon S3 (Simple Storage Service) using the AWS SDK. It follows the MVVM (Model-View-ViewModel) architectural pattern to separate concerns and provide a clean separation of the user interface and business logic.

## Features

- Upload files to Amazon S3 buckets.
- Supports encryption of sensitive data using the AES (Advanced Encryption Standard) algorithm.
- User-friendly interface with progress tracking for file uploads.
- Ability to save AWS credentials locally for convenience.

## Technologies Used

- **WPF (Windows Presentation Foundation):** The application is built using WPF, a UI framework provided by Microsoft, which allows for the creation of visually appealing and interactive desktop applications.

- **MVVM (Model-View-ViewModel):** The project follows the MVVM architectural pattern, which separates the user interface (View) from the underlying data and logic (ViewModel). This separation enhances maintainability and testability of the application.

- **AWS SDK (Software Development Kit):** The AWS SDK is utilized to interact with the Amazon S3 service. It provides a set of APIs and tools to simplify the integration of AWS services into applications.

- **Amazon S3 (Simple Storage Service):** Amazon S3 is a scalable and secure object storage service provided by Amazon Web Services. It allows for storing and retrieving large amounts of data, making it ideal for file storage and distribution.

## Encryption

The application uses the AES (Advanced Encryption Standard) algorithm to encrypt sensitive data, such as the AWS Access Key and Secret Access Key. The `EncryptData` method in the codebase demonstrates the encryption process. It encrypts the input data using AES and returns the encrypted data as a byte array. The specific implementation details of AES encryption, such as the encryption key and mode of operation, are not provided in the code snippet.

## Usage

1. Launch the application.
2. Select the desired region from the dropdown menu.
3. Enter the AWS Access Key and Secret Access Key in the respective fields.
4. Click the "Check Connection" button to verify the connection to the AWS service.
5. Specify the name of the Amazon S3 bucket where you want to upload the file. Optionally, you can include a folder name by appending it to the file name (e.g., "BucketName/FolderName").
6. Click the "Browse" button to select the file you want to upload.
7. (Optional) Check the "Save Credentials" checkbox to save the AWS credentials locally for future use.
8. Click the "Upload File" button to initiate the file upload process.
9. The application will display the progress of the file upload in the progress bar.
10. Once the upload is complete, a success message will be shown.

Note: Ensure that you have a valid AWS account and proper permissions to access the specified S3 bucket.

## Configuration

The application uses a configuration file to store the AWS credentials. The access keys are encrypted using the AES algorithm to enhance security. The encrypted access keys are then saved to the config files, allowing for secure storage of the credentials.

## Conclusion

The AWS File Uploader is a powerful and secure desktop application that simplifies the process of uploading files to Amazon S3. It leverages the AWS SDK and follows the MVVM pattern to provide a user-friendly interface and efficient file upload functionality. Whether you are a developer or a non-technical user, this application makes it easy to upload files to Amazon S3 with just a few clicks.
