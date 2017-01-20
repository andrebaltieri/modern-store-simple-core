using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModernStore.Data.Transaction;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Handlers;
using ModernStore.Domain.Repositories;
using ModernStore.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ModernStore.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserRepository _repository;
        private readonly UserCommandHandler _handler;
        private readonly TokenOptions _tokenOptions;
        private readonly JsonSerializerSettings _serializerSettings;

        public AccountController(IUnitOfWork uow, IUserRepository repository, UserCommandHandler handler, IOptions<TokenOptions> jwtOptions) : base(uow)
        {
            _repository = repository;
            _handler = handler;
            _tokenOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_tokenOptions);

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        [HttpPost]
        [Route("v1/users")]
        [AllowAnonymous]        
        public IActionResult Post([FromBody]RegisterUserCommand command)
        {
            _handler.Handle(command);
            return Response(command, "Usuário cadastrado com sucesso!", "Falha ao registrar", _handler.Notifications);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/authenticate")]
        public async Task<IActionResult> Post([FromForm] AuthenticateUserCommand command)
        {
            var identity = await GetClaimsIdentity(command);
            if (identity == null)
                return BadRequest(new { message = "Usuário ou senha inválidos" });

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, command.Username),
                new Claim(JwtRegisteredClaimNames.NameId, command.Username),
                new Claim(JwtRegisteredClaimNames.Email, command.Username),
                new Claim(JwtRegisteredClaimNames.Sub, command.Username),
                new Claim(JwtRegisteredClaimNames.Jti, await _tokenOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_tokenOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("ModernStore")
            };

            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: claims.AsEnumerable(),
                notBefore: _tokenOptions.NotBefore,
                expires: _tokenOptions.Expiration,
                signingCredentials: _tokenOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_tokenOptions.ValidFor.TotalSeconds,
                user = new { username = command.Username }
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private static void ThrowIfInvalidOptions(TokenOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("O período deve ser maior que zero", nameof(TokenOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(TokenOptions.JtiGenerator));
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private Task<ClaimsIdentity> GetClaimsIdentity(AuthenticateUserCommand command)
        {
            var user = _repository.Get(command.Username, command.Password);

            if (user == null)
                return Task.FromResult<ClaimsIdentity>(null);

            return Task.FromResult(new ClaimsIdentity(
                new GenericIdentity(user.Username, "Token"),
                new[] {
                    new Claim("ModernStore", user.Role.ToString())
                }));
        }

    }
}