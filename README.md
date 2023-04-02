# Azure.KeyVault.Reader
Library for getting secrets from KeyVault
This library allows you to retrieve secrets from the KeyVault service and use them in your application. 
With it, you can easily and securely store confidential data, such as passwords, keys, and tokens.

How to use the library
To use the library, follow these steps:

Add a reference to the library in your project.
Initialize the library by calling the ConfigurationProvider.Init() method.
Get the desired secret by calling the AzureSecretProvider.GetSecretByName(name) method.


How to locally compile the project

In the AppSettings file, create an Azure key and set its value to false. This will enable you to compile the project locally.
