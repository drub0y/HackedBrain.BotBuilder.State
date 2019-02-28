using Microsoft.Bot.Builder;

namespace HackedBrain.BotBuilder.State.Integration.Tests
{
    internal class FakeBotState : BotState
    {
        public FakeBotState(IStorage storage) : base(storage, nameof(FakeBotState))
        {
        }

        protected override string GetStorageKey(ITurnContext turnContext) =>
            nameof(FakeBotState);
    }
}