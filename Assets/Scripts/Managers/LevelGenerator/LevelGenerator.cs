using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
{
    [SerializeField] private List<GameObject> _enemyPlanePrefabs;
    [SerializeField] private GameObject _playerPlanePrefab;

    [Space(20)]
    [SerializeField] private Transform _enemySpawnTransform;
    [SerializeField] private Transform _playerSpawnTransform;

    private LevelConfigSO _currLevelConfig;
    private int _levelLoaded = -1;

    private PlayerPlaneController _playerPlaneController;
    private List<GameObject> _enemyPlaneObjects = new List<GameObject>();

    public EventHandler<int> LevelWin;

    protected override void Awake()
    {
        base.Awake();

        _levelLoaded = MenuManager.LevelToLoad;
        _currLevelConfig = GameManager.Instance.LevelConfigs[_levelLoaded - 1];
    }

    private void Start()
    {
        SpawnPlayer();
        StartCoroutine("SpawnEnemiesCoroutine");
    }

    private void SpawnPlayer()
    {
        GameObject playerObject = Instantiate(_playerPlanePrefab);
        if(playerObject != null)
        {
            playerObject.transform.position = _playerSpawnTransform.position;
            _playerPlaneController = playerObject.GetComponent<PlayerPlaneController>();

            _playerPlaneController.PlaneDestroyed += OnPlaneDestroyed;
        }
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        int numPlanesSpawned = 0;
        while(numPlanesSpawned < _currLevelConfig.TotalEnemiesToBeSpawned
            && _playerPlaneController != null && _playerPlaneController.IsAlive)
        {
            FormationType formationType = _currLevelConfig.PickWeightedRandomFormationType();
            PlaneType planeType = _currLevelConfig.PickWeightedRandomPlaneType();

            // Using hard-coded nums as of now.
            int numPlanesToSpawn = 1;
            if(formationType != FormationType.SINGLE)
            {
                numPlanesToSpawn = 4;
            }

            numPlanesSpawned += numPlanesToSpawn;
            List<GameObject> newEnemies = new List<GameObject>();
            for(int i=1; i<=numPlanesToSpawn; i++)
            {
                GameObject plane = GeneratePlane(planeType);
                plane.transform.position = _enemySpawnTransform.position;
                plane.GetComponent<PlaneController>().PlaneDestroyed += OnPlaneDestroyed;

                newEnemies.Add(plane);
            }

            _enemyPlaneObjects.AddRange(newEnemies);

            Formation formation = GenerateFormation(formationType);
            List<Transform> formationTransforms = newEnemies.Select(planeObj => planeObj.transform).ToList();
            formation.SetupUnits(formationTransforms, _enemySpawnTransform.position);

            yield return new WaitForSeconds(_currLevelConfig.InterSpawnDelayInSec);
        }

        // Generate boss -- No need of formations
        PlaneType bossPlaneType = _currLevelConfig.BossPlaneType;
        GameObject bossPlane = GeneratePlane(bossPlaneType);
        bossPlane.transform.position = _enemySpawnTransform.position;
        bossPlane.GetComponent<PlaneController>().PlaneDestroyed += OnPlaneDestroyed;
        bossPlane.GetComponent<AIPlaneController>().IsBoss = true;

        _enemyPlaneObjects.Add(bossPlane);
    }

    private Formation GenerateFormation(FormationType formationType)
    {
        switch (formationType)
        {
            case FormationType.LINEAR:
                return new LinearFormation();
            case FormationType.BOX:
                return new BoxFormation();
            case FormationType.SINGLE:
                return new SingleFormation();
            default:
                return null;
        }
    }

    private GameObject GeneratePlane(PlaneType planeType)
    {
        return Instantiate(_enemyPlanePrefabs[(int)planeType]);
    }

    private void OnPlaneDestroyed(object sender, EventArgs args)
    {
        if(sender is PlayerPlaneController)
        {
            ((PlayerPlaneController)sender).PlaneDestroyed -= OnPlaneDestroyed;
            // Level Failed
            GameManager.Instance.LoadMenuScene();
        }
        else if(sender is AIPlaneController controller)
        {
            int index = _enemyPlaneObjects.FindIndex(enemyObject => enemyObject.GetHashCode() == sender.GetHashCode());
            if(index >=0 && index < _enemyPlaneObjects.Count)
            {
                _enemyPlaneObjects.RemoveAt(index);
            }

            controller.PlaneDestroyed -= OnPlaneDestroyed;

            // Level win
            if(controller.IsBoss)
            {
                LevelWin?.Invoke(this, _levelLoaded);
                GameManager.Instance.LoadMenuScene();
            }
        }

    }
}
