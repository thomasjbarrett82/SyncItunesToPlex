namespace Core.Data {
    public interface IRestApiService {
        /// <summary>
        /// Submits a GET request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns>RestApiResponse</returns>
        RestApiResponse<T> Get<T>(RestApiRequest request);

        /// <summary>
        /// Submits a POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        RestApiResponse<T> Post<T>(RestApiRequest request);

        /// <summary>
        /// Submits a PUT request with no response data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RestApiResponse Put(RestApiRequest request);

        /// <summary>
        /// Submits a PUT request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        RestApiResponse<T> Put<T>(RestApiRequest request);

        /// <summary>
        /// Submits a DELETE request with no response data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RestApiResponse Delete(RestApiRequest request);
    }
}
