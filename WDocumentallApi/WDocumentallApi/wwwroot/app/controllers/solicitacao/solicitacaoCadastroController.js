(function (app) {


    app.controller('solicitacaoCadastroController', solicitacaoCadastroController);

    solicitacaoCadastroController.$inject = ['$scope', '$rootScope', 'dataService', 'notificacaoService', '$routeParams', '$filter'];

    function solicitacaoCadastroController($scope, $rootScope, dataService, notificacaoService, $routeParams, $filter) {
 
        var ctrl = this;

        ctrl.Base64 = null,
        ctrl.NomeDocumento = null,
        ctrl.ExtensaoDocumento = null;

        ctrl.solicitacao = {
            Nome: '',
            CPF: '',
            ValorDeclarado: '',
            QuantidadePagina: '',
            IDCertisign: '',
            IdStatusSolicitacao: '',
            Documentos:
                [{ DocumentoBase64: '', NomeDocumento: '', ExtensaoDocumento: '' }] 
        };


        ctrl.salvar = function () {
           
            ctrl.Documentos = [{
                DocumentoBase64: ctrl.Base64, NomeDocumento: ctrl.NomeDocumento, ExtensaoDocumento: ctrl.ExtensaoDocumento
            }]
            ctrl.solicitacao.Documentos = ctrl.Documentos;
          
            dataService.post('/Api/SolicitacaoDeRegistro/Cadastrar', ctrl.solicitacao, {}, sucess);
        };

        function sucess(response) {
            notificacaoService.displaySuccess("Solicitação cadastrada com sucesso!");
            ctrl.solicitacao = {};
        }

       
        var handleFileSelect = function (evt) {
            var file = evt.currentTarget.files[0];
            var reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(function ($scope) {
                    ctrl.Base64 = evt.target.result;
                });
            };
            reader.readAsDataURL(file);

            if (file)
            {           
                var extensao = file.name.split('.');
                ctrl.NomeDocumento = file.name;
                ctrl.ExtensaoDocumento  = extensao[1];
            }
        };
        angular.element(document.querySelector('#file')).on('change', handleFileSelect);

    };
})(angular.module('app'));