using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ToxiCode.SSO.Api.Handlers.RevokeAuthentication
{
    public record RevokeAuthenticationCommand(string Token, string IpAddress) : IRequest<EmptyResult>;
}