(function (app) {
    'use strict';

    app.controller('solicitacaoListagemController', solicitacaoListagemController);

    solicitacaoListagemController.$inject = ['dataService','notificacaoService' ];

    function solicitacaoListagemController(dataService, notificacaoService) {

        var ctrl = this;

        ctrl.listagemSolicitacao = {};
        ctrl.registrosPorPagina = 2;
        ctrl.totalRegistros = 0;
        ctrl.paginaCorrente = 1;
 
         ctrl.pesquisar = function (newPage) {
           pesquisar(newPage)
        };

         function pesquisar(page) {

            page = page - 1;

            dataService.get('/Api/SolicitacaoDeRegistro/Listar/'+page+'/'+ctrl.registrosPorPagina, {}, sucess);
        };

        function sucess(response) {
          ctrl.totalRegistros = response.data.TotalCount;
          ctrl.listagemSolicitacao = response.data.Items
        }

         pesquisar(0); 
    };
})(angular.module('app'));