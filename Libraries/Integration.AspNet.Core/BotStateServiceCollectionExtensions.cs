using Microsoft.Extensions.DependencyInjection;
using System;

namespace HackedBrain.BotBuilder.Integration.AspNet.Core
{
    public static class BotStateServiceCollectionExtensions
    {
        public static IServiceCollection AddBotState(this IServiceCollection services, Action<BotStateConfigurationBuilder> config)
        {
            config(new BotStateConfigurationBuilder(services));

            return services;
        }
    }
}
