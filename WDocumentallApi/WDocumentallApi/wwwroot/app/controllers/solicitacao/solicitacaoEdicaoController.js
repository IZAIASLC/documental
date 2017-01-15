(function (app) {


    app.controller('solicitacaoEdicaoController', solicitacaoEdicaoController);

    solicitacaoEdicaoController.$inject = ['$scope', '$rootScope', 'dataService', 'notificacaoService', '$routeParams', '$filter'];

    function solicitacaoEdicaoController($scope, $rootScope, dataService, notificacaoService, $routeParams, $filter) {
 
        var ctrl = this;
        ctrl.adicionado = false;
        ctrl.Base64 = null,
        ctrl.NomeDocumento = null,
        ctrl.ExtensaoDocumento = null;
        ctrl.solicitacao = {};
      
        ctrl.listar = function () {

            dataService.get('/Api/SolicitacaoDeRegistro/Listar/' + $routeParams.id, {}, sucessBusca);
        };

        function sucessBusca(response) {

            ctrl.solicitacao = response.data;
        }

        ctrl.listar();


        ctrl.editar = function () {
           
            ctrl.Documentos = [{}];
           // DocumentoBase64: ctrl.Base64, NomeDocumento: ctrl.NomeDocumento, ExtensaoDocumento: ctrl.ExtensaoDocumento
            ctrl.documento = {};

            ctrl.Documentos.pop();

            angular.forEach(ctrl.solicitacao.Documentos, function (item) {

                ctrl.documento.NomeDocumento = item.NomeDocumento;
                ctrl.documento.DocumentoBase64 = item.DocumentoBase64;
                ctrl.documento.ExtensaoDocumento = item.ExtensaoDocumento;
                ctrl.documento.IdDocumento = item.IdDocumento;
                ctrl.Documentos.push(ctrl.documento);
            });
            
            //Caso na atualização tenha sido inserido algum documento
            if (ctrl.Base64) {
                ctrl.documento = {};
                ctrl.documento.NomeDocumento = ctrl.NomeDocumento;
                ctrl.documento.DocumentoBase64 = ctrl.Base64;
                ctrl.documento.ExtensaoDocumento = ctrl.ExtensaoDocumento;
                ctrl.documento.IdDocumento = 0;
                ctrl.Documentos.push(ctrl.documento);
            }


            ctrl.solicitacao.Documentos = ctrl.Documentos;
          
            dataService.post('/Api/SolicitacaoDeRegistro/Atualizar', ctrl.solicitacao, {}, sucessEdicao);
        };

        function sucessEdicao(response) {
            notificacaoService.displaySuccess("Solicitação atualizada com sucesso!");
         
        }

        function documentoAdicionado(documento)
        {
            angular.forEach(ctrl.solicitacao.Documentos, function (item) {
               
                if (documento.toUpperCase() == item.NomeDocumento.toUpperCase()) {
                    notificacaoService.displaySuccess("Documento já adicionado nessa solicitação!");
                    ctrl.adicionado = true;
                    return;
                }

            });
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
              //  documentoAdicionado(file.name);
               // if (!ctrl.adicionado) {
                
                    var extensao = file.name.split('.');
                    ctrl.NomeDocumento = file.name;
                    ctrl.ExtensaoDocumento = extensao[1];

               // }
               // else {
                   // ctrl.Base64 = null;
               // }
            }
        };
        angular.element(document.querySelector('#file')).on('change', handleFileSelect);

    };
})(angular.module('app'));