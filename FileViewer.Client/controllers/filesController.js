'use strict';

app.controller('filesController', ['$scope', 'filesService', function ($scope, filesService) {
    //alert("controller");

    $scope.pathInfo = {
        RootPath: "rootPath",
        SubNodes: ["node1", "node2", "node3"],
        SmallFiles: 1,
        MiddleFiles: 2,
        BigFiles: 3
    };

    filesService.get({}, function (data) {
        $scope.pathInfo = data;
        if ($scope.pathInfo.SmallFiles === -1){
            $scope.count($scope.pathInfo.RootPath);
        }
    },
    function () {
        console.log("err");
    });

    $scope.transite = function (newPath) {

        var rootPath = {
            newPath: newPath,
            path: $scope.pathInfo.RootPath
        };

        filesService.transite({}, rootPath, function (data) {
            if (data.RootPath == null) {
                return;
            }
            $scope.pathInfo = data;
            if ($scope.pathInfo.SmallFiles === -1) {
                $scope.count($scope.pathInfo.RootPath);
            }
        },
        function () {
            console.log("err");
        }); //saves an entry. Assuming $scope.entry is the Entry object
        return;
    };

    $scope.count = function (path) {

        $scope.pathInfo.SmallFiles = "please, wait...";
        $scope.pathInfo.MiddleFiles = "please, wait...";
        $scope.pathInfo.BigFiles = "please, wait...";

        var rootPath = {
            newPath: path,
            path: path
        };

        filesService.count({}, rootPath, function (data) {
            $scope.pathInfo.SmallFiles = data.SmallFiles;
            $scope.pathInfo.MiddleFiles = data.MiddleFiles;
            $scope.pathInfo.BigFiles = data.BigFiles;
        },
        function () {
            console.log("err");
        }); //saves an entry. Assuming $scope.entry is the Entry object
        return;
    };
}]);

