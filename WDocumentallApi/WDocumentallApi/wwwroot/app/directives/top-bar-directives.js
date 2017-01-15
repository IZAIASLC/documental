(function (app) {
    'use strict';

   app.directive('topBar', topBar);

    function topBar() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'app/pages/layout/top-bar.html'
        }
    };
})(angular.module('app'));