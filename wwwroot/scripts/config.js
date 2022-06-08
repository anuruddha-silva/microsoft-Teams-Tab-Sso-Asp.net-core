(function () {

    // Save configuration changes
    microsoftTeams.settings.registerOnSaveHandler(function (saveEvent) {

        var tabUrl = window.location.protocol +
            '//' + window.location.host + '/ssoTeamsApp';


        microsoftTeams.settings.setSettings({
            contentUrl: tabUrl, 
            entityId: tabUrl    
        });


        saveEvent.notifySuccess();
    });

    microsoftTeams.settings.setValidityState(true);

})();