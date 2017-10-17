var SideMenu = angular.module("SideMenu", []);
var RootApp = angular.module("RootApp", ["SideMenu","app"]);

SideMenu.factory("SideMenuService", ['$http', '$window', function ($http, $window) {
    return {
        getDatas: function () {
            var url = '/SuperAdminSideMenu/GetDatas';
            return $http.get(url);
        },
        
    }
}]);

SideMenu.controller('SideMenuListCtlr', ['$scope', '$http', '$window', 'SideMenuService', function ($scope, $http, $window, SideMenuService) {
    function load() {
        $scope.users = null;
        SideMenuService.getDatas().then(function (result) {
            $scope.menus = result.data.Menus;
            //alert(result.data.Users.toSource());

            
        }, function (error) { alert(err.data); });
    }

    // initial table load
    load();

}]);

