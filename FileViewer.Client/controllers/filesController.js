'use strict';

app.controller('filesController', ['$scope', 'filesService', function ($scope, filesService) {
    $scope.pathInfo;

    $scope.small = 1;
    $scope.middle = 2;
    $scope.big = 3;
    $scope.paths = ["a", "s", "d"];

    $scope.getPathInfo = function (path) {
        
        filesService.query({ path: 'E:\polygon\testTasks\FileViewer' }, function (value) {
            $scope.pathInfo = value.data;
            alert("success");
        }, function (error) {
            $scope.pathInfo = error.data;
            alert("err");
            // Error handler code
        });
    }
    alert("controller");
}]);

