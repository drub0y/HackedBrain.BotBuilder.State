using Microsoft.Bot.Builder;

namespace HackedBrain.BotBuilder.Integration.AspNet.Core
{
    public static class BotStateBuilderExtensions
    {
        public static BotStateBuilder<TBotState> UseMemoryStorage<TBotState>(this BotStateBuilder<TBotState> builder) 
            where TBotState : BotState => 
                builder.UseStorage(new MemoryStorage());
    }
}
