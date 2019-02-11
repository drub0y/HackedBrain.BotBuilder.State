using System;
using FluentAssertions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Moq;
using Xunit;

namespace HackedBrain.BotBuilder.State.Conditional.Tests
{
    public class ConsiditionalStatePropertyAccessorTests
    {
        public class SelectAccessorForTurn
        {
            [Fact]
            public void SelectsCorrectConditionalAccessorForTurn()
            {
                var conversationState = new ConversationState(Mock.Of<IStorage>());

                var teamsPropertyAccessor = conversationState.CreateProperty<string>("PropertyTeams");
                var teamsPropertyCondition = new Func<ITurnContext, bool>(tc => tc.Activity.ChannelId == "msteams");

                var skypePropertyAccessor = conversationState.CreateProperty<string>("PropertySkype");
                var skypePropertyCondition = new Func<ITurnContext, bool>(tc => tc.Activity.ChannelId == "skype");

                var propertyAccessor = new ConditionalStatePropertyAccessorBuilder<string>()
                    .When(teamsPropertyCondition, teamsPropertyAccessor)
                    .When(skypePropertyCondition, skypePropertyAccessor)
                    .Build();

                var conditionalStatePropertyAccessor = propertyAccessor.Should().BeOfType<ConditionalStatePropertyAccessor<string>>().Subject;

                var testActivity = new Activity
                {
                    ChannelId = "skype"
                };

                var mockTurnContext = new Mock<ITurnContext>();
                mockTurnContext.Setup(tc => tc.Activity).Returns(testActivity);

                var selectedPropertyAccessor = conditionalStatePropertyAccessor.SelectAccessorForTurn(mockTurnContext.Object);

                selectedPropertyAccessor.Should().Be(skypePropertyAccessor);
            }

            [Fact]
            public void SelectsDefaultAccessorWhenNoConditionalAccessorsMatch()
            {
                var conversationState = new ConversationState(Mock.Of<IStorage>());

                var defaultPropertyAccessor = conversationState.CreateProperty<string>("Default");

                var propertyAccessor = new ConditionalStatePropertyAccessorBuilder<string>()
                    .Default(defaultPropertyAccessor)
                    .When(tc => false, conversationState.CreateProperty<string>("PropertyA"))
                    .When(tc => false, conversationState.CreateProperty<string>("PropertyB"))
                    .Build();

                var conditionalStatePropertyAccessor = propertyAccessor.Should().BeOfType<ConditionalStatePropertyAccessor<string>>().Subject;

                var selectedPropertyAccessor = conditionalStatePropertyAccessor.SelectAccessorForTurn(Mock.Of<ITurnContext>());

                selectedPropertyAccessor.Should().Be(defaultPropertyAccessor);
            }

            [Fact]
            public void ThrowsWhenNoConditionalAccessorsMatchAndNoDefaultAccessorIsConfigured()
            {
                var conversationState = new ConversationState(Mock.Of<IStorage>());

                var propertyAccessor = new ConditionalStatePropertyAccessorBuilder<string>()
                    .When(tc => false, conversationState.CreateProperty<string>("PropertyA"))
                    .When(tc => false, conversationState.CreateProperty<string>("PropertyB"))
                    .Build();

                var conditionalStatePropertyAccessor = propertyAccessor.Should().BeOfType<ConditionalStatePropertyAccessor<string>>().Subject;

                var action = new Action(() => conditionalStatePropertyAccessor.SelectAccessorForTurn(Mock.Of<ITurnContext>()));

                action.Should().Throw<NoStatePropertyAccessorCouldBeSelectedException>();
            }
        }
    }
}
