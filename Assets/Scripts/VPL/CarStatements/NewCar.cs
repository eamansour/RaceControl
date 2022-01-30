using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCar : CarStatement
{
    private static IPlayerManager s_originalPlayer;
    private static Transform s_spawnPoint;

    [SerializeField]
    private GameObject _carPrefab;
    private List<GameObject> _carObjects = new List<GameObject>();

    public void Construct(GameObject carPrefab)
    {
        _carPrefab = carPrefab;
    }

    protected override void Start()
    {
        base.Start();

        s_originalPlayer = Player;
        s_spawnPoint = GameObject.Find("CarSpawnPoint").transform;
    }

    protected override void Update()
    {
        base.Update();

        // Reset the current player to the level's original player and delete any new players
        if (!GameManager.Instance.LevelStarted && _carObjects.Count > 0)
        {
            foreach (GameObject car in _carObjects)
            {
                Destroy(car);
            }

            SetPlayer(s_originalPlayer);
            _carObjects.Clear();
        }
    }

    public override IEnumerator Run()
    {
        _carObjects.Add(GameObject.Instantiate(_carPrefab, s_spawnPoint.position, s_spawnPoint.rotation));
        yield return new WaitForSeconds(0.03f);

        // Set the current player to the newly created player (including race progress)
        IPlayerManager newPlayer = _carObjects[_carObjects.Count - 1].GetComponent<IPlayerManager>();
        ICar newCar = newPlayer.PlayerCar;
        newPlayer.SetRaceProgress(Player.CurrentLap, Player.CurrentControl, Player.TargetCheckpoint);

        // Update the current player
        SetPlayer(newPlayer);

        GameManager.Instance.Players.Add(newPlayer);
        newCar.SetCarLock(false);
    }
}
