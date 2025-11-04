using AutoMapper;
using FluentAssertions;
using Moq;
using mindtechNewsletter.Server.DTOs;
using mindtechNewsletter.Server.Models;
using mindtechNewsletter.Server.Repositories;
using mindtechNewsletter.Server.Services;

namespace mindtechNewsletter.Tests.Services
{
    public class SubscriberServiceTests
    {
        private readonly Mock<ISubscriberRepository> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly SubscriberService _service;

        public SubscriberServiceTests()
        {
            _repositoryMock = new Mock<ISubscriberRepository>();

            //configuracao do automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubscriberCreateDTO, Subscriber>();
                cfg.CreateMap<Subscriber, SubscriberReadDTO>();
            });
            _mapper = config.CreateMapper();

            _service = new SubscriberService(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task SubscribeAsync_ShouldReturnFail_WhenEmailAlreadyActive()
        {
            //arrange
            var existing = new Subscriber { Id = 1, Email = "test@example.com", IsActive = true };
            _repositoryMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(existing);

            var dto = new SubscriberCreateDTO { Email = "test@example.com" };

            //act
            var result = await _service.SubscribeAsync(dto);

            //assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("O email já está cadastrado.");
        }

        [Fact]
        public async Task SubscribeAsync_ShouldReactivate_WhenEmailExistsInactive()
        {
            //arrange
            var existing = new Subscriber { Id = 1, Email = "test@example.com", IsActive = false };
            _repositoryMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(existing);

            var dto = new SubscriberCreateDTO { Email = "test@example.com" };

            //act
            var result = await _service.SubscribeAsync(dto);

            //assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Inscrição reativada com sucesso.");
            existing.IsActive.Should().BeTrue();
            _repositoryMock.Verify(r => r.Update(existing), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SubscribeAsync_ShouldAddNewSubscriber_WhenEmailDoesNotExist()
        {
            //arrange
            _repositoryMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((Subscriber)null);

            var dto = new SubscriberCreateDTO { Email = "new@example.com" };

            //act
            var result = await _service.SubscribeAsync(dto);

            //assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Inscrição realizada com sucesso.");
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Subscriber>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UnsubscribeAsync_ShouldFail_WhenEmailNotFound()
        {
            //arrange
            _repositoryMock.Setup(r => r.GetByEmailAsync("notfound@example.com")).ReturnsAsync((Subscriber)null);

            var dto = new SubscriberCreateDTO { Email = "notfound@example.com" };

            //act
            var result = await _service.UnsubscribeAsync(dto);

            //assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email não encontrado ou já descadastrado.");
        }

        [Fact]
        public async Task UnsubscribeAsync_ShouldDeactivate_WhenEmailExists()
        {
            //arrange
            var existing = new Subscriber { Id = 1, Email = "test@example.com", IsActive = true };
            _repositoryMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(existing);

            var dto = new SubscriberCreateDTO { Email = "test@example.com" };

            //act
            var result = await _service.UnsubscribeAsync(dto);

            //assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Descadastro realizado com sucesso.");
            existing.IsActive.Should().BeFalse();
            _repositoryMock.Verify(r => r.Update(existing), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSubscribers()
        {
            //arrange
            var subscribers = new List<Subscriber>
            {
                new Subscriber { Id = 1, Email = "a@example.com", IsActive = true },
                new Subscriber { Id = 2, Email = "b@example.com", IsActive = false }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(subscribers);

            //act
            var result = await _service.GetAllAsync();

            //assert
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(2);
        }
    }
}
