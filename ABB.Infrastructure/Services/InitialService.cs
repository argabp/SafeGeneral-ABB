using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.Navigations.Commands;
using ABB.Application.RoleLevels.Commends;
using ABB.Application.RoleRoutes.Commands;
using ABB.Application.Roles.Commends;
using ABB.Application.Routes.Commands;
using ABB.Application.Users.Commands;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ABB.Infrastructure.Services
{
    public class InitialService : IInitialService
    {
        private readonly IMediator _mediator;
        private readonly ILog _log;
        private readonly IDbContext _db;
        private readonly IConfiguration _config;
        private readonly string ADMIN = "ADMIN";

        private readonly string[] ParentNavigation = new string[]
        {
            "Dashboard"
        };

        private readonly string[] SubNavigation = new string[]
        {

            "User Management",
            "Role Management",
            "Role for Menu",
            "Menu Management",
            "URL Management",
            "Role for URL",
        };

        public InitialService(IDbContext db, IMediator mediator, ILog log, IConfiguration config)
        {
            _db = db;
            _mediator = mediator;
            _log = log;
            _config = config;
        }

        public async Task Execute()
        {
            try
            {
                await _mediator.Send(new GenerateRouteCommand());
                await GenerateNavigations();
                await CreateDefaultRoles();
                await CreateAdminUser();
                await RegisterAdminToRole();
                await MapNavigationToAdmin();
                await MapRouteToAdmin();
            }
            catch (Exception ex)
            {
                _log.Error(ex, MethodBase.GetCurrentMethod());
            }
        }

        private async Task CreateDefaultRoles()
        {
            await AddRole(0, ADMIN, "Administrator ");
            await AddRole(1, "User", "User");
            await AddRoleLevel(0, 1);
            await AddRoleLevel(1, 2);
        }

        private async Task AddRole(int code, string name, string description)
        {
            if (_db.Role.Any(a => a.Name == name)) return;
            await _mediator.Send(new AddRoleCommand()
            {
                RoleCode = code,
                Name = name,
                Description = description
            });
        }

        private async Task AddRoleLevel(int code, int parentCode)
        {
            var roleId = _db.Role.FirstOrDefault(a => a.RoleCode == code)?.Id;
            if (_db.RoleLevel.Any(a => a.RoleId == roleId)) return;
            await _mediator.Send(new AddRoleLevelCommand()
            {
                RoleCode = code,
                ParentCode = parentCode
            });
        }

        private async Task CreateAdminUser()
        {
            var roleId = _db.Role.FirstOrDefault(a => a.Name == ADMIN).Id;
            var password = _config.GetSection("Administrator").GetSection("Password").Value;
            var command = new AddUserCommand
            {
                UserName = _config.GetSection("Administrator").GetSection("Username").Value,
                Password = password,
                ConfirmPassword = password,
                Email = "administrator@abb.com",
                RoleId = roleId,
                RoleName = ADMIN,
                FirstName = "ABB",
                LastName = "Admin",
                Photo = "default-profile-picture.png",
                CreatedBy = "System",
                PasswordExpiredDate = DateTime.Now.AddYears(1),
                IsActive = true,
                LockoutEnabled = false
            };
            if (_db.User.Any(a => a.UserName == command.UserName)) return;
            await _mediator.Send(command);
        }

        private async Task GenerateNavigations()
        {
            await AddNavigation(new AddNavigationCommand()
            {
                Action = "Index",
                Controller = "Home",
                Text = ParentNavigation[0],
                Icon = "fa-th"
            });

            // await AddNavigation(new AddNavigationCommand()
            // {
            //     Action = "",
            //     Controller = "",
            //     Text = ParentNavigation[1],
            //     Icon = "fa-bars"
            // });

            var setupNavigation = _db.Navigation.FirstOrDefault(w => w.Text == "Setup");

            await AddNavigation(new AddNavigationCommand()
            {
                Action = "Index",
                Controller = "User",
                Text = SubNavigation[0],
                Icon = "fa-user",
                ParentId = setupNavigation.ParentId
            });

            await AddNavigation(new AddNavigationCommand()
            {
                Action = "Index",
                Controller = "Role",
                Text = SubNavigation[1],
                Icon = "fa-user-friends",
                ParentId = setupNavigation.ParentId
            });

            await AddNavigation(new AddNavigationCommand()
            {
                Action = "Index",
                Controller = "RoleNavigation",
                Text = SubNavigation[2],
                Icon = "fa-circle",
                ParentId = setupNavigation.ParentId
            });

            await AddNavigation(new AddNavigationCommand()
            {
                Action = "Index",
                Controller = "Navigation",
                Text = SubNavigation[3],
                Icon = "fa-circle",
                ParentId = setupNavigation.ParentId
            });
            
            await AddNavigation(new AddNavigationCommand()
            {
                Action = "Index",
                Controller = "Route",
                Text = SubNavigation[4],
                Icon = "fa-circle",
                ParentId = setupNavigation.ParentId
            });
            
            await AddNavigation(new AddNavigationCommand()
            {
                Action = "Index",
                Controller = "RoleRoute",
                Text = SubNavigation[5],
                Icon = "fa-circle",
                ParentId = setupNavigation.ParentId
            });

        }

        private async Task<int> AddNavigation(AddNavigationCommand nav)
        {
            if (_db.Navigation.Any(a => a.Text == nav.Text)) return 0;
            return await _mediator.Send(nav);
        }

        private List<UserRole> GetUsers()
        {
            var roleAdmin = _db.Role.FirstOrDefault(w => w.RoleCode == 0);
            return  _db.UserRole.Where(w => w.RoleId == roleAdmin.Id).ToList();
        }

        private async Task MapNavigationToAdmin()
        {
            var adminUsers = GetUsers();
            var navigations = _db.Navigation.ToList();

            foreach (var adminUser in adminUsers)
            {
                foreach (var navigation in navigations)
                {
                    var command = new AddRoleNavigationCommand()
                    {
                        Text = navigation.Text,
                        NavigationId = navigation.NavigationId,
                        RoleId = adminUser.RoleId
                    };
                    
                    if(_db.RoleNavigation.Any(a => a.NavigationId == navigation.NavigationId 
                                                   && a.RoleId == adminUser.RoleId))
                        continue;

                    await _mediator.Send(command);
                }
            }
        }

        private async Task MapRouteToAdmin()
        {
            var adminUsers = GetUsers();
            var routes = _db.Route.ToList();

            foreach (var adminUser in adminUsers)
            {
                foreach (var route in routes)
                {
                    var command = new AddRoleRouteCommand()
                    {
                        RouteId = route.RouteId,
                        RoleId = adminUser.RoleId
                    };
                    
                    if(_db.RoleRoute.Any(a => a.RouteId == route.RouteId 
                                                   && a.RoleId == adminUser.RoleId))
                        continue;

                    await _mediator.Send(command);
                }
            }
        }
        
        private async Task RegisterAdminToRole()
        {
            var adminId = _db.User.FirstOrDefault(a => a.UserName.ToLower() == ADMIN.ToLower())?.Id;
            var roleId = _db.Role.FirstOrDefault(f => f.RoleCode == 0)?.Id;
            
            var userRole = new UserRole()
            {
                UserId = adminId,
                RoleId = roleId
            };
            
            if(_db.UserRole.Any(a => a.RoleId == roleId && a.UserId == adminId))
                return;
            
            _db.UserRole.Add(userRole);

            await _db.SaveChangesAsync(new CancellationToken());
        }
    }
}