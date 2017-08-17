app.factory('filesService', ['$resource',
      function ($resource) {
          return $resource('http://localhost:9000/api/files/:path', {}, {
              query: {
                  method: 'GET',
                  params: { path: '' },
                  isArray: false
              }
          });
      }
    ]);