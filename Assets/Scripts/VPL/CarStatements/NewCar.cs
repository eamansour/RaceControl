using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCar : CarStatement
{
    private const int MaxNewCarAmount = 10;
    private static IPlayerManager s_originalPlayer;
    private static Transform s_spawnPoint;
    private static List<GameObject> s_createdCars = new List<GameObject>();

    [SerializeField]
    private GameObject _carPrefab;

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
        if (!GameManager.LevelStarted && s_createdCars.Count > 0)
        {
            foreach (GameObject car in s_createdCars)
            {
                Destroy(car);
            }

            SetPlayer(s_originalPlayer);
            s_createdCars.Clear();
        }
    }

    public override IEnumerator Run()
    {
        // Prevent an infinite amount of cars from spawning
        if (s_createdCars.Count >= MaxNewCarAmount) yield break;

        s_createdCars.Add(GameObject.Instantiate(_carPrefab, s_spawnPoint.position, s_spawnPoint.rotation));
        yield return new WaitForSeconds(0.03f);

        // Set the current player to the newly created player (including race progress)
        IPlayerManager newPlayer = s_createdCars[s_createdCars.Count - 1].GetComponent<IPlayerManager>();
        ICar newCar = newPlayer.PlayerCar;
        newPlayer.CopyRaceProgress(Player);

        // Update the current player
        SetPlayer(newPlayer);

        GameManager.Players.Add(newPlayer);
        newCar.SetCarLock(false);
    }
}
