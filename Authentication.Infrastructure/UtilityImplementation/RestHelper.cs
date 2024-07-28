namespace Authentication.Infrastructure.ExternalServiceImplementation;


public class RestHelper : IRestHelper
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _client;

    public RestHelper(IHttpClientFactory client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<Result<TResponse>> ConsumeApi<TResponse>(string httpNamedClient, string url, object payload,
        string serviceProvider, ApiType type = ApiType.Get, Dictionary<string, string> headers = null,
        bool logRequest = true, bool logResponse = true)
    {
        _logger.Information($"URL ==> {url}");
        var apiResult = new Result<TResponse>();
        var jsonSerializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        try
        {
            var client = _client.CreateClient("AuthService");
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(url);
            switch (type)
            {
                case ApiType.Post:
                    requestMessage.Method = HttpMethod.Post;
                    break;
                case ApiType.Put:
                    requestMessage.Method = HttpMethod.Put;
                    break;
                case ApiType.Patch:
                    requestMessage.Method = HttpMethod.Patch;
                    break;
                case ApiType.Delete:
                    requestMessage.Method = HttpMethod.Delete;
                    break;
                default:
                    requestMessage.Method = HttpMethod.Get;
                    break;
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    requestMessage.Headers.Add(item.Key, item.Value);
                }
            }

            var serializePayload = JsonConvert.SerializeObject(payload);
            _logger.Information($"Payload ==> {serializePayload}");

            if (payload != null)
            {
                requestMessage.Content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response;
            switch (type)
            {
                case ApiType.Get when headers == null:
                    response = await client.GetAsync(url);
                    _logger.Information($"Response for user {serializePayload} ==> {JsonConvert.SerializeObject(response, jsonSerializerSettings)}");
                    break;
                case ApiType.Post when headers == null:
                    response = await client.PostAsync(url, requestMessage.Content);
                    _logger.Information($"Response for user {serializePayload} ==> {JsonConvert.SerializeObject(response, jsonSerializerSettings)}");
                    break;
                default:
                    response = await client.SendAsync(requestMessage);
                    _logger.Information($"Response for user {serializePayload} ==> {JsonConvert.SerializeObject(response, jsonSerializerSettings)}");
                    break;
            }
            
            var result = await response.Content.ReadAsStringAsync();
            _logger.Information($"Result for user {serializePayload} ==> {result}");

            if (response.IsSuccessStatusCode)
            {
                apiResult.ResponseDetails = JsonConvert.DeserializeObject<TResponse>(result);
                apiResult.ResponseMsg = "Successful";
                apiResult = Result<TResponse>.Success(apiResult.ResponseDetails, apiResult.ResponseMsg);
                _logger.Information($"ApiResult ==> {JsonConvert.SerializeObject(apiResult)}");
            }
            else
            {
                apiResult.ResponseDetails = JsonConvert.DeserializeObject<TResponse>(result);
                apiResult = Result<TResponse>.Failed(apiResult.ResponseDetails);
                _logger.Information($"ApiResult ==> {JsonConvert.SerializeObject(apiResult)}");
            }

            return apiResult;
        }
        catch (Exception ex)
        {
            apiResult.ResponseDetails = (TResponse)(object)null;
           
            apiResult.ResponseMsg = ex.Message;
            apiResult = Result<TResponse>.Error(apiResult.ResponseDetails, apiResult.ResponseMsg);
            _logger.Information($"ApiResult ==> {JsonConvert.SerializeObject(apiResult)}");
            return apiResult;
        }
    }
}