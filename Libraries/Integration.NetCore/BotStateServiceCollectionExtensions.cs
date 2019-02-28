using Microsoft.Extensions.DependencyInjection;
using System;

namespace HackedBrain.BotBuilder.Integration
{
    public static class BotStateServiceCollectionExtensions
    {
        public static IServiceCollection AddBotState(this IServiceCollection services, Action<BotStateConfigurationBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            configure(new BotStateConfigurationBuilder(services));

            return services;
        }
    }
}
