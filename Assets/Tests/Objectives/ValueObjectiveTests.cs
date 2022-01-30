using NUnit.Framework;
using UnityEngine;

[Category("ObjectiveTests")]
public class ValueObjectiveTests
{
    private ValueObjective _valueObjective;

    [SetUp]
    public void SetUp()
    {
        _valueObjective = new GameObject().AddComponent<ValueObjective>();
        Statement.SetUpEnvironment();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_valueObjective.gameObject);
    }

    [Test]
    public void IsComplete_ReturnsTrueIfVariableValueEqualsTargetValue()
    {
        Statement.Environment.Add("test", 50f);
        _valueObjective.Construct("test", 50);

        bool result = _valueObjective.IsComplete();

        Assert.IsTrue(result);
    }

    [Test]
    public void IsComplete_ReturnsFalseIfVariableValueDoesNotEqualTargetValue()
    {
        Statement.Environment.Add("test", 40f);
        _valueObjective.Construct("test", 50);

        bool result = _valueObjective.IsComplete();

        Assert.IsFalse(result);
    }
}
