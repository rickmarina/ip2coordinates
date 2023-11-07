using System.Text.Json;

internal class IpLocationService {

    private readonly IHttpClientFactory _httpClientFactory;
    private const string URL = "https://iplocation.com"; 
    private Dictionary<string, string> HEADERS = new() {
        {"Referer", "https://iplocation.com/"},
        {"Origin", "https://iplocation.com/"},
        {"Accept", "*/*"},
        {"Accept-Enconding", "gzip, defltate, br"},
        {"Accept-Language", "es-ES,es;q=0.9,en;q=0.8"},
        {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36"}
    };
    private long timestamp {get;set;}
    private int timestamp_times;
    private const int USAGE_LIMIT_PER_SECOND = 50;
    public IpLocationService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        timestamp_times = 0;
    }

    public async Task<IpLocationResponseModel?> Locate(string ipaddress) {
        string response = await Request(ipaddress);

        return JsonSerializer.Deserialize<IpLocationResponseModel>(response);
    }
    private async Task<string> Request(string ipaddress) { 
        
        long currentTs = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (currentTs == timestamp) 
            timestamp_times++;
        else   
            timestamp_times = 1;
        timestamp = currentTs;

        if (timestamp_times >= USAGE_LIMIT_PER_SECOND) {
            throw new Exception($"Usage limit exceded. {USAGE_LIMIT_PER_SECOND} r/s");
        }

        using (var httpClient = _httpClientFactory.CreateClient()) { 
            foreach (var kv in HEADERS) 
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(kv.Key, kv.Value);
            
            var formContent = new FormUrlEncodedContent(new [] {
                new KeyValuePair<string,string>("ip", ipaddress)
            });

            var response = await httpClient.PostAsync(URL,formContent);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            return body;
            
        }
    }


}