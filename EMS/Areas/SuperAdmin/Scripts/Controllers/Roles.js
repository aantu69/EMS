var app = angular.module("app", []);

app.factory("EMSService", ['$http', '$window', function ($http, $window) {
    return {
        createData: function (ViewModel, selectedMenus) {
            var url = '/Roles/CreateData';
            var redirect = '/SuperAdmin/Roles/Index';
            $http({
                url: url,
                method: "POST",
                data: { 'model': ViewModel, 'selectedMenus': selectedMenus },
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        updateData: function (ViewModel, selectedMenus) {
            var url = '/Roles/UpdateData';
            var redirect = '/SuperAdmin/Roles/Index';
            ViewModel.Id = this.getEditId();
            $http({
                url: url,
                method: "POST",
                data: { 'model': ViewModel, 'selectedMenus': selectedMenus },
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        getDatas: function (pagingInfo) {
            return $http.get('/Roles/GetDatas', { params: pagingInfo });
        },
        getData: function () {
            var currentId = this.getEditId();
            if (currentId != null) {
                return $http.get('/Roles/GetData', { params: { id: currentId } });
            }
        },
        getMenus: function () {
            return $http.get('/Roles/GetMenus');
        },
        getSelectedMenus: function () {
            var url = '/Roles/GetSelectedMenus';
            var currentId = this.getEditId();
            if (currentId != null) {
                return $http.get(url, { params: { id: currentId } });
            }
        },
        getEditId: function () {
            var absoluteUrlPath = $window.location.href;
            var results = String(absoluteUrlPath).split("/");
            if (results != null && results.length > 0) {
                var currentId = results[results.length - 1];
                return currentId;
            }
        },
        removeData: function (Id) {
            //var currentId = Shift.ShiftId;
            if (Id != null) {
                if (window.confirm('Are you sure, you want to delete this record?')) {
                    return $http({
                        url: '/Roles/DeleteData',
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
    $scope.redirect = function (ID) {
        $window.location.href = '/SuperAdmin/Roles/Edit/' + ID;
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
        $scope.roles = null;
        EMSService.getDatas($scope.pagingInfo).then(function (result) {
            $scope.roles = result.data.Roles;

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
    EMSService.getMenus().then(function (result) {
        $scope.menus = result.data.Menus;
    }, function (error) {
        alert('Error');
    });  

    $scope.selectedMenus = [];
    $scope.toggleSelection = function toggleSelection(selectedId) {
        var idx = $scope.selectedMenus.indexOf(selectedId);
        if (idx > -1) {//is currently selected
            $scope.selectedMenus.splice(idx, 1);
        }
        else {//is newly selected
            $scope.selectedMenus.push(selectedId);
        }
    };

    $scope.addData = function () {
        var role = {};
        role.Name = $scope.Name;
        role.Description = $scope.Description;
        var selectedMenus = $scope.selectedMenus;
        EMSService.createData(role, selectedMenus);
        //EMSService.createData(role);
    };
}]);

app.controller('EditCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    EMSService.getMenus().then(function (result) {
        $scope.menus = result.data.Menus;
    }, function (error) {
        alert('Error');
    });

    EMSService.getSelectedMenus().then(function (result) {
        $scope.selectedMenus = result.data;
    });
    $scope.toggleSelection = function toggleSelection(selectedId) {
        var idx = $scope.selectedMenus.indexOf(selectedId);
        if (idx > -1) {//is currently selected
            $scope.selectedMenus.splice(idx, 1);
        }
        else {//is newly selected
            $scope.selectedMenus.push(selectedId);
        }
    };

    EMSService.getData().then(function (result) {
        $scope.Name = result.data.Name;
        $scope.Description = result.data.Description;
        //alert(result.data.Name);
        //alert(result.data.toSource());
    }, function (error) {
        alert('Error');
    }

    );
    $scope.updateData = function () {
        $scope.btnUpdateText = "Updating...";
        var role = {};
        role.Name = $scope.Name;
        role.Description = $scope.Description;
        var selectedMenus = $scope.selectedMenus;
        EMSService.updateData(role, selectedMenus);
        $scope.btnUpdateText = "Update";
    };
}]);

