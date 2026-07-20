namespace Core.Data {
    public class RestApiResponse {
        public required string Request { get; set; } // serialized object with endpoint and params
        public required string StatusCode { get; set; }
        public required string StatusDescription { get; set; }
        public required string ResponseUri { get; set; } // created at location, for example
        public bool IsSuccessful { get; set; }
    }

    public class RestApiResponse<T> : RestApiResponse {
        public required T Data { get; set; }
    }
}
