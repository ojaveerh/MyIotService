namespace DeviceApi.Models
{
    public class RegisterDeviceRequest
    {
        public int DeviceNr { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Username { get; set; }
        public int DataId { get; set; }
        public string DataName { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public int Value { get; set; }

    }
}
