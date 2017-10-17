var app = angular.module("app", []);

app.factory("EMSService", ['$http', '$window', function ($http, $window) {
    return {
        createData: function (ViewModel, selectedRoles, tableName) {
            var url = '/Accounts/CreateRole';
            var redirect = '/SuperAdmin/Accounts/Roles';
            if (tableName == 'Users') {
                url = '/Accounts/CreateUser';
                redirect = '/SuperAdmin/Accounts/Users';
            }
            $http({
                url: url,
                method: "POST",
                data: { 'model': ViewModel, 'selectedRoles': selectedRoles},
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        updateData: function (ViewModel, tableName) {
            var url = '/Accounts/ModifyRole';
            var redirect = '/SuperAdmin/Accounts/Roles';
            if (tableName == 'Users') {
                url = '/Accounts/ModifyUser';
                redirect = '/SuperAdmin/Accounts/Users';
            }
            ViewModel.Id = this.getEditId();
            $http({
                url: url,
                method: "POST",
                data: ViewModel
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        getDatas: function (tableName) {
            var url = '/Accounts/GetRoles';
            if (tableName == 'Users') {
                url = '/Accounts/GetUsers';
            }
            return $http.get(url);
        },
        getDatasPagination: function (pagingInfo, tableName) {
            var url = '/Accounts/GetRolesPaging';
            if (tableName == 'Users') {
                url = '/Accounts/GetUsersPaging';
            }
            return $http.get(url, { params: pagingInfo });
        },
        getData: function (tableName) {
            var url = '/Accounts/GetRole';
            if (tableName == 'Users') {
                url = '/Accounts/GetUser';
            }
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
        removeData: function (Id, tableName) {
            var url = '/Accounts/DeleteRole';
            if (tableName == 'Users') {
                url = '/Accounts/DeleteUser';
            }
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

app.controller('RoleListCtlr', ['$scope', '$http', '$window', 'EMSService', function ($scope, $http, $window, EMSService) {
    $scope.btnDeleteText = "Delete";
    $scope.btnEditText = "Edit";
    $scope.redirect = function (Id) {
        $window.location.href = '/SuperAdmin/Accounts/UpdateRole/' + Id;
    };

    $scope.removeData = function (Id) {
        EMSService.removeData(Id, 'Roles').then(function (data) {
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
        EMSService.getDatasPagination($scope.pagingInfo, 'Roles').then(function (result) {
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
        }, function (error) { alert(err.data);});
    }

    // initial table load
    load();

}]);

app.controller('RoleAddCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnSaveText = "Save";
    $scope.showMessage = false;
    $scope.addData = function () {
        var role = {};
        role.Name = $scope.Name;
        EMSService.createData(role, 'Roles');
    };
}]);

app.controller('RoleEditCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    EMSService.getData('Roles').then(function (result) {
        $scope.Name = result.data.Name;
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
        EMSService.updateData(role, 'Roles');
        $scope.btnUpdateText = "Update";
    };
}]);

app.controller('UserListCtlr', ['$scope', '$http', '$window', 'EMSService', function ($scope, $http, $window, EMSService) {
    $scope.btnDeleteText = "Delete";
    $scope.btnEditText = "Edit";
    $scope.redirect = function (Id) {
        $window.location.href = '/SuperAdmin/Accounts/UpdateUser/' + Id;
    };

    $scope.removeData = function (Id) {
        EMSService.removeData(Id, 'Users').then(function (data) {
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
        EMSService.getDatasPagination($scope.pagingInfo, 'Users').then(function (result) {
            $scope.users = result.data.Users;

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

app.controller('UserAddCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnSaveText = "Save";
    $scope.showMessage = false;
    EMSService.getDatas('Roles').then(function (result) {
        $scope.roles = result.data.Roles;
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
        //alert($scope.ddlRole.Name);
        var selectedRoles = $scope.selectedRoles;
        EMSService.createData(user, selectedRoles, 'Users');
    };
}]);

app.controller('UserEditCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    EMSService.getData('Users').then(function (result) {
        $scope.UserName = result.data.UserName;
        $scope.Email = result.data.Email;
        $scope.FirstName = result.data.FirstName;
        $scope.LastName = result.data.LastName;
        //alert(result.data.Name);
        //alert(result.data.toSource());
    }, function (error) {
        alert('Error');
    }

    );
    $scope.updateData = function () {
        $scope.btnUpdateText = "Updating...";
        var user = {};
        user.Email = $scope.Email;
        user.FirstName = $scope.FirstName;
        user.LastName = $scope.LastName;
        user.InstituteId = 0;
        EMSService.updateData(user, 'Users');
        $scope.btnUpdateText = "Update";
    };
}]);

