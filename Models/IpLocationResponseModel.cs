    [Serializable]
    public class IpLocationResponseModel
    {
        public string? city { get; set; }
        public string? company { get; set; }
        public string? continent_code { get; set; }
        public string? country_code { get; set; }
        public string? country_name { get; set; }
        public int found { get; set; }
        public string? ip { get; set; }
        public string? ip_header { get; set; }
        public string? isp { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public object? metro_code { get; set; }
        public string? postal_code { get; set; }
        public string? region { get; set; }
        public string? region_name { get; set; }
        public string? time_zone { get; set; }
    }

