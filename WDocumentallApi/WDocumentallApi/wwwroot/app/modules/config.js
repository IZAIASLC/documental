(function (app) {
    'use strict';

    angular.module('app').constant('SETTINGS', {
        'VERSION': '0.0.1',
        'CURR_ENV': 'dev',
        'AUTH_TOKEN': 'access_token',
        'AUTH_USER': 'user',
        'SERVICE_URL': '/',
        'AUTH_HEADER':'header',
    });

  
})(angular.module('app'));