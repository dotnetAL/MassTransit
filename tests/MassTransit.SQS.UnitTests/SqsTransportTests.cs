using NUnit.Framework;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace MassTransit.SQS.UnitTests
{
    [TestFixture]
    public class SqsTransportTests
    {
        [Test]
        public void ShouldCreateBusUsingAmazonSqs()
        {
            var bus = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                });
            });

            Assert.That(bus, Is.Not.Null);
            Assert.That(bus, Is.InstanceOf<IBusControl>());
        }

        [Test]
        public async Task ShouldCreateBusAndStop()
        {
            var bus = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                });
            });

            // Test that we can start and stop the bus without errors
            await bus.StartAsync();
            await bus.StopAsync();

            Assert.That(bus, Is.Not.Null);
        }

        [Test]
        public void ShouldConfigureReceiveEndpoint()
        {
            var bus = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                });

                cfg.ReceiveEndpoint("test-queue", e =>
                {
                    e.Consumer<TestConsumer>();
                });
            });

            Assert.That(bus, Is.Not.Null);
        }

        [Test]
        public void ShouldConfigureFifoQueue()
        {
            var bus = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                });

                cfg.ReceiveEndpoint("test-queue.fifo", e =>
                {
                    e.Consumer<TestConsumer>();
                    // FIFO specific configuration would go here
                });
            });

            Assert.That(bus, Is.Not.Null);
        }

        [Test]
        public void ShouldHandleEndpointFormatter()
        {
            var formatter = new DefaultEndpointNameFormatter("test-", false);
            var queueName = formatter.Consumer<TestConsumer>();

            Assert.That(queueName, Does.StartWith("test-"));
            Assert.That(queueName, Does.Contain("Test"));
        }

        [Test]
        public void ShouldConfigureMessageRetry()
        {
            var bus = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                });

                cfg.ReceiveEndpoint("test-queue", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
                    e.Consumer<TestConsumer>();
                });
            });

            Assert.That(bus, Is.Not.Null);
        }
    }

    // Test consumer for configuration testing
    public class TestConsumer : IConsumer<TestMessage>
    {
        public Task Consume(ConsumeContext<TestMessage> context)
        {
            return Task.CompletedTask;
        }
    }

    // Test message for configuration testing
    public class TestMessage
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}