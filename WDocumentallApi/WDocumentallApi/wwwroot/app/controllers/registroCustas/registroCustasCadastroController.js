(function (app) {


    app.controller('registroCustasCadastroController', registroCustasCadastroController);

    registroCustasCadastroController.$inject = ['$scope', '$rootScope', 'dataService', 'notificacaoService', '$routeParams', '$filter'];

    function registroCustasCadastroController($scope, $rootScope, dataService, notificacaoService, $routeParams, $filter) {
 
        var ctrl = this;
        $('.datepicker').pickadate({
            container: 'body',
            format: 'dd/mm/yyyy'
        })
      
        ctrl.isodata = '';
        ctrl.registrosPorPagina = 5;
        ctrl.totalRegistros = 0;
        ctrl.paginaCorrente = 1;

        ctrl.listaStatus = {};
        ctrl.listaSolicitacao = {};

        ctrl.registroCustas = {
            Descricao: '',
            Valor:'',
            IdSolicitacaoDeRegistro : '' 
           
        };

        ctrl.solicitacao = {
            isodata:'',
            status: '',
            page: 0,
            pageSize: 5,
 
        }

        ctrl.salvar = function() {
           
            ctrl.registroCustas.IdSolicitacaoDeRegistro = ctrl.solicitacao.IdSolicitacaoDeRegistro
            dataService.post('/Api/SolicitacaoDeRegistrosCustas/Cadastrar', ctrl.registroCustas, {}, sucess);
        };

        function sucess(response) {
            notificacaoService.displaySuccess("Registros de custas cadastrada com sucesso!");
            ctrl.registroCustas = {};
        }

        ctrl.listarStatus = function ()
        {
            dataService.get('/Api/StatusSolicitacao/Listar', {}, sucessStatus);
        }

        function sucessStatus(response)
        {
            ctrl.listaStatus = response.data;
        }
        ctrl.listarStatus();

        ctrl.pesquisarForm = function () {

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