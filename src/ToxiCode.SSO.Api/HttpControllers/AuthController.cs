using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToxiCode.SSO.Api.Attributes;
using ToxiCode.SSO.Api.Dtos;
using ToxiCode.SSO.Api.Handlers;
using ToxiCode.SSO.Api.Handlers.Authenticate;
using ToxiCode.SSO.Api.Handlers.GetUser;
using ToxiCode.SSO.Api.Handlers.RefreshToken;
using ToxiCode.SSO.Api.Handlers.RegisterUser;
using ToxiCode.SSO.Api.Handlers.RevokeAuthentication;
using ToxiCode.SSO.Api.Infrastructure.Exceptions;
using ToxiCode.SSO.Api.Infrastructure.Extensions;

namespace ToxiCode.SSO.Api.HttpControllers
{
    [ApiController]
    [Route("/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
            => _mediator = mediator;
        
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticateResponse>> Register([FromBody] RegisterUserCommand request)
        {
            try
            {
                request.IpAddress = HttpContext.GetIpAddress();
                var response = await _mediator.Send(request);
                Response.SetTokenCookie(response.RefreshToken);
                return response;
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody] AuthenticateCommand request)
        {
            try
            {
                request.IpAddress = HttpContext.GetIpAddress();
                var response = await _mediator.Send(request);
                Response.SetTokenCookie(response.RefreshToken);
                return response;
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"] ?? "";
            var isExtended = !string.IsNullOrEmpty(Request.Cookies["isExtended"]);
            try
            {
                var response = await _mediator.Send(new RefreshTokenCommand(refreshToken, HttpContext.GetIpAddress()));
                Response.SetTokenCookie(response.RefreshToken, isExtended);
                return Ok(response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [JwtAuthorize]
        [HttpGet("verify")]
        public async Task<ActionResult<User>> Verify()
        {
            var userMeta = (User?) HttpContext.Items["User"];
            var cmd = new GetUserCommand(userMeta!.Id);
            var result = await _mediator.Send(cmd);
            return result.User;
        }

        [JwtAuthorize]
        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeAuthentication()
        {
            var refreshToken = Request.Cookies["refreshToken"] ?? "";
            try
            {
                await _mediator.Send(new RevokeAuthenticationCommand(refreshToken, HttpContext.GetIpAddress()));
                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}