using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
{
    [Header("Planes")]
    [SerializeField] private List<GameObject> _enemyPlanePrefabs;
    [SerializeField] private GameObject _playerPlanePrefab;

    [Header("Spawn Transforms")][Space(20)]
    [SerializeField] private Transform _enemySpawnTransform;
    [SerializeField] private Transform _playerSpawnTransform;

    [Header("Powerups")][Space(20)]
    [SerializeField] private List<GameObject> _powerupPrefabs;

    private LevelConfigSO _currLevelConfig;
    private int _levelLoaded = -1;

    private PlayerPlaneController _playerPlaneController;
    private List<GameObject> _enemyPlaneObjects = new List<GameObject>();

    private int _currentScore = 0;
    public int CurrentScore
    {
        get{ return _currentScore; }
        set
        {
            _currentScore = value;

            ScoreChanged?.Invoke(this, _currentScore);
        }
    }

    public EventHandler<int> LevelWin;
    public EventHandler<int> ScoreChanged;

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
            List<AIPlaneController> newEnemies = new List<AIPlaneController>();
            for(int i=1; i<=numPlanesToSpawn; i++)
            {
                GameObject plane = GeneratePlane(planeType);
                plane.transform.position = _enemySpawnTransform.position;
                AIPlaneController aiPlaneController = plane.GetComponent<AIPlaneController>();

                if(aiPlaneController == null)
                {
                    Debug.LogErrorFormat("AIPlaneController script not found. i={0}. This shouldn't happen.", i);
                    continue;
                }

                aiPlaneController.PlaneDestroyed += OnPlaneDestroyed;

                newEnemies.Add(aiPlaneController);
            }

            _enemyPlaneObjects.AddRange(newEnemies.Select(enemy => enemy.gameObject));

            Formation formation = GenerateFormation(formationType);
            //List<Transform> formationTransforms = newEnemies.Select(planeObj => planeObj.transform).ToList();
            formation.Initialize(newEnemies, _enemySpawnTransform.position);

            yield return new WaitForSeconds(_currLevelConfig.InterSpawnDelayInSec);
        }

        // Spawn the boss plane, if all other planes are already generated.
        SpawnBossPlane();
    }

    private void SpawnBossPlane()
    {
        // Generate boss
        Formation bossSoloFormation = GenerateFormation(FormationType.SINGLE);

        PlaneType bossPlaneType = _currLevelConfig.BossPlaneType;
        GameObject bossPlane = GeneratePlane(bossPlaneType);

        AIPlaneController bossPlaneController = bossPlane.GetComponent<AIPlaneController>();
        //bossPlane.transform.position = _enemySpawnTransform.position;
        bossPlaneController.PlaneDestroyed += OnPlaneDestroyed;
        bossPlaneController.IsBoss = true;

        bossSoloFormation.Initialize(new List<AIPlaneController>() { bossPlaneController }
                                     , _enemySpawnTransform.position);

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

            CurrentScore += controller.PlaneConfig.GetRewardScore();
            controller.PlaneDestroyed -= OnPlaneDestroyed;

            // Level win
            if(controller.IsBoss)
            {
                LevelWin?.Invoke(this, _levelLoaded);
                GameManager.Instance.LoadMenuScene();
            }
        }
    }

    public void SpawnPowerup(PowerupType powerupType, Vector2 pos)
    {
        if(powerupType != PowerupType.NONE)
        {
            int powerupIndex = ((int)powerupType - 1);
            if(powerupIndex >=0 && powerupIndex < _powerupPrefabs.Count)
            {
                GameObject powerupObject
                    = Instantiate(_powerupPrefabs[powerupIndex], pos, Quaternion.identity);

            }
        }
    }
}
