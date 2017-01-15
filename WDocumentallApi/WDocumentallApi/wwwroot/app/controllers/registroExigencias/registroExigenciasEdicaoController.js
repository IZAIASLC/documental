(function (app) {


    app.controller('registroExigenciasEdicaoController', registroExigenciasEdicaoController);

    registroExigenciasEdicaoController.$inject = ['$scope', '$rootScope', 'dataService', 'notificacaoService', '$routeParams', '$filter'];

    function registroExigenciasEdicaoController($scope, $rootScope, dataService, notificacaoService, $routeParams, $filter) {
 
        var ctrl = this;
 
        ctrl.listar = function () {

            dataService.get('/Api/SolicitacaoDeRegistrosExigencias/Listar/' + $routeParams.id, {}, sucessBusca);
        };

        function sucessBusca(response) {

            ctrl.registroExigencias = response.data;
        }

        ctrl.listar();


        ctrl.editar = function () {
  
            dataService.post('/Api/SolicitacaoDeRegistrosExigencias/Atualizar', ctrl.registroExigencias, {}, sucessEdicao);
        };

        function sucessEdicao(response) {
            notificacaoService.displaySuccess("Registro de exigências atualizada com sucesso!");
         
        }

       
    };
})(angular.module('app'));