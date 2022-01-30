using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NSubstitute;

[Category("VPLTests")]
[Category("ExpressionTests")]
public class NullCheckTests
{
    private GameObject _testObject;
    private TMP_Dropdown _operandDropdown;
    private NullCheckExpression _nullExpression;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();

        _nullExpression = _testObject.AddComponent<NullCheckExpression>();
        _operandDropdown = _testObject.AddComponent<TMP_Dropdown>();

        _nullExpression.Construct(_operandDropdown);

        Statement.SetUpEnvironment();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
    }

    [Test]
    public void NullCheck_ReturnsFalseIfVariableNotSet()
    {
        _operandDropdown.AddOptions(new List<string> { "test" });

        bool result = _nullExpression.EvaluateExpression();

        Assert.IsFalse(result);
    }

    [Test]
    public void NullCheck_ReturnsTrueIfVariableIsNotNull()
    {
        Statement.Environment.Add("test", 10f);
        _operandDropdown.AddOptions(new List<string> { "test" });

        bool result = _nullExpression.EvaluateExpression();

        Assert.IsTrue(result);
    }

    [Test]
    public void NullCheckYellowFlag_ReturnsTrueIfYellowFlagExists()
    {
        IRaceFlag yellowFlag = Substitute.For<IRaceFlag>();
        yellowFlag.Flag.Returns(RaceFlag.FlagType.YellowFlag);

        List<IRaceFlag> flags = new List<IRaceFlag> { yellowFlag };
        _nullExpression.Construct(flags);
        _operandDropdown.AddOptions(new List<string> { "YellowFlag" });

        bool result = _nullExpression.EvaluateExpression();

        Assert.IsTrue(result);
    }

    [Test]
    public void NullCheckYellowFlag_ReturnsFalseIfYellowFlagDoesNotExist()
    {
        List<IRaceFlag> flags = new List<IRaceFlag>();
        _nullExpression.Construct(flags);
        _operandDropdown.AddOptions(new List<string> { "YellowFlag" });

        bool result = _nullExpression.EvaluateExpression();

        Assert.IsFalse(result);
    }

    [Test]
    public void NullCheckRedFlag_ReturnsTrueIfRedFlagExists()
    {
        IRaceFlag redFlag = Substitute.For<IRaceFlag>();
        redFlag.Flag.Returns(RaceFlag.FlagType.RedFlag);

        List<IRaceFlag> flags = new List<IRaceFlag> { redFlag };
        _nullExpression.Construct(flags);
        _operandDropdown.AddOptions(new List<string> { "RedFlag" });

        bool result = _nullExpression.EvaluateExpression();

        Assert.IsTrue(result);
    }

    [Test]
    public void NullCheckRedFlag_ReturnsFalseIfRedFlagDoesNotExist()
    {
        List<IRaceFlag> flags = new List<IRaceFlag>();
        _nullExpression.Construct(flags);
        _operandDropdown.AddOptions(new List<string> { "RedFlag" });

        bool result = _nullExpression.EvaluateExpression();

        Assert.IsFalse(result);
    }
}
