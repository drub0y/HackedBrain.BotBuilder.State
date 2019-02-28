using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using HackedBrain.BotBuilder.Integration;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HackedBrain.BotBuilder.State.Integration.Tests
{
    public class BotStateBuilderTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void NullBotStateConfigurationBuilderShouldThrow() =>
                new Action(() => new BotStateBuilder<BotState>(null))
                    .Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("botStateConfigurationBuilder");
        }

        public class UseBotStateFactoryTests
        {
            [Fact]
            public void NullStateFactoryShouldThrow() =>
                new Action(() => new BotStateBuilder<BotState>(new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>())).UseBotStateFactory(null))
                    .Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("factory");

            [Fact]
            public void BuildsUsingSuppliedStateFactory()
            {
                var stateFactoryMock = new Mock<Func<FakeBotState>>();
                stateFactoryMock.Setup(f => f()).Returns(new Mock<FakeBotState>(Mock.Of<IStorage>()).Object);

                new BotStateBuilder<FakeBotState>(new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>()))
                    .UseBotStateFactory(stateFactoryMock.Object)
                    .Build();

                stateFactoryMock.Verify(sf => sf(), Times.Once());
            }

            [Fact]
            public void ThrowsWhenStateFactoryProvidesNullInstance() =>
                new Action(() => new BotStateBuilder<FakeBotState>(new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>()))
                    .UseBotStateFactory(() => null)
                    .Build())
                    .Should().Throw<InvalidOperationException>();
        }

        public class UseStorageTests
        {
            [Fact]
            public void NullStorageShouldThrow() =>
                new Action(() => new BotStateBuilder<BotState>(new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>())).UseStorage(null))
                    .Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("storage");

            [Fact]
            public void BuildsWhenGivenAValidIStorageInstance()
            {
                var botState = new BotStateBuilder<FakeBotState>(new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>()))
                    .UseStorage(Mock.Of<IStorage>())
                    .Build();
            }
        }

        public class WithPropertyTests
        {
            [Fact]
            public void UnspecificedPropertyNameShouldUseNameOfPropertyType()
            {
                var serviceCollectionMock = new Mock<IServiceCollection>();

                new BotStateBuilder<FakeBotState>(new BotStateConfigurationBuilder(serviceCollectionMock.Object))
                    .UseBotStateFactory(() => new Mock<FakeBotState>(Mock.Of<IStorage>()).Object)
                    .WithProperty<string>()
                    .Build();

                // TODO: having to verify this way sucks, but there's no way to verify purely through the BotState that was created because
                //    1. BotState does not expose its properties collection (fine)
                //    2. BotState's CreateProperty<T> is not virtual, so can't be mocked/overridden
                //    3. There is no IBotState interface
                serviceCollectionMock.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd => sd.ServiceType == typeof(IStatePropertyAccessor<string>) && ((sd.ImplementationInstance as IStatePropertyAccessor<string>).Name == nameof(String)))));
            }

            [Fact]
            public void SpecifiedPropertyNameShouldBeUsed()
            {
                var serviceCollectionMock = new Mock<IServiceCollection>();

                new BotStateBuilder<FakeBotState>(new BotStateConfigurationBuilder(serviceCollectionMock.Object))
                    .UseBotStateFactory(() => new Mock<FakeBotState>(Mock.Of<IStorage>()).Object)
                    .WithProperty<string>(nameof(SpecifiedPropertyNameShouldBeUsed))
                    .Build();

                // TODO: having to verify this way sucks, but there's no way to verify purely through the BotState that was created because
                //    1. BotState does not expose its properties collection (fine)
                //    2. BotState's CreateProperty<T> is not virtual, so can't be mocked/overridden
                //    3. There is no IBotState interface
                serviceCollectionMock.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd => sd.ServiceType == typeof(IStatePropertyAccessor<string>) && ((sd.ImplementationInstance as IStatePropertyAccessor<string>).Name == nameof(SpecifiedPropertyNameShouldBeUsed)))));
            }

        }
    }
}
