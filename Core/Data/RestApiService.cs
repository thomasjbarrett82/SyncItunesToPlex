using RestSharp;
using System.Text.Json;

namespace Core.Data {
    public class RestApiService : IRestApiService {
        private readonly RestClient _client;

        public RestApiService(string baseUrl) {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            _client = new RestClient(baseUrl);
        }

        public RestApiResponse<T> Get<T>(RestApiRequest req) {
            var request = new RestRequest(req.Uri);
            if (req.Params != null && req.Params.Count > 0) {
                foreach (var param in req.Params) {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var response = _client.Get(request);
            if (!response.IsSuccessful)
                throw new InvalidOperationException($"Get failed with {response.StatusCode}, {response.StatusDescription}: {req.Uri}");

            return new RestApiResponse<T> {
                Request = JsonSerializer.Serialize(response.Request),
                StatusCode = response.StatusCode.ToString(),
                StatusDescription = response.StatusDescription ?? string.Empty,
                ResponseUri = response.ResponseUri?.AbsoluteUri ?? string.Empty,
                IsSuccessful = response.IsSuccessful,
                Data = JsonSerializer.Deserialize<T>(response.Content)
            };
        }

        public RestApiResponse<T> Post<T>(RestApiRequest req) {
            var request = new RestRequest(req.Uri);
            if (req.Params != null && req.Params.Count > 0) {
                foreach (var param in req.Params) {
                    request.AddQueryParameter(param.Key, param.Value, true);
                }
            }

            var response = _client.Post(request);
            if (!response.IsSuccessful)
                throw new InvalidOperationException($"Post failed with {response.StatusCode}, {response.StatusDescription}: {req.Uri}");

            return new RestApiResponse<T> {
                Request = JsonSerializer.Serialize(response.Request),
                StatusCode = response.StatusCode.ToString(),
                StatusDescription = response.StatusDescription,
                ResponseUri = response.ResponseUri.AbsoluteUri,
                IsSuccessful = response.IsSuccessful,
                Data = JsonSerializer.Deserialize<T>(response.Content)
            };
        }

        public RestApiResponse Put(RestApiRequest req) {
            var request = new RestRequest(req.Uri);
            if (req.Params != null && req.Params.Count > 0) {
                foreach (var param in req.Params) {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            var response = _client.Put(request);
            if (!response.IsSuccessful)
                throw new InvalidOperationException($"Put failed with {response.StatusCode}, {response.StatusDescription}: {req.Uri}");

            return new RestApiResponse {
                Request = JsonSerializer.Serialize(response.Request),
                StatusCode = response.StatusCode.ToString(),
                StatusDescription = response.StatusDescription,
                ResponseUri = response.ResponseUri.AbsoluteUri,
                IsSuccessful = response.IsSuccessful
            };
        }

        public RestApiResponse<T> Put<T>(RestApiRequest req) {
            var request = new RestRequest(req.Uri);
            if (req.Params != null && req.Params.Count > 0) {
                foreach (var param in req.Params) {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            var response = _client.Put(request);
            if (!response.IsSuccessful)
                throw new InvalidOperationException($"Put failed with {response.StatusCode}, {response.StatusDescription}: {req.Uri}");

            return new RestApiResponse<T> {
                Request = JsonSerializer.Serialize(response.Request),
                StatusCode = response.StatusCode.ToString(),
                StatusDescription = response.StatusDescription,
                ResponseUri = response.ResponseUri.AbsoluteUri,
                IsSuccessful = response.IsSuccessful,
                Data = JsonSerializer.Deserialize<T>(response.Content)
            };
        }

        public RestApiResponse Delete(RestApiRequest req) {
            var request = new RestRequest(req.Uri);
            if (req.Params != null && req.Params.Count > 0) {
                foreach (var param in req.Params) {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            var response = _client.Delete(request);
            if (!response.IsSuccessful)
                throw new InvalidOperationException($"Delete failed with {response.StatusCode}, {response.StatusDescription}: {req.Uri}");

            return new RestApiResponse {
                Request = JsonSerializer.Serialize(response.Request),
                StatusCode = response.StatusCode.ToString(),
                StatusDescription = response.StatusDescription,
                ResponseUri = response.ResponseUri.AbsoluteUri,
                IsSuccessful = response.IsSuccessful
            };
        }
    }
}
