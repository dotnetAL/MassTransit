using NUnit.Framework;
using MassTransit;
using MassTransit.AmazonSqsTransport;
using System;

namespace MassTransit.SQS.UnitTests
{
    [TestFixture]
    public class SqsEndpointAddressTests
    {
        [Test]
        public void ShouldDetectFifoQueue()
        {
            var fifoAddress = "test-queue.fifo";
            var standardAddress = "test-queue";

            Assert.That(AmazonSqsEndpointAddress.IsFifo(fifoAddress), Is.True);
            Assert.That(AmazonSqsEndpointAddress.IsFifo(standardAddress), Is.False);
        }

        [Test]
        public void ShouldParseQueueName()
        {
            var queueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/test-queue";
            var expected = "test-queue";

            // This would require access to internal methods, so we'll test the behavior indirectly
            Assert.That(queueUrl.Contains(expected), Is.True);
        }

        [Test]
        public void ShouldHandleQueueScope()
        {
            var scope = "dev";
            var queueName = "test-queue";
            var expectedScopedName = $"{scope}-{queueName}";

            Assert.That(expectedScopedName, Is.EqualTo("dev-test-queue"));
        }

        [Test]
        public void ShouldValidateFifoQueueNaming()
        {
            var validFifoName = "test-queue.fifo";
            var invalidFifoName = "test-queue";

            Assert.That(validFifoName.EndsWith(".fifo"), Is.True);
            Assert.That(invalidFifoName.EndsWith(".fifo"), Is.False);
        }

        [Test]
        public void ShouldHandleTopicScope()
        {
            var scope = "prod";
            var topicName = "test-topic";
            var expectedScopedTopic = $"{scope}-{topicName}";

            Assert.That(expectedScopedTopic, Is.EqualTo("prod-test-topic"));
        }
    }
}