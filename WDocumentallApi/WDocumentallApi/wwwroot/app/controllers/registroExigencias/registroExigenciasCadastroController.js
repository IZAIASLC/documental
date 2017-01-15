(function (app) {

    app.controller('registroExigenciasCadastroController', registroExigenciasCadastroController);
    registroExigenciasCadastroController.$inject = ['$scope', '$rootScope', 'dataService', 'notificacaoService', '$routeParams', '$filter'];

    function registroExigenciasCadastroController($scope, $rootScope, dataService, notificacaoService, $routeParams, $filter) {
 
        var ctrl = this;

        $('.datepicker').pickadate({
            container: 'body',
            format: 'dd/mm/yyyy'
        })
        ctrl.isodata = '';
        ctrl.listaSolicitacao = {};
        ctrl.solicitacao = {
            isodata: null,
            status: null,
            page: 0,
            pageSize: 5,

        }

        ctrl.registrosPorPagina = 1;
        ctrl.totalRegistros = 0;
        ctrl.paginaCorrente = 1;


        ctrl.registroExigencias = {
            Descricao: '',          
            IdSolicitacaoDeRegistro : ''          
        };

        ctrl.salvar = function () {          
            ctrl.registroExigencias.IdSolicitacaoDeRegistro = ctrl.solicitacao.IdSolicitacaoDeRegistro;
            dataService.post('/Api/SolicitacaoDeRegistrosExigencias/Cadastrar', ctrl.registroExigencias, {}, sucess);
        };

        function sucess(response) {
            notificacaoService.displaySuccess("Exigências cadastrada com sucesso!");
            ctrl.registroExigencias = {};
        }

        ctrl.listarStatus = function () {
            dataService.get('/Api/StatusSolicitacao/Listar', {}, sucessStatus);
        }

        function sucessStatus(response) {
            ctrl.listaStatus = response.data;
        }
        ctrl.listarStatus();


        ctrl.pesquisarForm = function ()
        {
            ctrl.isodata = FormatarIsoData(ctrl.solicitacao.isodata);
            ctrl.paginaCorrente = 1;
            ctrl.pesquisar(0);
        }

        ctrl.pesquisar = function (page) {
            ctrl.listaSolicitacao = {};
            page = page - 1;
            dataService.get('/Api/SolicitacaoDeRegistro/Pesquisar/' + ctrl.isodata + '/' + ctrl.solicitacao.status + '/' + page + '/' + ctrl.registrosPorPagina, {}, sucessPesquisa);
        }

        function sucessPesquisa(response) {
            ctrl.totalRegistros = response.data.TotalCount;
            ctrl.listaSolicitacao = response.data.Items;
        }

        ctrl.selecionar = function (solicitacao) {     
            ctrl.solicitacao.Nome = solicitacao.Nome;
            ctrl.solicitacao.IDCertisign = solicitacao.IDCertisign;
            ctrl.solicitacao.IdSolicitacaoDeRegistro = solicitacao.IdSolicitacaoDeRegistro;
            $('.modal').closeModal();

        }
 
    };
})(angular.module('app'));