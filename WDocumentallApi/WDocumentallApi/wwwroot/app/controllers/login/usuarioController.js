(function (app) {


    app.controller('usuarioController', usuarioController);

    usuarioController.$inject = ['dataService', 'notificacaoService'];

    function usuarioController(dataService, notificacaoService) {

        var ctrl = this;
        ctrl.usuario = {
            Nome: '',
            Email: '',
            Senha: '',
            Ativo: true,
            UsuarioPerfils: [{ IdPerfil: 1 }]
        };
 
        ctrl.salvar = function() {

            dataService.post('/Api/Usuario/Cadastrar', ctrl.usuario, {}, sucess);
        };
        function sucess(response) {
            notificacaoService.displaySuccess("Usuário cadastrado com sucesso!");
            ctrl.usuario = {};
        }
    }

})(angular.module('app'));