(function (app) {
    'use strict';
    app.controller('indexController', indexController);

    indexController.$inject = ['$location', '$rootScope'];

    function indexController($location,$rootScope) {
         var ctrl = this;
     
         ctrl.logout = function () {
          
            localStorage.removeItem("access_token");
            localStorage.removeItem("user");
            $rootScope.token = null;
            $rootScope.user = null;
            $location.path('/login');
 
        }

    };

})(angular.module('app'));