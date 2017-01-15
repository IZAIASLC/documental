(function (app) {


    app.controller('registroCustasEdicaoController', registroCustasEdicaoController);

    registroCustasEdicaoController.$inject = ['$scope', '$rootScope', 'dataService', 'notificacaoService', '$routeParams', '$filter'];

    function registroCustasEdicaoController($scope, $rootScope, dataService, notificacaoService, $routeParams, $filter) {
 
        var ctrl = this;
 
        ctrl.listar = function () {

            dataService.get('/Api/SolicitacaoDeRegistrosCustas/Listar/' + $routeParams.id, $rootScope.header, sucessBusca);
        };

        function sucessBusca(response) {

            ctrl.registroCustas = response.data;
           
        }

        ctrl.listar();


        ctrl.editar = function () {
  
            dataService.post('/Api/SolicitacaoDeRegistrosCustas/Atualizar', ctrl.registroCustas, $rootScope.header, sucessEdicao);
        };

        function sucessEdicao(response) {
            notificacaoService.displaySuccess("Registro de custas atualizada com sucesso!");
         
        }

       
    };
})(angular.module('app'));