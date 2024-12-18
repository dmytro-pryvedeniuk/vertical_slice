﻿using System.Reflection;

namespace TravelInspiration.API.Shared.Slices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSlices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var slices = assembly.GetTypes().Where(
            t => typeof(ISlice).IsAssignableFrom(t)
                && t != typeof(ISlice)
                && t.IsPublic
                && !t.IsAbstract);
        foreach (var slice in slices)
            services.AddSingleton(typeof(ISlice), slice);
        return services;
    }
}