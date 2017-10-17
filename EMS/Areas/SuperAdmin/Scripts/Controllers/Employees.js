var employeeModule = angular.module("employeeModule", []);

employeeModule.factory("employeeService", ['$http', '$window', function ($http, $window) {
    return {
        createData: function (Employees) {
            $http({
                url: "/Employees/CreateData",
                method: "POST",
                data: Employees
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = '/SuperAdmin/Employees/Index';
                }
            }, function (response) {

            })
        },
        updateData: function (Employees) {
            Employees.ID = this.getEditId();
            $http({
                url: "/Employees/UpdateData",
                method: "POST",
                data: Employees
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = '/SuperAdmin/Employees/Index';
                }
            }, function (response) {

            })
        },
        getDatas: function (pagingInfo) {
            return $http.get('/Employees/GetDatas', { params: pagingInfo });
        },
        getData: function () {
            var currentId = this.getEditId();
            if (currentId != null) {
                return $http.get('/Employees/GetData', { params: { id: currentId } });
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
            if (Id != null && parseInt(Id, 10) > 0) {
                if (window.confirm('Are you sure, you want to delete this record?')) {
                    return $http({
                        url: '/Employees/DeleteData',
                        method: 'POST',
                        data: { Id: Id }
                    });
                }

            }
        },
    }
}]);

employeeModule.controller('EmployeeListCtlr', ['$scope', '$http', '$window', 'employeeService', function ($scope, $http, $window, employeeService) {
    $scope.btnDeleteText = "Delete";
    $scope.btnEditText = "Edit";
    $scope.redirect = function (ID) {
        $window.location.href = '/SuperAdmin/Employees/Edit/' + ID;
    };

    $scope.removeData = function (ID) {
        employeeService.removeData(ID).then(function (data) {
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
        $scope.employees = null;
        employeeService.getDatas($scope.pagingInfo).then(function (result) {
            $scope.employees = result.data.Employees;

            $scope.pagingInfo.totalPages = result.data.TotalPages;
            $scope.pagingInfo.pageSize = $scope.pagingInfo.pageSize;
            $scope.pageSizeOption = [ 5, 10 ];            
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

employeeModule.controller('EmployeeAddCtlr', ['$scope', '$http', 'employeeService', function ($scope, $http, employeeService) {
    $scope.btnSaveText = "Save";
    $scope.showMessage = false;
    $scope.addData = function () {
        var employee = {};
        employee.Name = $scope.Name;
        employee.Address = $scope.Address;
        employeeService.createData(employee);
    };
}]);

employeeModule.controller('EmployeeEditCtlr', ['$scope', '$http', 'employeeService', function ($scope, $http, employeeService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    employeeService.getData().then(function (result) {
        $scope.Name = result.data.Name;
        $scope.Address = result.data.Address;
        //alert(result.data.Name);
        //alert(result.data.toSource());
    }, function (error) {
        alert('Error');
    }

    );
    $scope.updateData = function () {
        $scope.btnUpdateText = "Updating...";
        var employee = {};
        employee["Name"] = $scope.Name;
        employee["Address"] = $scope.Address;
        employeeService.updateData(employee);
        $scope.btnUpdateText = "Update";
    };
}]);

