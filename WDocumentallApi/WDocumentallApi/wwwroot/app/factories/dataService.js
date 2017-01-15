(function (app) {
    'use strict';

    app.factory('dataService', dataService);

    dataService.$inject = ['$http', '$location', 'notificacaoService', '$rootScope'];

    function dataService($http, $location, notificacaoService, $rootScope) {
        var service = {
            get: get,
            post: post
        };

        function tratarErro(data)
        {
            var retorno = JSON.stringify(data);

            var json = JSON.parse(retorno);

            var listaErros = [];
            for (var i = 0; i < json.data.errors.length; i++) {

                listaErros.push(json.data.errors[i]+ "<br>");
            }
            return listaErros;
        }

        function get(url,config,success) {
            return $http.get(url, { headers: { 'Authorization': 'Bearer ' + localStorage.getItem("access_token") } })
                    .then(function (result) {
                      //  console.log(result);
                        //var retorno = JSON.stringify(result);
                        // console.log(retorno);
                         success(result);
                    }, function (result) {
                        //var retorno = JSON.stringify(data);
                        // console.log(retorno);
                        if (result.status == '401') {
                            notificacaoService.displayWarning('Não foi possível obter o token. Favor logar novamente');
                            $rootScope.previousState = $location.path();
                            $location.path('/login');
                        }
                        else {
                            notificacaoService.displayInfo(tratarErro(result));
                        }
                    });
        }

        function post(url, data, header, success) {
          
            return $http.post(url, data, { headers: { 'Authorization': 'Bearer ' + localStorage.getItem("access_token") } })
                    .then(function (result) {

                        //  console.log(result);
                        //var retorno = JSON.stringify(result);
                        // console.log(retorno);

                        success(result);
                    }, function (result) {
                        if (result.status == '401') {
                            notificacaoService.displayWarning('Não foi possível obter o token. Favor logar novamente');
                            $rootScope.previousState = $location.path();
                            $location.path('/login');
                        }
                        else   {

                            notificacaoService.displayInfo(tratarErro(result));
                        }
                    });
        }

        return service;
    }

})(angular.module('app'));