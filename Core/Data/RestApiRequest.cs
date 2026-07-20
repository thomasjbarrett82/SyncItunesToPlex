namespace Core.Data {
    public class RestApiRequest {
        public required string Uri { get; set; }
        public required List<RestApiParam> Params { get; set; }
    }
}
