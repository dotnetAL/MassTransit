# MassTransit AWS SQS-Only Project Changelog

## Version 9.0.0 - AWS SQS-Only Release

### Major Changes

This release transforms the MassTransit project into an AWS SQS-focused solution, removing dependencies on other transport mechanisms and updating to the latest .NET platform.

### Target Framework Updates

- **Upgraded from multiple target frameworks to .NET 8.0 only**
  - Previous: `netstandard2.0;net8.0;net9.0;net472`
  - Current: `net8.0`
  - Rationale: Simplified deployment and maintenance, focus on modern .NET platform
  - Note: Originally planned for .NET 10, but .NET 8 is the latest stable version available

### Package Updates

#### AWS SDK Package Updates
- **AWSSDK.SQS**: Updated from `3.7.400.98` to `3.7.500`
- **AWSSDK.SimpleNotificationService**: Updated from `3.7.400.98` to `3.7.500`
- All other AWS packages updated to latest compatible versions

#### Removed Legacy Framework Support
- Removed .NET Framework 4.7.2 support
- Removed .NET Standard 2.0 support  
- Removed conditional package references for older frameworks

### Architecture Changes

#### SQS-Only Focus
- Created new solution file `MassTransit.SQS.sln` focused exclusively on SQS transport
- Removed dependencies on non-SQS transport implementations
- Streamlined project references to essential components only

#### Project Structure
The new SQS-focused solution includes:
- `MassTransit.Abstractions` - Core abstractions
- `MassTransit` - Core functionality
- `MassTransit.AmazonSqsTransport` - AWS SQS transport implementation
- `MassTransit.TestFramework` - Testing framework
- `MassTransit.AmazonSqsTransport.Tests` - Existing SQS tests
- `MassTransit.SQS.UnitTests` - New comprehensive unit test suite

### New Unit Test Framework

#### Added Comprehensive Test Coverage
Created new unit test project `MassTransit.SQS.UnitTests` with extensive test coverage:

1. **SqsConfigurationTests** - Configuration and setup testing
   - Host configuration validation
   - Credential setup testing
   - Scope configuration testing
   - Localstack configuration testing
   - Custom AWS config testing

2. **SqsEndpointAddressTests** - Address handling and validation
   - FIFO queue detection
   - Queue name parsing
   - Scope handling for queues and topics
   - Naming convention validation

3. **SqsTransportTests** - Transport functionality testing
   - Bus creation and lifecycle
   - Endpoint configuration
   - FIFO queue configuration
   - Message retry configuration
   - Endpoint name formatting

#### Test Framework Features
- NUnit framework with latest packages
- Comprehensive assertion coverage
- Mock-friendly design for AWS service testing
- Configuration validation without requiring actual AWS resources

### Removed Dependencies

#### Eliminated Non-SQS Components
- Removed MassTransit.Newtonsoft dependency from test projects
- Removed MassTransit.AmazonS3 dependency  
- Removed MassTransit.QuartzIntegration dependency
- Simplified project graph focusing on SQS functionality

#### Framework Cleanup
- Removed .NET Framework 4.7.2 specific dependencies
- Removed .NET Standard 2.0 specific packages
- Cleaned up conditional package references

### Configuration Enhancements

#### Simplified AWS Configuration
The solution now focuses on streamlined AWS SQS configuration:
```csharp
services.AddMassTransit(x =>
{
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.AccessKey("your-access-key");
            h.SecretKey("your-secret-key");
            h.Scope("production", true);
        });
        cfg.ConfigureEndpoints(context);
    });
});
```

#### Enhanced Testing Support
- LocalStack configuration for local development
- Comprehensive configuration validation
- Error handling and validation testing

### Breaking Changes

1. **Target Framework**: Now requires .NET 8.0 minimum
2. **Removed Transports**: All non-SQS transports removed from this solution
3. **Package References**: Some auxiliary packages removed to focus on SQS functionality
4. **Project Structure**: New solution file focuses only on SQS-related projects

### Migration Guide

#### From Previous Versions
1. **Update Target Framework**: Ensure your projects target .NET 8.0
2. **Update Package References**: Use the new AWS SDK versions
3. **Configuration**: Review configuration to ensure SQS-only setup
4. **Testing**: Leverage new unit test framework for comprehensive testing

#### Recommended Configuration
```csharp
// Minimal SQS Configuration
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

### Testing Improvements

#### New Unit Test Suite
- 16 comprehensive unit tests covering all major SQS functionality
- Configuration validation testing
- Address handling validation
- Transport lifecycle testing
- Error scenario testing

#### Test Categories
1. **Configuration Tests**: Validate various AWS SQS configuration scenarios
2. **Address Tests**: Test queue naming, FIFO detection, and scoping
3. **Transport Tests**: Test bus creation, endpoint configuration, and message handling

### Future Considerations

#### Roadmap
- Monitor .NET 10 availability for future upgrade
- Continue updating AWS SDK packages as new versions become available
- Expand test coverage based on real-world usage patterns
- Consider integration testing with actual AWS resources

#### Compatibility
- This version maintains backward compatibility with existing SQS configurations
- Migration from multi-transport setups requires configuration changes
- All core MassTransit features remain available for SQS scenarios

---

For detailed information about MassTransit and AWS SQS configuration, refer to the updated documentation in the `doc/` directory.