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
    public void UpdateCompletion_PassesObjectiveIfVariableValueEqualsTargetValue()
    {
        Statement.Environment.Add("test", 50f);
        _valueObjective.Construct("test", 50);

        _valueObjective.UpdateCompletion();

        Assert.IsTrue(_valueObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_DoesNotPassIfVariableValueDoesNotEqualTargetValue()
    {
        Statement.Environment.Add("test", 40f);
        _valueObjective.Construct("test", 50);

        _valueObjective.UpdateCompletion();

        Assert.IsFalse(_valueObjective.Passed);
    }
}
