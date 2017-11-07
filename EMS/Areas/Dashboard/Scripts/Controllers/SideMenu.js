var SideMenu = angular.module("SideMenu", []);
var RootApp = angular.module("RootApp", ["SideMenu", "app"]);

SideMenu.directive('icheck', function ($timeout, $parse) {
    return {
        require: 'ngModel',
        link: function ($scope, element, $attrs, ngModel) {
            return $timeout(function () {
                var value;
                var color = $attrs.color;
                var skin = $attrs.skin;

                skin = skin && color ? skin + '-' + color : skin;
                value = $attrs['ngValue'];

                $scope.$watch($attrs['ngModel'], function (newValue) {
                    $(element).iCheck('update');
                })

                function update(checked) {
                    if ($(element).attr('type') === 'radio') {
                        ngModel.$setViewValue(value);
                    } else {
                        ngModel.$setViewValue(checked);
                    }
                }

                return $(element).iCheck({
                    checkboxClass: skin ? 'icheckbox_' + skin : 'icheckbox_square-blue',
                    radioClass: skin ? 'iradio_' + skin : 'iradio_square-blue',

                }).on('ifChanged', function (event) {
                    $scope.$apply(function () {
                        update(event.target.checked);
                    });
                });
            });
        }
    };
});

SideMenu.directive('select2', function () {
    return {
        restrict: 'A',
        scope: {
            //'selectWidth': '@',
            'ngModel': '='
        },
        link: function (scope, element, attrs) {
            //scope.selectWidth = scope.selectWidth || 200;
            element.select2({
                //width: scope.selectWidth,
            });

            scope.$watch('ngModel', function (newVal, oldVal) {
                window.setTimeout(function () {
                    element.select2("val", newVal);
                });
            });
        }
    };
});

SideMenu.directive("datepicker", function () {
    function link(scope, element, attrs, controller) {
        element.datepicker({
            onSelect: function (dt) {
                scope.$apply(function () {
                    controller.$setViewValue(dt);
                });
            },
            dateFormat: "mm/dd/yy"
        });
    }

    return {
        require: 'ngModel',
        link: link
    };
});

SideMenu.factory("SideMenuService", ['$http', '$window', function ($http, $window) {
    return {
        getDatas: function () {
            var url = '/DashboardSideMenu/GetDatas';
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

