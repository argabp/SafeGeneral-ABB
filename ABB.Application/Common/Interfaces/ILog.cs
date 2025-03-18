using System;
using System.Reflection;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Application.Common.Interfaces
{
    public interface ILog
    {
        void Error(Exception ex, MethodBase mb);
        void Information(string message, MethodBase mb);
        void Information(string message);
        void Warning(string message, MethodBase mb);
        void Warning(string message);
        void IdentityInformation(IdentityResult iResult, AppUser request, MethodBase mb);
    }
}