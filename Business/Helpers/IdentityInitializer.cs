using Business.Helpers;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                if (!_roleManager.RoleExistsAsync(Roles.ROLE_API_ACOMODACOES).Result)
                {
                    var result = _roleManager.CreateAsync(new IdentityRole(Roles.ROLE_API_ACOMODACOES)).Result;
                    if (!result.Succeeded) throw new Exception($"Erro durante a criação da role {Roles.ROLE_API_ACOMODACOES}");
                }

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "admin_apiacomodacoes",
                        Email = "admin-apiacomodacoes@teste.com.br",
                        EmailConfirmed = true
                    }, "AdminAPIAcomodacoes01!", Roles.ROLE_API_ACOMODACOES);

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "usrinvalido_apiacomodacoes",
                        Email = "usrinvalido-apiacomodacoes@teste.com.br",
                        EmailConfirmed = true
                    }, "UsrInvAPIAcomodacoes01!");
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
