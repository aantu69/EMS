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
        createData: function (ViewModel, file) {
            var url = '/Institutes/CreateData';
            var redirect = '/SuperAdmin/Institutes/Index';
            var formData = new FormData();
            formData.append("ViewModel", ViewModel);
            formData.append("file", file);
            $http({
                url: url,
                method: "POST",
                data: formData
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) === 'object') {
                    addMessage(response.data);
                    window.location.href = redirect;
                }
            }, function (response) {

            });
        },
        updateData: function (ViewModel) {
            ViewModel.ID = this.getEditId();
            $http({
                url: "/Institutes/UpdateData",
                method: "POST",
                data: ViewModel
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) === 'object') {
                    window.location.href = '/SuperAdmin/Institutes/Index';
                }
            }, function (response) {

            })
        },
        getDatas: function (pagingInfo) {
            return $http.get('/Institutes/GetDatas', { params: pagingInfo });
        },
        getData: function () {
            var currentId = this.getEditId();
            if (currentId !== null) {
                return $http.get('/Institutes/GetData', { params: { id: currentId } });
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
                        url: '/Institutes/DeleteData',
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
    $scope.redirect = function (Id) {
        $window.location.href = '/SuperAdmin/Institutes/Edit/' + Id;
    };

    $scope.removeData = function (Id) {
        EMSService.removeData(Id).then(function (data) {
            load();
        });
    }

    $scope.pagingInfo = {
        page: 1,
        pageSize: 5,
        sortBy: 'ShortName',
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
        $scope.insts = null;
        EMSService.getDatas($scope.pagingInfo).then(function (result) {
            $scope.insts = result.data.Insts;
            //alert(result.data.Insts.toSource());

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
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    //$scope.JoinDate = "15-10-2017";
    $scope.addData = function () {
        var inst = {};
        inst.Name = $scope.Name;
        inst.ShortName = $scope.ShortName;
        inst.Address = $scope.Address;
        inst.Email = $scope.Email;
        inst.Phone = $scope.Phone;
        inst.Mobile = $scope.Mobile;
        inst.Contact = $scope.Contact;
        inst.JoinDate = $scope.JoinDate;
        inst.ExpireDate = $scope.ExpireDate;
        inst.IsActive = $scope.IsActive;
        var file = $scope.SelectedFileForUpload;
        EMSService.createData(inst, file);
    };
}]);

app.controller('EditCtlr', ['$scope', '$http', '$filter', 'EMSService', function ($scope, $http, $filter, EMSService) {
    $scope.btnUpdateText = "Update";
    $scope.showMessage = false;
    EMSService.getData().then(function (result) {
        //alert(result.data.toSource());
        $scope.Name = result.data.Name;
        $scope.ShortName = result.data.ShortName;
        $scope.Address = result.data.Address;
        $scope.Email = result.data.Email;
        $scope.Phone = result.data.Phone;
        $scope.Mobile = result.data.Mobile;
        $scope.Contact = result.data.Contact;
        //$scope.JoinDate = result.data.JoinDate;

        $scope.JoinDate = $.datepicker.formatDate("mm/dd/yy", new Date(parseInt(result.data.JoinDate.substr(6))));
        //$scope.JoinDate = $filter("date")(1508349600000, "MM/dd/yyyy");
        $scope.IsActive = result.data.IsActive;
        //$scope.ExpireDate = result.data.ExpireDate;
        //$scope.ExpireDate = new Date(parseInt(result.data.ExpireDate.substr(6), "MM/dd/yyyy"));
        $scope.ExpireDate = $.datepicker.formatDate("mm/dd/yy", new Date(parseInt(result.data.ExpireDate.substr(6))));
        //$scope.ExpireDate = new Date(+result.data.ExpireDate.replace(/\/Date\((\d+)\)\//, '$1'));
        //alert(result.data.Name);
        //alert(result.data.toSource());
    }, function (error) {
        alert('Error');
    }

    );
    $scope.updateData = function () {
        $scope.btnUpdateText = "Updating...";
        var inst = {};
        inst.Name = $scope.Name;
        inst.ShortName = $scope.ShortName;
        inst.Address = $scope.Address;
        inst.Email = $scope.Email;
        inst.Phone = $scope.Phone;
        inst.Mobile = $scope.Mobile;
        inst.Contact = $scope.Contact;
        inst.JoinDate = $scope.JoinDate;
        inst.ExpireDate = $scope.ExpireDate;
        inst.IsActive = $scope.IsActive;
        EMSService.updateData(inst);
        $scope.btnUpdateText = "Update";
    };
}]);

