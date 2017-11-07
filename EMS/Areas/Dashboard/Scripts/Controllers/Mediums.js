var app = angular.module("app", []);

app.factory("EMSService", ['$http', '$window', function ($http, $window) {
    return {
        createData: function (ViewModel) {
            var url = '/Mediums/CreateData';
            var redirect = '/Dashboard/Mediums/Index';
            $http({
                url: url,
                method: "POST",
                data: ViewModel,
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        updateData: function (ViewModel) {
            ViewModel.Id = this.getEditId();
            var url = '/Mediums/UpdateData';
            var redirect = '/Dashboard/Mediums/Index';
            $http({
                url: url,
                method: "POST",
                data: ViewModel,
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            })
        },
        getDatas: function (pagingInfo) {
            return $http.get('/Mediums/GetDatas', { params: pagingInfo });
        },
        getShifts: function () {
            return $http.get('/Mediums/GetShifts');
        },
        getSelectedShifts: function () {
            var url = '/Mediums/GetSelectedShifts';
            var currentId = this.getEditId();
            if (currentId != null) {
                return $http.get(url, { params: { Id: currentId } });
            }
        },
        
        getData: function () {
            var currentId = this.getEditId();
            if (currentId != null) {
                return $http.get('/Mediums/GetData', { params: { Id: currentId } });
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
                        url: '/Mediums/DeleteData',
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
        $window.location.href = '/Dashboard/Mediums/Edit/' + Id;
    };

    $scope.removeData = function (Id) {
        EMSService.removeData(Id).then(function (data) {
            load();
        });
    }

    //EMSService.getShifts().then(function (result) {
    //    $scope.shifts = result.data;
    //}, function (error) {
    //    alert('Error');
    //});

    $scope.pagingInfo = {
        page: 1,
        pageSize: 5,
        sortBy: 'MediumName',
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
        $scope.mediums = null;
        EMSService.getDatas($scope.pagingInfo).then(function (result) {
            $scope.mediums = result.data.Mediums;

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
    
    EMSService.getShifts().then(function (result) {
        $scope.shifts = result.data;
    }, function (error) {
        alert('Error');
    });

    $scope.selectedShifts = [];
    $scope.toggleSelection = function toggleSelection(selectedId) {
        var idx = $scope.selectedShifts.indexOf(selectedId);
        if (idx > -1) {//is currently selected
            $scope.selectedShifts.splice(idx, 1);
        }
        else {//is newly selected
            $scope.selectedShifts.push(selectedId);
        }
    };
    $scope.addData = function () {
        var medium = {};
        medium.MediumName = $scope.MediumName;
        //var selectedShifts = $scope.selectedShifts;
        //EMSService.createData(medium, selectedShifts);
        EMSService.createData(medium);
    };
}]);

app.controller('EditCtlr', ['$scope', '$http', 'EMSService', function ($scope, $http, EMSService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    EMSService.getData().then(function (result) {
        $scope.MediumName = result.data.MediumName;
        //alert(result.data.Name);
        //alert(result.data.toSource());
    }, function (error) {
        alert('Error');
    });
    EMSService.getShifts().then(function (result) {
        $scope.shifts = result.data;
    }, function (error) {
        alert('Error');
    });

    EMSService.getSelectedShifts().then(function (result) {
        $scope.selectedShifts = result.data;
    });

    $scope.toggleSelection = function toggleSelection(selectedId) {
        var idx = $scope.selectedShifts.indexOf(selectedId);
        if (idx > -1) {//is currently selected
            $scope.selectedShifts.splice(idx, 1);
        }
        else {//is newly selected
            $scope.selectedShifts.push(selectedId);
        }
    };

    $scope.updateData = function () {
        $scope.btnUpdateText = "Updating...";
        var medium = {};
        medium.MediumName = $scope.MediumName;
        //var selectedShifts = $scope.selectedShifts;
        //EMSService.updateData(medium, selectedShifts);
        EMSService.updateData(medium);
        $scope.btnUpdateText = "Update";
    };
}]);

