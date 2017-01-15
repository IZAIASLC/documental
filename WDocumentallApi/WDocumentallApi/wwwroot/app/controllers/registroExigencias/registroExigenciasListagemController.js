(function (app) {
    'use strict';

    app.controller('registroExigenciasListagemController', registroExigenciasListagemController);

    registroExigenciasListagemController.$inject = ['$rootScope', 'dataService', 'notificacaoService'];

    function registroExigenciasListagemController($rootScope, dataService, notificacaoService) {

        var ctrl = this;

    
        ctrl.listagemRegistroExigencias = {};
        ctrl.registrosPorPagina = 2;
        ctrl.totalRegistros = 0;
        ctrl.paginaCorrente = 1;

        ctrl.pesquisar = function (newPage) {
            pesquisar(newPage)
        };

        function pesquisar(page) {

            page = page - 1;
            dataService.get('/Api/SolicitacaoDeRegistrosExigencias/Listar/' + page + '/' + ctrl.registrosPorPagina, {}, sucess);

        };

        function sucess(response) {
            ctrl.totalRegistros = response.data.TotalCount;
            ctrl.listagemRegistroExigencias = response.data.Items
        }

        pesquisar(0);

    };
})(angular.module('app'));