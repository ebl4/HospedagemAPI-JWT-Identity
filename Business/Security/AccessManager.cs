using Business.Helpers;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Business.Security
{
    public class AccessManager
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private SigningConfigurations _signInConfiguration;
        private TokenConfigurations _tokenConfigurations;

        public AccessManager(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signInConfiguration = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        public bool ValidateCredentials(AccessCredentials credenciais)
        {
            bool credenciaisValidas = false;

            if(credenciais != null && !string.IsNullOrWhiteSpace(credenciais.UserID))
            {
                var userIdentity = _userManager.FindByNameAsync(credenciais.UserID).Result;
                if(userIdentity != null)
                {
                    var resultLogin = _signInManager.CheckPasswordSignInAsync(userIdentity, credenciais.Password, false).Result;
                    if (resultLogin.Succeeded)
                    {
                        // Verifica se o usuário em questão possui 
                        // a role Acesso-HospedagemAPI
                        credenciaisValidas = _userManager.IsInRoleAsync(userIdentity, Roles.ROLE_HOSPEDAGEM_API).Result;
                    }
                }
            }

            return credenciaisValidas;
        }

        public Token GenerateToken(AccessCredentials credenciais)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(credenciais.UserID, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, credenciais.UserID)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signInConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new Token()
            {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "Ok"
            };
        }
    }
}
