using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HackedBrain.BotBuilder.Integration.Tests
{
    public class BotStateServiceCollectionExtensionsTests
    {
        public class AddBotStateTests
        {
            [Fact]
            public void NullServiceCollectionShouldThrow()
            {
                var serviceCollection = default(IServiceCollection);

                var action = new Action(() => serviceCollection.AddBotState(b => { }));

                action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("services");
            }

            [Fact]
            public void NullConfigurationCallbackShouldThrow()
            {
                var action = new Action(() => new ServiceCollection().AddBotState(null));

                action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("configure");
            }
        }
    }
}
