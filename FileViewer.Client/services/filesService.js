app.factory('filesService', ['$resource',
      function ($resource) {
          return $resource('http://localhost:9000/api/files/:action', {}, {
              transite: {
                  method: 'POST',
                  params: { action: 'transite' },
                  headers: {
                      'Content-Type': 'application/json',
                      "Access-Control-Allow-Origin": "*"
                  },
                  hasBody: true
              },
              count: {
                  method: 'POST',
                  params: { action: 'count' },
                  headers: {
                      'Content-Type': 'application/json',
                      "Access-Control-Allow-Origin": "*"
                  },
                  hasBody: true
              }
          })
      }
]);