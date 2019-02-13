using Microsoft.Bot.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HackedBrain.BotBuilder.Integration.AspNet.Core
{
    public class BotStateConfigurationBuilder
    {
        private IServiceCollection _services;

        internal BotStateConfigurationBuilder(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get => _services; }

        public BotStateConfigurationBuilder UseBotState(BotState botState)
        {
            _services.AddSingleton<BotState>(botState);
            _services.AddSingleton(botState.GetType(), botState);

            return this;
        }
    }
}
