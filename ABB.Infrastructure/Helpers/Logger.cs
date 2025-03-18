using System;
using System.Linq;
using System.Reflection;
using System.Text;
using ABB.Application.Common.Interfaces;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace ABB.Infrastructure.Helpers
{

public class Logger : ILog
{
    public void Error(Exception ex, MethodBase mb)
    {
        Log.Error(ex, $"{mb?.ReflectedType?.FullName} | {mb?.Name}");
    }

    public void IdentityInformation(IdentityResult iResult, AppUser request, MethodBase mb)
    {
        var strBuilder = new StringBuilder();
        foreach (var err in iResult.Errors?.ToList()) strBuilder.AppendLine(err.Description);
        Log.Information($"From: {mb?.ReflectedType?.FullName} | {mb?.Name}| User {request.UserName} Result:{iResult.Succeeded} | Error = {strBuilder.ToString()}");
    }

    public void Information(string message, MethodBase mb)
    {
        Log.Information($"{mb?.ReflectedType?.FullName} | {mb?.Name} => Msg: {message}");
    }

    public void Information(string message)
    {
        Log.Information($"{message}");
    }

    public void Warning(string message, MethodBase mb)
    {
        Log.Warning($"{mb?.ReflectedType?.FullName} | {mb?.Name} => Msg: {message}");
    }

    public void Warning(string message)
    {
        Log.Warning($"{message}");
    }
}
}