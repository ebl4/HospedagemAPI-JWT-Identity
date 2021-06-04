using Business.Helpers;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;
using System;

namespace HospedagemAPI.Security
{
    public class IdentityInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                if (!_roleManager.RoleExistsAsync(Roles.ROLE_HOSPEDAGEM_API).Result)
                {
                    var result = _roleManager.CreateAsync(new IdentityRole(Roles.ROLE_HOSPEDAGEM_API)).Result;
                    if (!result.Succeeded) throw new Exception($"Erro durante a criação da role {Roles.ROLE_HOSPEDAGEM_API}");
                }

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "admin_hospedagemAPI",
                        Email = "admin-hospedagemAPI@teste.com.br",
                        EmailConfirmed = true
                    }, "AdminHospedagemAPI01!", Roles.ROLE_HOSPEDAGEM_API);

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "usrinvalido_hospedagemAPI",
                        Email = "usrinvalido-hospedagemAPI@teste.com.br",
                        EmailConfirmed = true
                    }, "UsrInvHospedagemAPI01!");
            }
        }

        public void CreateUser(ApplicationUser user, string password, string initialRole = null)
        {
            if(_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var result = _userManager.CreateAsync(user, password).Result;

                if (result.Succeeded && !string.IsNullOrWhiteSpace(initialRole)) 
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
            }
        }
    }
}
