using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

[Category("ExpressionTests")]
public class LiteralExpressionTests
{
    private GameObject _testObject;
    private FloatLiteral _floatLiteral;
    private NodeLiteral _nodeLiteral;
    private TMP_Dropdown _literalDropdown;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();

        _floatLiteral = _testObject.AddComponent<FloatLiteral>();
        _nodeLiteral = _testObject.AddComponent<NodeLiteral>();
        _literalDropdown = new GameObject().AddComponent<TMP_Dropdown>();

        _floatLiteral.Construct(dropdownInput: _literalDropdown);
        _nodeLiteral.Construct(dropdownInput: _literalDropdown);
        Statement.SetUpEnvironment();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
        Object.Destroy(_literalDropdown.gameObject);
    }

    [Test]
    public void FloatLiteral_ShouldParseNumericInputToFloat(
        [Values("-1", "23", "5.87")] string input
    )
    {
        _literalDropdown.AddOptions(new List<string> { input });

        float result = _floatLiteral.EvaluateExpression();

        Assert.AreEqual(float.Parse(input), result);
    }

    [Test]
    public void FloatLiteral_ShouldReturnZeroIfInvalid(
        [Values("BAD", "123BAD", "!!£$%^&123")] string input
    )
    {
        _literalDropdown.AddOptions(new List<string> { input });

        float result = _floatLiteral.EvaluateExpression();

        Assert.AreEqual(0f, result);
    }

    [Test]
    public void FloatLiteral_ShouldReturnVariableFloatValueIfSet()
    {
        Statement.Environment.Add("test", 10f);
        _literalDropdown.AddOptions(new List<string> { "test" });

        float result = _floatLiteral.EvaluateExpression();

        Assert.AreEqual(10f, result);
    }

    [Test]
    public void FloatLiteral_ShouldReturnZeroIfVariableIsNotFloat()
    {
        Statement.Environment.Add("test", new Node<object>());
        _literalDropdown.AddOptions(new List<string> { "test" });

        float result = _floatLiteral.EvaluateExpression();

        Assert.AreEqual(0f, result);
    }

    [Test]
    public void NodeLiteral_ShouldReturnNodeIfVariableSet()
    {
        Node<IPlayerManager> playerNode = new Node<IPlayerManager>();

        Statement.Environment.Add("test", playerNode);
        _literalDropdown.AddOptions(new List<string> { "test" });

        Node<IPlayerManager> result = _nodeLiteral.EvaluateExpression();

        Assert.AreEqual(playerNode, result);
    }

    [Test]
    public void NodeLiteral_ShouldReturnNextNodeIfGetNextRequired()
    {
        _nodeLiteral.Construct(_literalDropdown, true);

        Node<IPlayerManager> playerNode = new Node<IPlayerManager>();
        Node<IPlayerManager> nextNode = new Node<IPlayerManager>();
        playerNode.Next = nextNode;

        Statement.Environment.Add("test", playerNode);
        _literalDropdown.AddOptions(new List<string> { "test" });

        Node<IPlayerManager> result = _nodeLiteral.EvaluateExpression();

        Assert.AreEqual(nextNode, result);
    }
}
