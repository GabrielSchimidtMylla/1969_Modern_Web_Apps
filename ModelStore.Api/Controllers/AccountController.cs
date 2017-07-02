using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentValidator;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModelStore.Api.Security;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ModelStore.Api.Controllers
{
    public class AccountController : BaseController
    {
        private Customer _customer;
        private readonly ICustomerRepository _customerRepository;
        private readonly TokenOptions _tokenOptions;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public AccountController(IOptions<TokenOptions> options, IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
            : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _tokenOptions = options.Value;
            ThrowIfInvalidOptions(_tokenOptions);

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/authenticate")]
        public async Task<IActionResult> Post([FromForm] AuthenticateUserCommand command)
        {
            if (command == null)
                return await Response(null, new List<Notification> {new Notification("User", "Usuário ou Senha inválidos")});

            var identity = await GetClaims(command);

            if(identity == null)
                return await Response(null, new List<Notification> { new Notification("User", "Usuário ou Senha inválidos") });

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, _customer.User.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, _customer.Name.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _customer.Email),
                new Claim(JwtRegisteredClaimNames.Sub, command.Username),
                new Claim(JwtRegisteredClaimNames.Jti, await _tokenOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_tokenOptions.IssuedAt).ToString(),ClaimValueTypes.Integer64),
                identity.FindFirst("ModernStore")
            };
            //jwt == Json Web Token
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
                token = encodedJwt,
                expires = (int) _tokenOptions.ValidFor.TotalSeconds,
                user = new
                {
                    name = _customer.Name.ToString(),
                    email = _customer.Email,
                    username = _customer.User.UserName
                }
            };

            var json = JsonConvert.SerializeObject(response, _jsonSerializerSettings);

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

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);

        private Task<ClaimsIdentity> GetClaims(AuthenticateUserCommand command)
        {
            var customer = _customerRepository.GetByUserName(command.Username);

            if (customer == null)
                return Task.FromResult<ClaimsIdentity>(null);

            _customer = customer;

            if (!customer.User.Authenticate(command.Username, command.Password))
                return Task.FromResult<ClaimsIdentity>(null);

            return Task.FromResult(new ClaimsIdentity(
                new GenericIdentity(customer.User.UserName, "Token"),
                new[]
                {
                    new Claim("ModernStore", "User")
                }));
        }
    }
}
