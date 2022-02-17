using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using UnityEngine.UI;

[Category("VPLTests")]
public class DataStructureDefinitionTests
{
    private LinkedListDefinition _linkedListDef;
    private TestHelper _testHelper;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();
        _testHelper = _testObject.AddComponent<TestHelper>();
        _linkedListDef = _testObject.AddComponent<LinkedListDefinition>();

    }

    [TearDown]
    public void TearDown()
    {
        Statement.Environment.Clear();
        GameManager.Players.Clear();
        Object.Destroy(_testObject);
    }

    [Test]
    public void LinkedList_Run_SetsCarVariable_WithNode()
    {
        List<IPlayerManager> testPlayers = new List<IPlayerManager>
        {
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>()
        };

        GameManager.Players.AddRange(testPlayers);
        _testHelper.RunCoroutine(_linkedListDef.Run());
        
        Assert.IsInstanceOf<Node<IPlayerManager>>(Statement.Environment["car"]);
    }

    [Test]
    public void LinkedList_Run_GameManagerPlayer_AsLinkedNodes()
    {
        List<IPlayerManager> testPlayers = new List<IPlayerManager>
        {
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>()
        };

        GameManager.Players.AddRange(testPlayers);
        _testHelper.RunCoroutine(_linkedListDef.Run());
        
        Assert.AreSame(testPlayers[0], Statement.Environment.Get<Node<IPlayerManager>>("car").Data);
        Assert.AreSame(testPlayers[1], Statement.Environment.Get<Node<IPlayerManager>>("car").Next.Data);
    }
}
