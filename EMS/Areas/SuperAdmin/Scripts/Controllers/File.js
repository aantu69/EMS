var app = angular.module('app', []);
app.controller('AddCtrl', function ($scope, FileUploadService) {
    // Variables
    $scope.Message = "";
    $scope.FileInvalidMessage = "";
    $scope.SelectedFileForUpload = null;
    $scope.FileDescription = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFileValid = false;
    $scope.IsFormValid = false;

    //Form Validation
    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });


    // THIS IS REQUIRED AS File Control is not supported 2 way binding features of Angular
    // ------------------------------------------------------------------------------------
    //File Validation
    $scope.ChechFileValid = function (file) {
        var isValid = false;
        if ($scope.SelectedFileForUpload != null) {
            if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif') && file.size <= (512 * 1024)) {
                $scope.FileInvalidMessage = "";
                isValid = true;
            }
            else {
                $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)";
            }
        }
        else {
            $scope.FileInvalidMessage = "Image required!";
        }
        $scope.IsFileValid = isValid;
    };

    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    //----------------------------------------------------------------------------------------

    //Save File
    $scope.SaveFile = function () {
        $scope.IsFormSubmitted = true;
        $scope.Message = "";
        $scope.ChechFileValid($scope.SelectedFileForUpload);
        if ($scope.IsFormValid && $scope.IsFileValid) {
            //var model = {};
            var model = {
                "Description": $scope.FileDescription,
                //"file": $scope.SelectedFileForUpload
            };
            //model.Description = $scope.FileDescription;
            FileUploadService.UploadFile(model, $scope.SelectedFileForUpload).then(function (d) {
                alert(d.Message);
                ClearForm();
            }, function (e) {
                alert(e);
            });
        }
        else {
            $scope.Message = "All the fields are required.";
        }
    };
    //Clear form 
    function ClearForm() {
        $scope.FileDescription = "";
        //as 2 way binding not support for File input Type so we have to clear in this way
        //you can select based on your requirement
        angular.forEach(angular.element("input[type='file']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });

        $scope.f1.$setPristine();
        $scope.IsFormSubmitted = false;
    }

});
app.factory('FileUploadService', function ($http, $q) { // explained abour controller and service in part 2
    return {
        UploadFile: function (model, file) {
            var url = '/UploadedFiles/SaveFiles';
            var redirect = '/SuperAdmin/Users/Index';
            //var formData = new FormData();
            //formData.append("model", model);
            //formData.append("file", file);
            var defer = $q.defer();
            $http({
                url: url,
                method: "POST",
                headers: { "Content-Type": undefined },
                data: { 'model': model, 'file': file },
                transformRequest: function(data) {
                    var formData = new FormData();
                    formData.append("model", angular.toJson(data.model));
                    formData.append("file", file);
                    //for (var i = 0; i < data.files.length; i++) {
                    //    formData.append("files[" + i + "]", data.files[i]);
                    //}
                    
                },
                //data: { user: $scope.user, files: $scope.files }
            }).then(function (response) {
                if (response !== 'undefined' && typeof (response) == 'object') {
                    window.location.href = redirect;
                }
            }, function (response) {

            });
            //$http({
            //    url: url,
            //    method: "POST",
            //    //data: { 'model': model, 'file': file },
            //    //data: formData,
            //    headers: { 'Content-Type': undefined },
            //    data: model,
            //    transformRequest: function (data, headersGetter) {
            //        var formData = new FormData();
            //        angular.forEach(data, function (value, key) {
            //            formData.append(key, value);
            //        });
            //        return formData;
            //    }
            //}).then(function (response) {
            //    if (response !== 'undefined' && typeof (response) == 'object') {
            //        window.location.href = redirect;
            //    }
            //}, function (response) {

            //});
            //$http.post(url, formData,
            //    {
            //        withCredentials: true,
            //        headers: { 'Content-Type': undefined },
            //        //transformRequest: angular.identity
            //    })
            //.success(function (d) {
            //    defer.resolve(d);
            //})
            //.error(function () {
            //    defer.reject("File Upload Failed!");
            //});

            //return defer.promise;
        }
    }

});