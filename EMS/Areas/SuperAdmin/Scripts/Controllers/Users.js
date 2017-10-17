var app = angular.module("app", []);

app.factory("EMSService", ['$http', '$window', function ($http, $window) {
    return {
        createData: function (ViewModel, selectedRoles) {
            var url = '/Users/CreateData';
            var redirect = '/SuperAdmin/Users/Index';
            $http({
                url: url,
                method: "POST",
                data: { 'model': ViewModel, 'selectedRoles': selectedRoles },
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        updateData: function (ViewModel, selectedRoles) {
            var url = '/Users/UpdateData';
            var redirect = '/SuperAdmin/Users/Index';
            ViewModel.Id = this.getEditId();
            $http({
                url: url,
                method: "POST",
                data: { 'model': ViewModel, 'selectedRoles': selectedRoles }
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        getDatas: function () {
            var url = '/Users/GetRoles';
            return $http.get(url);
        },
        getDatasPagination: function (pagingInfo) {
            var url = '/Users/GetDatas';
            return $http.get(url, { params: pagingInfo });
        },
        getData: function () {
            var url = '/Users/GetData';
            var currentId = this.getEditId();
            if (currentId != null) {
                return $http.get(url, { params: { id: currentId } });
            }
        },
        getSelectedRoles: function () {
            var url = '/Users/GetSelectedRoles';
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
            var url = '/Users/DeleteData';
            if (Id != null) {
                if (window.confirm('Are you sure, you want to delete this record?')) {
                    return $http({
                        url: url,
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
    $scope.redirect = function (Id) {
        $window.location.href = '/SuperAdmin/Users/Edit/' + Id;
    };

    $scope.removeData = function (Id) {
        EMSService.removeData(Id).then(function (data) {
            load();
        });
    }

    $scope.pagingInfo = {
        page: 1,
        pageSize: 5,
        sortBy: 'Email',
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
        $scope.users = null;
        EMSService.getDatasPagination($scope.pagingInfo).then(function (result) {
            $scope.users = result.data.Users;
            //alert(result.data.Users.toSource());

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
        }, function (error) { alert(err.data); });
    }

    // initial table load
    load();

}]);

app.controller('AddCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnSaveText = "Save";
    $scope.showMessage = false;
    EMSService.getDatas().then(function (result) {
        $scope.roles = result.data;
    });
    $scope.selectedRoles = [];
    $scope.toggleSelection = function toggleSelection(selectedId) {
        var idx = $scope.selectedRoles.indexOf(selectedId);
        if (idx > -1) {//is currently selected
            $scope.selectedRoles.splice(idx, 1);
        }
        else {//is newly selected
            $scope.selectedRoles.push(selectedId);
        }
    };


    $scope.addData = function () {
        var user = {};
        user.Email = $scope.Email;
        user.FirstName = $scope.FirstName;
        user.LastName = $scope.LastName;
        user.InstituteId = 0;
        //user.UserRoles = $scope.ddlRole.Name;
        user.Password = $scope.Password;
        var selectedRoles = $scope.selectedRoles;
        EMSService.createData(user, selectedRoles);
    };
}]);

app.controller('EditCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    EMSService.getData().then(function (result) {
        $scope.UserName = result.data.UserName;
        $scope.Email = result.data.Email;
        $scope.FirstName = result.data.FirstName;
        $scope.LastName = result.data.LastName;
        //alert(result.data.Name);
        //alert(result.data.toSource());
    }, function (error) {
        alert('Error');
    });

    EMSService.getDatas().then(function (result) {
        $scope.roles = result.data;
    });

    EMSService.getSelectedRoles().then(function (result) {
        $scope.selectedRoles = result.data;
    });
    //$scope.selectedRoles = [];
    $scope.toggleSelection = function toggleSelection(selectedId) {
        var idx = $scope.selectedRoles.indexOf(selectedId);
        if (idx > -1) {//is currently selected
            $scope.selectedRoles.splice(idx, 1);
        }
        else {//is newly selected
            $scope.selectedRoles.push(selectedId);
        }
    };

    $scope.updateData = function () {
        $scope.btnUpdateText = "Updating...";
        var user = {};
        user.Email = $scope.Email;
        user.FirstName = $scope.FirstName;
        user.LastName = $scope.LastName;
        user.InstituteId = 0;
        var selectedRoles = $scope.selectedRoles;
        EMSService.updateData(user, selectedRoles);
        $scope.btnUpdateText = "Update";
    };
}]);

