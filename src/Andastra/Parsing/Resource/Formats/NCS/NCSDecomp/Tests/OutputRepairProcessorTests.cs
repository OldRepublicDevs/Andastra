// Tests for OutputRepairProcessor functionality
// Ensures repair operations work correctly and maintain engine parity

using System;
using Xunit;
using FluentAssertions;

namespace Andastra.Parsing.Formats.NCS.NCSDecomp.Tests
{
    public class OutputRepairProcessorTests
    {
        [Fact]
        public void RepairOutput_WithNullOrEmptyInput_ReturnsInput()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();

            // Act & Assert
            OutputRepairProcessor.RepairOutput(null, config).Should().BeNull();
            OutputRepairProcessor.RepairOutput("", config).Should().Be("");
            OutputRepairProcessor.RepairOutput("   ", config).Should().Be("   ");
        }

        [Fact]
        public void RepairOutput_WithSyntaxRepair_AddsMissingSemicolons()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateMinimalConfig();
            string malformedCode = @"
void main()
{
    int x = 5
    float y = 3.14
    string s = ""hello""
}";

            string expectedCode = @"
void main()
{
    int x = 5;
    float y = 3.14;
    string s = ""hello"";
}";

            // Act
            string result = OutputRepairProcessor.RepairOutput(malformedCode, config);

            // Assert
            result.Should().Be(expectedCode);
            config.RepairsApplied.Should().BeTrue();
        }

        [Fact]
        public void RepairOutput_WithTypeRepair_RemovesInvalidCasts()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();
            config.EnableTypeRepair = true;
            config.EnableSyntaxRepair = false;

            string malformedCode = @"
void main()
{
    (invalidtype)someVar;
    (int)validVar;
}";

            string expectedCode = @"
void main()
{
    someVar;
    (int)validVar;
}";

            // Act
            string result = OutputRepairProcessor.RepairOutput(malformedCode, config);

            // Assert
            result.Should().Be(expectedCode);
            config.RepairsApplied.Should().BeTrue();
        }

        [Fact]
        public void RepairOutput_WithExpressionRepair_FixesOperatorPrecedence()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();
            config.EnableExpressionRepair = true;
            config.EnableSyntaxRepair = false;
            config.EnableTypeRepair = false;
            config.EnableControlFlowRepair = false;

            string malformedCode = "int result = a + b * c + d;";
            string expectedCode = "int result = (a + b) * (c + d);";

            // Act
            string result = OutputRepairProcessor.RepairOutput(malformedCode, config);

            // Assert
            result.Should().Be(expectedCode);
            config.RepairsApplied.Should().BeTrue();
        }

        [Fact]
        public void RepairOutput_WithControlFlowRepair_FixesIfStatements()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();
            config.EnableControlFlowRepair = true;
            config.EnableSyntaxRepair = false;

            string malformedCode = @"
void main()
{
    if condition {
        DoSomething();
    }
    while otherCondition {
        DoSomethingElse();
    }
}";

            string expectedCode = @"
void main()
{
    if (condition) {
        DoSomething();
    }
    while (otherCondition) {
        DoSomethingElse();
    }
}";

            // Act
            string result = OutputRepairProcessor.RepairOutput(malformedCode, config);

            // Assert
            result.Should().Be(expectedCode);
            config.RepairsApplied.Should().BeTrue();
        }

        [Fact]
        public void RepairOutput_WithUnmatchedBraces_AddsMissingClosingBraces()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();
            config.EnableSyntaxRepair = true;

            string malformedCode = @"
void main()
{
    if (condition) {
        DoSomething();
    // Missing closing brace here
";

            // Act
            string result = OutputRepairProcessor.RepairOutput(malformedCode, config);

            // Assert
            result.Should().Contain("}"); // Should have added closing brace
            config.RepairsApplied.Should().BeTrue();
        }

        [Fact]
        public void RepairOutput_WithVerboseLogging_RecordsAppliedRepairs()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();
            config.VerboseLogging = true;

            string malformedCode = @"
void main()
{
    int x = 5
}";

            // Act
            string result = OutputRepairProcessor.RepairOutput(malformedCode, config);

            // Assert
            config.RepairsApplied.Should().BeTrue();
            config.AppliedRepairs.Should().NotBeEmpty();
            config.AppliedRepairs.Should().Contain(r => r.Contains("semicolon"));
        }

        [Fact]
        public void CreateDefaultConfig_ReturnsExpectedDefaults()
        {
            // Act
            var config = OutputRepairProcessor.CreateDefaultConfig();

            // Assert
            config.EnableSyntaxRepair.Should().BeTrue();
            config.EnableTypeRepair.Should().BeTrue();
            config.EnableExpressionRepair.Should().BeTrue();
            config.EnableControlFlowRepair.Should().BeTrue();
            config.EnableFunctionSignatureRepair.Should().BeTrue();
            config.MaxRepairPasses.Should().Be(3);
            config.VerboseLogging.Should().BeFalse();
        }

        [Fact]
        public void CreateMinimalConfig_EnablesOnlySyntaxRepair()
        {
            // Act
            var config = OutputRepairProcessor.CreateMinimalConfig();

            // Assert
            config.EnableSyntaxRepair.Should().BeTrue();
            config.EnableTypeRepair.Should().BeFalse();
            config.EnableExpressionRepair.Should().BeFalse();
            config.EnableControlFlowRepair.Should().BeFalse();
            config.EnableFunctionSignatureRepair.Should().BeFalse();
        }

        [Fact]
        public void CreateComprehensiveConfig_EnablesAllRepairsWithVerboseLogging()
        {
            // Act
            var config = OutputRepairProcessor.CreateComprehensiveConfig();

            // Assert
            config.EnableSyntaxRepair.Should().BeTrue();
            config.EnableTypeRepair.Should().BeTrue();
            config.EnableExpressionRepair.Should().BeTrue();
            config.EnableControlFlowRepair.Should().BeTrue();
            config.EnableFunctionSignatureRepair.Should().BeTrue();
            config.VerboseLogging.Should().BeTrue();
        }

        [Fact]
        public void RepairOutput_WithMaxRepairPasses_LimitsIterations()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();
            config.MaxRepairPasses = 1;
            config.VerboseLogging = true;

            // Code that would benefit from multiple passes
            string complexMalformedCode = @"
void main()
{
    if condition {
        int x = 5
        (invalidtype)someVar
    }
}";

            // Act
            string result = OutputRepairProcessor.RepairOutput(complexMalformedCode, config);

            // Assert
            config.AppliedRepairs.Count.Should().BeLessThanOrEqualTo(config.MaxRepairPasses);
        }

        [Fact]
        public void RepairOutput_WithValidCode_ReturnsUnchanged()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();

            string validCode = @"
void main()
{
    int x = 5;
    float y = 3.14;
    if (x > 0) {
        DoSomething();
    }
}";

            // Act
            string result = OutputRepairProcessor.RepairOutput(validCode, config);

            // Assert
            result.Should().Be(validCode);
            config.RepairsApplied.Should().BeFalse();
        }

        [Fact]
        public void RepairOutput_WithReturnStatement_FixesMalformedReturns()
        {
            // Arrange
            var config = OutputRepairProcessor.CreateDefaultConfig();
            config.EnableControlFlowRepair = true;

            string malformedCode = @"
int GetValue()
{
    return 42
}";

            string expectedCode = @"
int GetValue()
{
    return 42;
}";

            // Act
            string result = OutputRepairProcessor.RepairOutput(malformedCode, config);

            // Assert
            result.Should().Be(expectedCode);
            config.RepairsApplied.Should().BeTrue();
        }
    }
}
