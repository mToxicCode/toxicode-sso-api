using AutoMapper;
using ToxiCode.SSO.Api.DataLayer.Cmds;
using ToxiCode.SSO.Api.Handlers.Authenticate;
using ToxiCode.SSO.Api.Handlers.RefreshToken;
using ToxiCode.SSO.Api.Handlers.RevokeAuthentication;

namespace ToxiCode.SSO.Api.Infrastructure.MappingProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RefreshTokenCommand, GetUserByRefreshTokenCmd>()
                .ForMember(x => x.RefreshToken, opt => opt.MapFrom(y => y.Token));

            CreateMap<RefreshTokenCommand, UpdateRefreshTokenCmd>()
                .ForMember(x => x.Token, opt => opt.MapFrom(y => y.Token))
                .ForMember(x => x.RevokedByIp, opt => opt.MapFrom(y => y.IpAddress));

            CreateMap<RevokeAuthenticationCommand, UpdateRefreshTokenCmd>()
                .ForMember(x => x.RevokedByIp, opt => opt.MapFrom(y => y.IpAddress));
            
            CreateMap<AuthenticateCommand, GetUserByUsernameCmd>();
        }
    }
}