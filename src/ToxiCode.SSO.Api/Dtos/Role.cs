namespace ToxiCode.SSO.Api.Dtos
{
    public class Role
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string[] Rules { get; set; } = null!;
    }
}