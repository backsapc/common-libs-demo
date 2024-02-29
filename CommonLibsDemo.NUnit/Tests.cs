using FluentValidation;
using FluentValidation.TestHelper;
using KellermanSoftware.CompareNetObjects;

namespace CommonLibsDemo.NUnit;

public class ComparableObject
{
    public string Value { get; set; }
    public NestableObject NestableObject { get; set; }
}

public class NestableObject
{
    public int IntValue { get; set; }
    public string StringValue { get; set; }
}

public class ComparableObjectValidator : AbstractValidator<ComparableObject>
{
    public ComparableObjectValidator()
    {
        RuleFor(x => x.Value).NotEmpty();
        RuleFor(x => x.NestableObject).NotEmpty();
        RuleFor(x => x.NestableObject.StringValue).NotEmpty();
        RuleFor(x => x.NestableObject.IntValue).GreaterThan(100);
    }
}

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CompareObject_Should_Compare()
    {
        // arrange
        var first = new ComparableObject
        {
            Value = Guid.NewGuid().ToString(),
            NestableObject = new()
            {
                IntValue = new Random().Next(),
                StringValue = Guid.NewGuid().ToString()
            }
        };
        
        var second = new ComparableObject
        {
            Value = Guid.NewGuid().ToString(),
            NestableObject = new()
            {
                IntValue = new Random().Next(),
                StringValue = first.NestableObject.StringValue
            }
        };

        CompareLogic compareLogic = new CompareLogic();
        
        // act
        ComparisonResult result = compareLogic.Compare(first, second);

        // assert
        Assert.That(result.AreEqual, Is.False);
    }
    
    [Test]
    public void FluentValidation_Should_PassValidation()
    {
        // arrange
        var first = new ComparableObject
        {
            Value = Guid.NewGuid().ToString(),
            NestableObject = new()
            {
                IntValue = 101,
                StringValue = Guid.NewGuid().ToString()
            }
        };

        var validator = new ComparableObjectValidator();
        
        // act
        var validationResult = validator.TestValidate(first);
        
        // assert
        Assert.That(validationResult.IsValid, Is.True);
    }
    
    [Test]
    public void FluentValidation_Should_FailValidation()
    {
        // arrange
        var first = new ComparableObject
        {
            Value = string.Empty,
            NestableObject = new()
            {
                IntValue = 1,
                StringValue = null
            }
        };

        var validator = new ComparableObjectValidator();
        
        // act
        var validationResult = validator.TestValidate(first);
        
        Assert.Multiple(() =>
        {

            // assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors, Is.Not.Empty);
        });
    }
}