# TeamsssoBackend
This is the sample project asp.net core enable sso microsoft teams tab app and exchange client token to access  token the backend project

following this video https://www.youtube.com/watch?v=kruUnaZgQaY&t=1826s and https://www.youtube.com/watch?v=SaBbfVgqZHc&t=1931s

# Configure Azure app registration
Navigate to the Azure portal and select the Azure AD service.

Select the App Registrations blade on the left, then select New registration.

In the Register an application page that appears, enter your application's registration information:
In the Name section, enter a meaningful application name that will be displayed to users of the app, for example teams-sso-sample.

Under Supported account types, select Accounts in any organizational directory.

In the Redirect URI (optional) section, select Single-page application in the combo-box and enter a redirect URI (e.g. https://contoso.ngrok.io/auth-end).

Select Register to create the application.

In the app's registration screen, find and note the Application (client) ID. You use this value in your app's configuration file(s) later in your code.

In the app's registration screen, select the Certificates & secrets blade in the left to open the page where we can generate secrets and upload certificates.

In the Client secrets section, select New client secret:
Type a key description (for instance app secret),

Select one of the available key durations (6 months, 12 months or Custom) as per your security posture.

The generated key value will be displayed when you select the Add button. Copy and save the generated value for use in later steps.

In the app's registration screen, select the API permissions blade in the left to open the page where we add access to the APIs that your application needs.

Select the Add a permission button and then,
Ensure that the Microsoft APIs tab is selected.

In the Commonly used Microsoft APIs section, select Microsoft Graph
In the Delegated permissions section, select openid, profile, email, offline_access and user.read in the list. Use the search box if necessary.

Select the Add permissions button at the bottom.

In the app's registration screen, select the Expose an API blade to the left to open the page where you can declare the parameters to expose this app as an API for which client applications can obtain access tokens for. The first thing that we need to do is to declare the unique resource URI that the clients will be using to obtain access tokens for this Api. To declare an resource URI, follow the following steps:
Select Set next to the Application ID URI to generate a URI that is unique for this app.

For this sample, edit the proposed Application ID URI (e.g. api://contoso.ngrok.io/{appId}) and selecting Save.

All APIs have to publish a minimum of one scope for the client's to obtain an access token successfully. To publish a scope, follow the following steps:
Select Add a scope button open the Add a scope screen and Enter the values as indicated below:
For Scope name, use access_as_user.

Select Admins and users options for Who can consent?.

For Admin consent display name type Access teams-sso-sample.

For Admin consent description type Allows the app to access teams-sso-sample as the signed-in user.

For User consent display name type Access teams-sso-sample.

For User consent description type Allow the application to access teams-sso-sample on your behalf.

Keep State as Enabled.

Select the Add scope button on the bottom to save this scope.

Still in the Expose an API blade, find the Authorized client applications section, follow the steps below: - Select Add a client application - In the panel that opens to the right, enter 1fec8e78-bce4-4aaf-ab1b-5451cc387264 - Check the Authorized scopes checkbox for the scope you've just exposed (e.g. access_as_user) - Select Add application to save. - In the Authorized client applications section, select Add a client application again. - In the panel that opens to the right, enter 5e3ce6c0-2b1f-4285-8d4b-75ee78787346 - Check the Authorized scopes checkbox for the scope you've just exposed (e.g. access_as_user) - Select Add application to save.
3. Update app configuration & run the web application

# Asp.net project configurations
Open the msteams-tabs-sso-sample.sln file in Visual Studio (or the project folder in Visual Studio Code). Modify the appsettings.json file as below:

Find the app key ClientId and replace the existing value with the application ID (clientId) of the Azure Application created earlier.

Find the app key TenantId and replace the existing value with your Azure AD tenant ID.

Find the app key Domain and replace the existing value with your Azure AD tenant name.

Find the app key ClientSecret and replace the existing value with the key you saved during the creation of the application, in the Azure portal.

Compile and run the application (e.g. press F5 in Visual Studio or type dotnet run in VS Code terminal)

Start Ngrok (or a similar tunneling tool), using the following command:

ngrok http https://localhost:44329/ -host-header="localhost:44329"
For the Redirect URI in the 'Authentication' page in your Azure AD Application, enter https://{yourNgrokDomain}.ngrok.io/auth/authPopup.

⚠️ Make sure the Redirect URI is of type Single-page application, and not Web.

# Manifest file configuratons
Update & package the Teams app manifest
Inside the src folder for this sample is a manifest.json file. The following needs to be changed in this file:

The "id" value must be populated with a new Guid value. You can do this in various ways depending on your platform of choice, but a simple PowerShell command is:
New-Guid

The {appId} values (near the bottom of the manifest) must be replaced with the Azure Application ID you generated in step 2 above, when generating the new Azure AD application.

The {ngrokSubdomain} value must be replaced with whatever ngrok subdomain you are using. If you are using another tunneling tool, you might need to replace the entire {ngrokSubdomain}.ngrok.io value, and when you create a Production version of your application you will similarly need to supply a complete production URL.

From within the src folder, in the command line, run the gulp command (you will need the gulp.js task runner installed to do this). This will generate a .zip manifest file that can be easily uploaded to Microsoft Teams.


# Upload the manifest to Teams
There are a few possible options to do this, depending on your development tools and platform. The easiest is simple to use Teams' App Studio tool, in particular the manifest editor tab which allows you to import a manifest (i.e. the one you created in step 4 above) and immediately install it.
