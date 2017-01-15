(function (app) {
    'use strict';

    app.controller('loginController', loginController);

    loginController.$inject = ['$location', '$rootScope', 'loginService', 'notificacaoService','SETTINGS'];

    function loginController($location, $rootScope, loginService, notificacaoService, SETTINGS) {
        var ctrl = this;

      
        ctrl.login = {
            email: '',
            senha: ''
        };

        ctrl.login = {};

 
        ctrl.logar = function () {

            loginService.login(ctrl.login).success(success)
                .catch(fail);;
        }

        function success(response) {
            $rootScope.user = ctrl.login.email;
            $rootScope.token = response.access_token;
            localStorage.setItem(SETTINGS.AUTH_TOKEN, response.access_token);
           
            ctrl.login = {};

             $location.path('/');
        }

        function fail(error) {
            notificacaoService.displayError(error.data.error_description);
        }
 
    };


})(angular.module('app'));