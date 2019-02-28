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
    public class BotStateConfiguraitonBuilderTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void NullServiceCollectionShouldThrow() =>
                new Action(() => new BotStateConfigurationBuilder(null)).Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("services");
        }

        public class UseBotStateTests
        {
            [Fact]
            public void NullBotStateShouldThrow() =>
                new Action(() => new BotStateConfigurationBuilder(Mock.Of<IServiceCollection>()).UseBotState(null)).Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("botState");

            [Fact]
            public void AddsExpectedServiceDescriptorsForBotState_Single()
            {
                var serviceCollectionMock = new Mock<IServiceCollection>();

                new BotStateConfigurationBuilder(serviceCollectionMock.Object)
                    .UseBotState(new FakeBotState(Mock.Of<IStorage>()));

                serviceCollectionMock.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd => sd.ServiceType == typeof(FakeBotState))), Times.Once());
                serviceCollectionMock.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd => sd.ServiceType == typeof(BotState))), Times.Once());
            }

            [Fact]
            public void AddsExpectedServiceDescriptorsForBotState_Multiple()
            {
                var serviceCollectionMock = new Mock<IServiceCollection>();

                new BotStateConfigurationBuilder(serviceCollectionMock.Object)
                    .UseBotState(new FakeBotState(Mock.Of<IStorage>()))
                    .UseBotState(new FakeBotState(Mock.Of<IStorage>()));

                serviceCollectionMock.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd => sd.ServiceType == typeof(FakeBotState))), Times.Exactly(2));
                serviceCollectionMock.Verify(sc => sc.Add(It.Is<ServiceDescriptor>(sd => sd.ServiceType == typeof(BotState))), Times.Exactly(2));
            }
        }
    }
}
