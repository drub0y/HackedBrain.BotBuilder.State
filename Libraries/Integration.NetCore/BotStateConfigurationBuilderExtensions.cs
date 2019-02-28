using Microsoft.Bot.Builder;
using System;

namespace HackedBrain.BotBuilder.Integration
{
    public static class BotStateConfigurationBuilderExtensions
    {
        public static BotStateConfigurationBuilder UseUserState(this BotStateConfigurationBuilder builder, Action<BotStateBuilder<UserState>> configure) =>
                UseBotState<UserState>(builder, configure);

        public static BotStateConfigurationBuilder UseConversationState(this BotStateConfigurationBuilder builder, Action<BotStateBuilder<ConversationState>> configure) =>
                UseBotState<ConversationState>(builder, configure);

        private static BotStateConfigurationBuilder UseBotState<TBotState>(BotStateConfigurationBuilder builder, Action<BotStateBuilder<TBotState>> configure)
            where TBotState : BotState
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var stateBuilder = new BotStateBuilder<TBotState>(builder);

            configure(stateBuilder);

            var botState = stateBuilder.Build();

            return builder.UseBotState(botState);
        }
    }
}
