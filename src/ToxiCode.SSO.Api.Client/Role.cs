namespace ToxiCode.SSO.Api.Client
{
    public class Role
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string[] Rules { get; set; } = null!;
    }
}