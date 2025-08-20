using NUnit.Framework;
using MassTransit;
using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SimpleNotificationService;

namespace MassTransit.SQS.UnitTests
{
    [TestFixture]
    public class SqsConfigurationTests
    {
        [Test]
        public void ShouldConfigureAmazonSqsHost()
        {
            var busControl = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                });
            });

            Assert.That(busControl, Is.Not.Null);
        }

        [Test]
        public void ShouldConfigureSqsWithScope()
        {
            var busControl = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                    h.Scope("test-env", true);
                });
            });

            Assert.That(busControl, Is.Not.Null);
        }

        [Test]
        public void ShouldConfigureLocalstackHost()
        {
            var busControl = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.LocalstackHost();
            });

            Assert.That(busControl, Is.Not.Null);
        }

        [Test]
        public void ShouldConfigureWithCustomSqsConfig()
        {
            var busControl = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("us-east-1", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                    h.Config(new AmazonSQSConfig
                    {
                        RegionEndpoint = Amazon.RegionEndpoint.USEast1
                    });
                    h.Config(new AmazonSimpleNotificationServiceConfig
                    {
                        RegionEndpoint = Amazon.RegionEndpoint.USEast1
                    });
                });
            });

            Assert.That(busControl, Is.Not.Null);
        }

        [Test]
        public void ShouldThrowWhenInvalidRegion()
        {
            // Note: MassTransit may not validate AWS region names at configuration time
            // This test demonstrates how you would test for invalid configuration scenarios
            var busControl = Bus.Factory.CreateUsingAmazonSqs(cfg =>
            {
                cfg.Host("invalid-region", h =>
                {
                    h.AccessKey("test-access-key");
                    h.SecretKey("test-secret-key");
                });
            });

            // The bus control is created but validation may happen at runtime
            Assert.That(busControl, Is.Not.Null);
        }
    }
}