(function (app) {
    'use strict';

    app.controller('registroCustasListagemController', registroCustasListagemController);

    registroCustasListagemController.$inject = ['$rootScope', 'dataService', 'notificacaoService'];

    function registroCustasListagemController($rootScope, dataService, notificacaoService) {

        var ctrl = this;

        ctrl.listagemRegistroCustas = {};

        ctrl.registrosPorPagina = 2;
        ctrl.totalRegistros = 0;
        ctrl.paginaCorrente = 1;

        ctrl.pesquisar = function (newPage) {
            pesquisar(newPage)
        };

        function pesquisar(page) {

            page = page - 1;

            dataService.get('/Api/SolicitacaoDeRegistrosCustas/Listar/' + page + '/' + ctrl.registrosPorPagina, {}, sucess);
        };

        function sucess(response) {
            ctrl.totalRegistros = response.data.TotalCount;
            ctrl.listagemRegistroCustas = response.data.Items
        }

        pesquisar(0);

    }

})(angular.module('app'));