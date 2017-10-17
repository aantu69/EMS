var app = angular.module("app", []);

app.factory("EMSService", ['$http', '$window', function ($http, $window) {
    var message = "";

    var addMessage = function (msg) {
        message = msg;
    };

    var getMessage = function () {
        return message;
    };
    return {
        addMessage: addMessage,
        getMessage: getMessage,
        createData: function (Menu) {
            $http({
                url: "/Menus/CreateData",
                method: "POST",
                data: Menu
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) === 'object') {
                    addMessage(response.data);
                    window.location.href = '/SuperAdmin/Menus/Index';
                }
            }, function (response) {

            });
        },
        updateData: function (Menu) {
            Menu.ID = this.getEditId();
            $http({
                url: "/Menus/UpdateData",
                method: "POST",
                data: Menu
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) === 'object') {
                    window.location.href = '/SuperAdmin/Menus/Index';
                }
            }, function (response) {

            })
        },
        getDatas: function (pagingInfo) {
            return $http.get('/Menus/GetDatas', { params: pagingInfo });
        },
        getData: function () {
            var currentId = this.getEditId();
            if (currentId !== null) {
                return $http.get('/Menus/GetData', { params: { id: currentId } });
            }
        },
        getEditId: function () {
            var absoluteUrlPath = $window.location.href;
            var results = String(absoluteUrlPath).split("/");
            if (results !== null && results.length > 0) {
                var currentId = results[results.length - 1];
                return currentId;
            }
        },
        removeData: function (Id) {
            //var currentId = Shift.ShiftId;
            if (Id != null && parseInt(Id, 10) > 0) {
                if (window.confirm('Are you sure, you want to delete this record?')) {
                    return $http({
                        url: '/Menus/DeleteData',
                        method: 'POST',
                        data: { Id: Id }
                    });
                }

            }
        },
    }
}]);

app.controller('ListCtlr', ['$scope', '$http', '$window', 'EMSService', function ($scope, $http, $window, EMSService) {
    $scope.btnDeleteText = "Delete";
    $scope.btnEditText = "Edit";
    $scope.message = EMSService.getMessage();
    $scope.redirect = function (ID) {
        $window.location.href = '/SuperAdmin/Menus/Edit/' + ID;
    };

    $scope.removeData = function (ID) {
        EMSService.removeData(ID).then(function (data) {
            load();
        });
    }

    $scope.pagingInfo = {
        page: 1,
        pageSize: 5,
        sortBy: 'Name',
        isAsc: true,
        search: '',
        totalPages: 0
    };

    $scope.search = function () {
        $scope.pagingInfo.page = 1;
        load();
    };

    $scope.sort = function (sortBy) {
        if (sortBy === $scope.pagingInfo.sortBy) {
            $scope.pagingInfo.isAsc = !$scope.pagingInfo.isAsc;
        } else {
            $scope.pagingInfo.sortBy = sortBy;
            $scope.pagingInfo.isAsc = true;
        }
        $scope.pagingInfo.page = 1;
        load();
    };

    $scope.selectPage = function (page) {
        $scope.pagingInfo.page = page;
        load();
    };
    $scope.filterPage = function (pageSize) {
        $scope.pagingInfo.pageSize = pageSize;
        $scope.pagingInfo.page = 1;
        load();
    };

    function load() {
        $scope.menus = null;
        EMSService.getDatas($scope.pagingInfo).then(function (result) {
            $scope.menus = result.data.Menus;

            $scope.pagingInfo.totalPages = result.data.TotalPages;
            $scope.pagingInfo.pageSize = $scope.pagingInfo.pageSize;
            $scope.pageSizeOption = [5, 10];
            $scope.maxPages = 11;
            $scope.pagesAfter = result.data.TotalPages - $scope.pagingInfo.page;
            $scope.pagesBefore = $scope.pagingInfo.page - 1;
            $scope.totalPagesLessEqualMaxPages = $scope.pagingInfo.totalPages > 1 && $scope.pagingInfo.totalPages <= $scope.maxPages ? true : false;
            $scope.totalPagesGreaterMaxPages = $scope.pagingInfo.totalPages > $scope.maxPages ? true : false;
            $scope.pagesAfterLessEqual4 = $scope.totalPagesGreaterMaxPages && $scope.pagesAfter <= 4 ? true : false;
            $scope.pagesBeforeLessEqual4 = $scope.totalPagesGreaterMaxPages && $scope.pagesBefore <= 4 ? true : false;
            $scope.pagesAfterBeforeGreater4 = $scope.totalPagesGreaterMaxPages && $scope.pagesAfter > 4 && $scope.pagesBefore > 4 ? true : false;
            $scope.pageSubsetAfter = result.data.TotalPages - $scope.maxPages - 1 > 1 ? result.data.TotalPages - $scope.maxPages - 1 : 2;
            $scope.pageSubsetBefore = $scope.maxPages + 2 < result.data.TotalPages ? $scope.maxPages + 2 : result.data.TotalPages - 1;
            $scope.pageSubsetAfter1 = $scope.pagingInfo.page - 7 > 1 ? $scope.pagingInfo.page - 7 : 2;
            $scope.pageSubsetAfter2 = $scope.pagingInfo.page + 7 < result.data.TotalPages ? $scope.pagingInfo.page + 7 : result.data.TotalPages - 1;

            $scope.range = function (min, max, step) {
                step = step || 1;
                var input = [];
                for (var i = min; i <= max; i += step) {
                    input.push(i);
                }
                return input;
            };

            //alert(result.data.data.toSource());
            //alert($scope.pagingInfo.sortBy);
        });
    }

    // initial table load
    load();

}]);

app.controller('AddCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnSaveText = "Save";
    $scope.showMessage = false;
    $scope.addData = function () {
        var menu = {};
        menu.Name = $scope.Name;
        menu.Area = $scope.Area;
        menu.ControllerName = $scope.ControllerName;
        menu.ActionName = $scope.ActionName;
        menu.IsActive = $scope.IsActive;
        menu.Order = $scope.Order;
        EMSService.createData(menu);
    };
}]);

app.controller('EditCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    EMSService.getData().then(function (result) {
        $scope.Name = result.data.Name;
        //alert(result.data.Name);
        //alert(result.data.toSource());
    }, function (error) {
        alert('Error');
    }

    );
    $scope.updateData = function () {
        $scope.btnUpdateText = "Updating...";
        var menu = {};
        menu["Name"] = $scope.Name;
        EMSService.updateData(menu);
        $scope.btnUpdateText = "Update";
    };
}]);

