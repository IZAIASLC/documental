(function (app) {
    'use strict';

    app.directive('sideBar', sideBar);

    function sideBar() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'app/pages/layout/side-bar.html'
        }
    };

})(angular.module('app'));