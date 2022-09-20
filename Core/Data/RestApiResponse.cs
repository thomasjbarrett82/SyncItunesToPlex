namespace Core.Data {
    public class RestApiResponse {
        public string Request { get; set; } // serialized object with endpoint and params
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string ResponseUri { get; set; } // created at location, for example
        public bool IsSuccessful { get; set; }
    }

    public class RestApiResponse<T> : RestApiResponse {
        public T Data { get; set; }
    }
}
