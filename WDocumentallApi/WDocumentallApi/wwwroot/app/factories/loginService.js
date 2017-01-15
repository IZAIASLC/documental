(function (app) {
    'use strict';
    app.factory('loginService', loginService);

    loginService.$inject = ['$http', 'SETTINGS'];

    function loginService($http, SETTINGS) {
       
        return {
            login: login
        };

        function login(data) {

            var dt = "grant_type=password&username=" + data.email + "&password=" + data.senha;
            var url = SETTINGS.SERVICE_URL + 'Api/Token';
            var header = { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };

            return $http.post(url, dt, header);
        }
 
    }
})(angular.module('app'));