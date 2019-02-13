using Microsoft.Bot.Builder;
using System;

namespace HackedBrain.BotBuilder.Integration.AspNet.Core
{
    public static class BotStateConfigurationBuilderExtensions
    {
        public static BotStateConfigurationBuilder UseUserState<TBotState>(this BotStateConfigurationBuilder builder, Action<BotStateBuilder<TBotState>> config)
            where TBotState : BotState =>
                UseBotState(builder, config, typeof(UserState));

        public static BotStateConfigurationBuilder UseConversationState<TBotState>(this BotStateConfigurationBuilder builder, Action<BotStateBuilder<TBotState>> config)
            where TBotState : BotState =>
                UseBotState(builder, config, typeof(ConversationState));

        private static BotStateConfigurationBuilder UseBotState<TBotState>(BotStateConfigurationBuilder builder, Action<BotStateBuilder<TBotState>> config, Type botStateType)
            where TBotState : BotState
        {
            var stateBuilder = new BotStateBuilder<TBotState>(builder);

            config(stateBuilder);

            var botState = stateBuilder.Build();

            return builder.UseBotState(botState);
        }
    }
}
