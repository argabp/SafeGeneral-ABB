using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ABB.Application.Common.Helpers;
using ABB.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Infrastructure.Helpers
{
    public class AssemblyHelper : IAssemblyHelper
    {
        public List<Route> GetAssemblyRoutes()
        {
            List<Route> RouteList = new List<Route>();
            var assemblyList2 = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("ABB")).ToList();
            foreach (Assembly asm in assemblyList)
            {
                var result = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type =>
                        type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m
                        .GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true)
                        .Any())
                    .GroupBy(d => new { d.DeclaringType, d.Name })
                    .Select(x => new Route()
                        { Controller = x.Key.DeclaringType.Name.Replace("Controller", ""), Action = x.Key.Name })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
                RouteList.AddRange(result);
            }

            return RouteList;
        }
    }
}