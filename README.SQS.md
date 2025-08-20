# MassTransit AWS SQS-Only Edition

[![Build Status](https://github.com/dotnetAL/MassTransit/workflows/build/badge.svg)](https://github.com/dotnetAL/MassTransit/actions)
[![NuGet](https://img.shields.io/nuget/v/MassTransit.AmazonSQS.svg)](https://www.nuget.org/packages/MassTransit.AmazonSQS/)

**MassTransit AWS SQS-Only Edition** is a streamlined version of the popular MassTransit service bus framework, focused exclusively on Amazon SQS/SNS transport. This edition provides a simplified, high-performance messaging solution for AWS-centric applications.

## What's New in This Edition

### 🎯 **SQS-Only Focus**
- Removed all non-SQS transport dependencies
- Streamlined project structure for AWS SQS/SNS only
- Optimized for cloud-native AWS applications

### 🚀 **Latest .NET Support**
- Updated to .NET 8.0 (latest stable)
- Removed legacy .NET Framework support
- Modern C# language features and performance optimizations

### 📦 **Updated AWS SDK**
- Latest AWS SDK packages (AWSSDK.SQS 3.7.500, AWSSDK.SimpleNotificationService 3.7.500)
- Enhanced AWS integration and security features
- Improved performance and reliability

### 🧪 **Comprehensive Testing**
- New dedicated unit test framework
- 16+ comprehensive tests covering all SQS functionality
- Configuration validation and error handling tests
- LocalStack support for local development

## Quick Start

### Installation

```bash
dotnet add package MassTransit.AmazonSQS
```

### Basic Configuration

```csharp
using MassTransit;

services.AddMassTransit(x =>
{
    x.AddConsumer<OrderConsumer>();
    
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.AccessKey("your-iam-access-key");
            h.SecretKey("your-iam-secret-key");
        });

        cfg.ConfigureEndpoints(context);
    });
});
```

### Environment-Scoped Configuration

```csharp
services.AddMassTransit(x =>
{
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.AccessKey("your-access-key");
            h.SecretKey("your-secret-key");
            h.Scope("production", true); // Scope both queues and topics
        });

        // Use scoped endpoint naming
        cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter("prod-", false));
    });
});
```

### LocalStack Development

```csharp
services.AddMassTransit(x =>
{
    x.UsingAmazonSqs(cfg =>
    {
        cfg.LocalstackHost(); // Automatically configures for LocalStack
    });
});
```

## Features

### 🔧 **Core Messaging Features**
- **Publish/Subscribe**: Full SNS topic support with automatic SQS subscription
- **Send/Receive**: Direct SQS queue messaging
- **Request/Response**: Built-in request-response pattern support
- **Message Retry**: Configurable retry policies with dead letter queues
- **FIFO Queues**: Full support for Amazon SQS FIFO queues

### 🏗️ **Enterprise Features**
- **Saga State Machines**: Durable workflow orchestration
- **Message Scheduling**: Future message delivery
- **Message Deduplication**: FIFO queue deduplication support
- **Error Handling**: Comprehensive error queue management
- **Health Checks**: Built-in health monitoring

### ⚡ **Performance & Reliability**
- **High Throughput**: Optimized for AWS SQS performance characteristics
- **Auto Scaling**: Automatic scaling based on queue depth
- **Dead Letter Queues**: Automatic poison message handling
- **Message Ordering**: FIFO queue support for ordered processing

## Project Structure

This SQS-only edition includes:

```
MassTransit.SQS.sln
├── src/
│   ├── MassTransit.Abstractions/          # Core abstractions
│   ├── MassTransit/                       # Core messaging framework
│   ├── MassTransit.TestFramework/         # Testing utilities
│   └── Transports/
│       └── MassTransit.AmazonSqsTransport/ # AWS SQS/SNS transport
└── tests/
    ├── MassTransit.AmazonSqsTransport.Tests/ # Integration tests
    └── MassTransit.SQS.UnitTests/           # Comprehensive unit tests
```

## Configuration Examples

### Advanced SQS Configuration

```csharp
services.AddMassTransit(x =>
{
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.AccessKey("your-access-key");
            h.SecretKey("your-secret-key");
            
            // Custom SQS configuration
            h.Config(new AmazonSQSConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1,
                MaxErrorRetry = 3
            });
            
            // Custom SNS configuration
            h.Config(new AmazonSimpleNotificationServiceConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            });
        });

        // Configure specific endpoint
        cfg.ReceiveEndpoint("order-queue", e =>
        {
            e.ConfigureConsumer<OrderConsumer>(context);
            e.UseMessageRetry(r => r.Intervals(100, 200, 500));
        });
    });
});
```

### FIFO Queue Configuration

```csharp
services.AddMassTransit(x =>
{
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h => /* host config */);
        
        cfg.ReceiveEndpoint("order-queue.fifo", e =>
        {
            e.ConfigureConsumer<OrderConsumer>(context);
            // FIFO-specific configuration automatically applied
        });
    });
});
```

## Testing

### Unit Testing with the Test Framework

```csharp
[Test]
public async Task Should_consume_order_message()
{
    using var harness = new InMemoryTestHarness();
    var consumer = harness.Consumer<OrderConsumer>();

    await harness.Start();

    await harness.InputQueueSendEndpoint.Send(new OrderSubmitted
    {
        OrderId = "12345",
        Timestamp = DateTime.UtcNow
    });

    Assert.That(await harness.Consumed.Any<OrderSubmitted>());
    Assert.That(await consumer.Consumed.Any<OrderSubmitted>());
}
```

### Running Tests

```bash
# Build the solution
dotnet build MassTransit.SQS.sln

# Run all tests
dotnet test MassTransit.SQS.sln

# Run specific test project
dotnet test tests/MassTransit.SQS.UnitTests/
```

## AWS Requirements

### IAM Permissions

Your AWS IAM user/role needs the following permissions:

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "sqs:*",
                "sns:*"
            ],
            "Resource": "*"
        }
    ]
}
```

### Recommended Security

- Use IAM roles instead of access keys when running in AWS
- Implement least privilege principles
- Enable CloudTrail logging for audit trails
- Use VPC endpoints for enhanced security

## Migration from Full MassTransit

### From Previous Versions

1. **Update Target Framework**: Ensure .NET 8.0 minimum
2. **Update NuGet Packages**: Use latest MassTransit.AmazonSQS package
3. **Remove Other Transports**: Remove references to RabbitMQ, Azure Service Bus, etc.
4. **Update Configuration**: Ensure using `UsingAmazonSqs` configuration method

### Configuration Migration

```csharp
// Old multi-transport configuration
services.AddMassTransit(x =>
{
    x.UsingRabbitMq(...); // Remove
    // or
    x.UsingInMemory(...);  // Remove
});

// New SQS-only configuration
services.AddMassTransit(x =>
{
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.AccessKey("your-access-key");
            h.SecretKey("your-secret-key");
        });
        cfg.ConfigureEndpoints(context);
    });
});
```

## Documentation

- [AWS SQS Quick Start](doc/content/2.quick-starts/4.amazon-sqs.md)
- [SQS Configuration Guide](doc/content/3.documentation/2.configuration/2.transports/4.amazon-sqs.md)
- [Changelog](CHANGELOG.md)

## Support

For issues and questions:
- [GitHub Issues](https://github.com/dotnetAL/MassTransit/issues)
- [Documentation](doc/)
- [AWS SQS Documentation](https://docs.aws.amazon.com/sqs/)

## License

Licensed under the Apache License, Version 2.0. See [LICENSE](LICENSE) for details.

---

**Note**: This is a specialized, SQS-only edition of MassTransit. For multi-transport scenarios, consider the full MassTransit framework.