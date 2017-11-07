var SideMenu = angular.module("SideMenu", []);
var RootApp = angular.module("RootApp", ["SideMenu", "app"]);

SideMenu.directive('icheck', ['$timeout', '$parse', function ($timeout, $parse) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attr, ngModel) {
            $timeout(function () {
                var value = attr.value;
                var color = attr.color;
                var skin = attr.skin;
                skin = skin && color ? skin + '-' + color : skin;

                function update(checked) {
                    if (attr.type === 'radio') {
                        ngModel.$setViewValue(value);
                    } else {
                        ngModel.$setViewValue(checked);
                    }
                }

                $(element).iCheck({
                    checkboxClass: skin ? 'icheckbox_' + skin : 'icheckbox_square-red',
                    radioClass: skin ? 'iradio_' + skin : 'iradio_square-red',
                }).on('ifChanged', function (e) {
                    scope.$apply(function () {
                        update(e.target.checked);
                    });
                }).on('ifClicked', function (e) {
                    scope.$apply(function () {
                        update(e.target.checked);
                    });
                });

                //scope.$watch(attr.ngChecked, function (checked) {
                //    if (typeof checked === 'undefined') checked = !!ngModel.$viewValue;
                //    update(checked);
                //}, true);

                scope.$watch(attr.ngModel, function (model) {
                    $(element).iCheck('update');
                }, true);

            })
        }
    }
}]);

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

