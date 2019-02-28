using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using HackedBrain.BotBuilder.Integration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HackedBrain.BotBuilder.State.Integration.Tests
{
    public class BotStateConfiguraitonBuilderExtensionsTests
    {
        public class UseConversationStateTests
        {
            [Fact]
            public void NullBotStateConfigurationBuilderShouldThrow() =>
                new Action(() => default(BotStateConfigurationBuilder).UseConversationState(b => { })).Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("builder");

            [Fact]
            public void NullConfigurationCallbackShouldThrow() =>
               new Action(() => new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>()).UseConversationState(null)).Should().Throw<ArgumentNullException>()
                   .And.ParamName.Should().Be("configure");
        }

        public class UseUserStateTests
        {
            [Fact]
            public void NullBotStateConfigurationBuilderShouldThrow() =>
                new Action(() => default(BotStateConfigurationBuilder).UseUserState(b => { })).Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("builder");

            [Fact]
            public void NullConfigurationCallbackShouldThrow() =>
               new Action(() => new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>()).UseUserState(null)).Should().Throw<ArgumentNullException>()
                   .And.ParamName.Should().Be("configure");
        }
    }
}
