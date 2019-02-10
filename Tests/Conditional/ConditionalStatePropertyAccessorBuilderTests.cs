using System;
using FluentAssertions;
using Microsoft.Bot.Builder;
using Moq;
using Xunit;

namespace HackedBrain.BotBuilder.State.Conditional.Tests
{
    public class ConditionalStatePropertyAccessorBuilderTests
    {
        public class Build
        {
            [Fact]
            public void ThrowsWhenNoAccessorsConfigured()
            {
                var action = new Action(() =>
                {
                    new ConditionalStatePropertyAccessorBuilder<object>().Build();
                });

                action.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void ThrowsWhenConfiguringWithNullCondition()
            {
                var action = new Action(() =>
                {
                    new ConditionalStatePropertyAccessorBuilder<object>()
                        .When(null, Mock.Of<IStatePropertyAccessor<object>>());
                });

                action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("condition");
            }

            [Fact]
            public void ThrowsWhenConfiguringWithNullStatePropertyAccessor()
            {
                var action = new Action(() =>
                {
                    new ConditionalStatePropertyAccessorBuilder<object>()
                        .When(tc => true, null);
                });

                action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("conditionalStatePropertyAccessor");
            }

            [Fact]
            public void DefaultAccessorOnly()
            {
                new ConditionalStatePropertyAccessorBuilder<object>()
                    .Default(Mock.Of<IStatePropertyAccessor<object>>())
                    .Build();
            }

            [Fact]
            public void SingleConditionalAccessor_NoDefaultAccessor()
            {
                new ConditionalStatePropertyAccessorBuilder<object>()
                    .When(tc => true, Mock.Of<IStatePropertyAccessor<object>>())
                    .Build();
            }

            [Fact]
            public void MultipleConditionalAccessors_NoDefaultAccessor()
            {
                new ConditionalStatePropertyAccessorBuilder<object>()
                    .When(tc => true, Mock.Of<IStatePropertyAccessor<object>>())
                    .When(tc => false, Mock.Of<IStatePropertyAccessor<object>>())
                    .Build();
            }

            [Fact]
            public void DefaultAccessor_WithMultipleConditionalAccessors()
            {
                var conversationStateA = new ConversationState(Mock.Of<IStorage>());
                var conversationStateB = new ConversationState(Mock.Of<IStorage>());

                var propertyA = conversationStateA.CreateProperty<string>("PropertyA");

                var propertyB = conversationStateB.CreateProperty<string>("PropertyB");
                var propertyBCondition = new Func<ITurnContext, bool>(tc => tc.Activity.ChannelId == "msteams");

                var propertyC = conversationStateB.CreateProperty<string>("PropertyC");
                var propertyCCondition = new Func<ITurnContext, bool>(tc => tc.Activity.ChannelId == "skype");

                var propertyAccessor = new ConditionalStatePropertyAccessorBuilder<string>()
                    .Default(propertyA)
                    .When(propertyBCondition, propertyB)
                    .When(propertyCCondition, propertyC)
                    .Build();

                var selectedStatePropertyAccessor = propertyAccessor.Should().BeOfType<ConditionalStatePropertyAccessor<string>>().Subject;

                selectedStatePropertyAccessor.DefaultAccessor.Should().Be(propertyA);
                selectedStatePropertyAccessor.ConditionalAccessors.Should().BeEquivalentTo((propertyBCondition, propertyB), (propertyCCondition, propertyC));
            }
        }
    }
}
