using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class OrderObjectiveTests
{
    private OrderObjective _orderObjective;
    private List<IPlayerManager> _players = new List<IPlayerManager>();
    private List<int> _requiredIndexOrder = new List<int>();

    [SetUp]
    public void SetUp()
    {
        _orderObjective = new GameObject().AddComponent<OrderObjective>();
        _orderObjective.Construct(_requiredIndexOrder, _players);
    }

    [TearDown]
    public void TearDown()
    {
        _players.Clear();
        _requiredIndexOrder.Clear();
        Object.Destroy(_orderObjective.gameObject);
    }

    private void CreatePlayerMocks(int numberOfMocks)
    {
        for (int i = 0; i < numberOfMocks; i++)
        {
            IPlayerManager player = Substitute.For<IPlayerManager>();
            _players.Add(player);
        }
    }

    [Test]
    public void UpdateCompletion_PassesObjectiveRequiredLapsCompletedInRequiredOrder()
    {
        CreatePlayerMocks(3);
        foreach (IPlayerManager player in _players)
        {
            player.CurrentLap.Returns(1);
        }

        _orderObjective.Construct(1, false, false, Substitute.For<IPlayerManager>());
        _requiredIndexOrder.AddRange(new List<int> { 0, 1, 2 });

        _orderObjective.UpdateCompletion();

        Assert.IsTrue(_orderObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_DoesNotPassRequiredLapsNotCompleted()
    {
        CreatePlayerMocks(3);
        foreach (IPlayerManager player in _players)
        {
            player.CurrentLap.Returns(0);
        }

        _orderObjective.Construct(1, false, false, Substitute.For<IPlayerManager>());
        _requiredIndexOrder.AddRange(new List<int> { 0, 1, 2 });

        _orderObjective.UpdateCompletion();

        Assert.IsFalse(_orderObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_FailsAndDoesNotPassRequiredOrderIncorrect()
    {
        CreatePlayerMocks(3);
        _players[0].CurrentLap.Returns(0);
        _players[1].CurrentLap.Returns(1);
        _players[2].CurrentLap.Returns(1);

        _orderObjective.Construct(1, false, false, Substitute.For<IPlayerManager>());
        _requiredIndexOrder.AddRange(new List<int> { 0, 1, 2 });

        _orderObjective.UpdateCompletion();

        Assert.IsTrue(_orderObjective.Failed);
    }
}
