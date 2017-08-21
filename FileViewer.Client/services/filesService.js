app.factory('filesService', ['$resource',
      function ($resource) {
          return $resource('http://:host/:service/:action',
              {
                  host: 'localhost:9000',
                  service: 'api/files',
                  action: ''
              },
              {//
                  get: {
                      method: 'GET',
                      params: { dfg: 'wer' }
                  },
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