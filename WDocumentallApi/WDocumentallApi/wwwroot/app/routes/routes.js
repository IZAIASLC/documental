
(function (app) {
    'use strict';

    app.config(function ($routeProvider) {

        $routeProvider.
          when('/', {
              templateUrl: 'app/pages/index/index.html',
              controller: 'indexController',
              controllerAs: 'ctrl',
              resolve: { isAuthenticated: isAuthenticated }
          }),
    
            $routeProvider.when('/login', {
                templateUrl: 'app/pages/acesso/login.html',
                controller: 'loginController',
                controllerAs: 'ctrl',
            }),

         $routeProvider.when('/cadastro', {
             templateUrl: 'app/pages/acesso/cadastrar.html',
             controller: 'usuarioController',
             controllerAs: 'ctrl'
         }),
    
         $routeProvider.when('/solicitacao/cadastrar', {
             templateUrl: 'app/pages/solicitacao/solicitacao-cadastrar.html',
             controller: 'solicitacaoCadastroController',
             controllerAs: 'ctrl',
               resolve: { isAuthenticated: isAuthenticated }
         }),

           $routeProvider.when('/solicitacao/listar', {
               templateUrl: 'app/pages/solicitacao/solicitacao-listar.html',
               controller: 'solicitacaoListagemController',
               controllerAs: 'ctrl',
               resolve: { isAuthenticated: isAuthenticated }
               
           }),

          $routeProvider.when('/solicitacao/editar/:id', {
              templateUrl: 'app/pages/solicitacao/solicitacao-editar.html',
              controller: 'solicitacaoEdicaoController',
              controllerAs: 'ctrl',
              resolve: { isAuthenticated: isAuthenticated }

          }),

          $routeProvider.when('/registrocustas/cadastrar', {
              templateUrl: 'app/pages/registrocustas/custas-cadastrar.html',
              controller: 'registroCustasCadastroController',
              controllerAs: 'ctrl',
              resolve: { isAuthenticated: isAuthenticated }
          }),

           $routeProvider.when('/registrocustas/listar', {
               templateUrl: 'app/pages/registrocustas/custas-listar.html',
               controller: 'registroCustasListagemController',
               controllerAs: 'ctrl',
               resolve: { isAuthenticated: isAuthenticated }

           }),

          $routeProvider.when('/registrocustas/editar/:id', {
              templateUrl: 'app/pages/registrocustas/custas-editar.html',
              controller: 'registroCustasEdicaoController',
              controllerAs: 'ctrl',
              resolve: { isAuthenticated: isAuthenticated }

          }),


         $routeProvider.when('/registroexigencias/cadastrar', {
             templateUrl: 'app/pages/registroexigencias/exigencias-cadastrar.html',
             controller: 'registroExigenciasCadastroController',
             controllerAs: 'ctrl',
             resolve: { isAuthenticated: isAuthenticated }
         }),

           $routeProvider.when('/registroexigencias/listar', {
               templateUrl: 'app/pages/registroexigencias/exigencias-listar.html',
               controller: 'registroExigenciasListagemController',
               controllerAs: 'ctrl',
               resolve: { isAuthenticated: isAuthenticated }

           }),

          $routeProvider.when('/registroexigencias/editar/:id', {
              templateUrl: 'app/pages/registroexigencias/exigencias-editar.html',
              controller: 'registroExigenciasEdicaoController',
              controllerAs: 'ctrl',
              resolve: { isAuthenticated: isAuthenticated }

          }),

       

        $routeProvider.otherwise({
            redirectTo: '/'
        });


    });

    isAuthenticated.$inject = ['$rootScope', '$location'];

    function isAuthenticated($rootScope, $location) {
     
        var token = localStorage.getItem("access_token");

        
        if (token == null) {
          //  $rootScope.previousState = $location.path();
           
           $location.path('/login');
        }
        
    }

})(angular.module('app'));

