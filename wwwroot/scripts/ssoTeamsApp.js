(function () {
    'use strict';

    // 1. Get teams Token 
    function getClientSideToken() {

        return new Promise((resolve, reject) => {

            display("1. Get auth token from Microsoft Teams");
            microsoftTeams.authentication.getAuthToken({
                successCallback: (result) => {
                    display(result);
                    let decodedToken = jwt_decode(result);
                    display("client side token to server for exchange the client token to On Behalf Access Token.");
                    resolve(result);
                },
                failureCallback: function (error) {
                    reject("Error getting token: " + error);
                }
            });

        });

    }

             //Request Backend for exchange token and call ms graph
    function getUserDataFromServer(clientSideToken) {

        display("2. Exchange client token to access token and request user details from ms graph.");

        return new Promise((resolve, reject) => {
          
            microsoftTeams.getContext((context) => {
                const headers = new Headers();
                const bearer = `Bearer ${clientSideToken}`;

                headers.append("Authorization", bearer);
                const options = {
                    method: "GET",
                    headers: headers
                };
                fetch('/api/ApiAuth/auth/token', options)
                    .then((response) => {
                        if (response.ok) {
                            return response.text();
                        } else {
                            reject(response.error);
                        }
                    })
                    .then((responseJson) => {
                        if (responseJson.error) {
                            reject(responseJson.error);
                        }
                        else if ("unauthorized_client" === responseJson || "invalid_grant" === responseJson) {
                            reject(responseJson);
                       } else {
                           console.log(responseJson);
                           const serverSideToken = responseJson.split(',');
                           serverSideToken.map(x=>display(x));
                           resolve(serverSideToken);
                        }
                    });
            });
        });
    }

    // Show the consent pop-up
    function requestConsent() {
        return new Promise((resolve, reject) => {
            microsoftTeams.authentication.authenticate({
                url: window.location.origin + "/auth/authPopup",
                width: 600,
                height: 535,
                successCallback: (result) => {
                    resolve(result);
                },
                failureCallback: (reason) => {
                    reject(JSON.stringify(reason));
                }
            });
        });
    }

    function display(text, elementTag) {
        var logDiv = document.getElementById('logs');
        var newElement = document.createElement(elementTag ? elementTag : "p");
        newElement.innerText = text;
        logDiv.append(newElement);
        console.log("ssoDemo: " + text);
        return newElement;
    }

    microsoftTeams.initialize();

    // inline 
    getClientSideToken()
        .then((clientSideToken) => {
            return getUserDataFromServer(clientSideToken);
        })
        .catch((error) => {
            if ("unauthorized_client" === error || "invalid_grant" === error) {
                display(`Error: ${error} - user or admin consent required`);
                let button = display("Consent", "button");
                button.onclick = (() => {
                    requestConsent()
                        .then((result) => {
                            if (result) {
                                display(`Received access token ${result.accessToken}`);
                                window.location.reload();
                            }
                        })
                        .catch((error) => {
                            display(`ERROR ${error}`);
                            button.disabled = true;
                            let refreshButton = display("Refresh page", "button");
                            refreshButton.onclick = (() => { window.location.reload(); });
                        });
                });
            } else {
                display(`Error from web service: ${error}`);
            }
        });

    microsoftTeams.getContext(function (context) {
        setTheme(context.theme);
    });


    microsoftTeams.registerOnThemeChangeHandler(function (theme) {
        setTheme(theme);
    });

    function setTheme(theme) {
        if (theme) {
            document.body.className = 'theme-' + (theme === 'default' ? 'light' : theme);
        }
    }

})();
