﻿using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Backend.Fx.AspNetCore.Util;

[PublicAPI]
[Obsolete("Use configuration.GetSection(nameof(MyOptions)).Bind(myOptionsInstance)")]
public static class ConfigurationEx
{
    public static TOptions Load<TOptions>(this IConfiguration configuration) where TOptions : class, new()
    {
        IConfigurationSection configurationSection = configuration.GetSection(typeof(TOptions).Name);
        var configurationOptions = new NamedConfigureFromConfigurationOptions<TOptions>(
            typeof(TOptions).Name,
            configurationSection);
        var options = new TOptions();
        configurationOptions.Action(options);
        return options;
    }
}