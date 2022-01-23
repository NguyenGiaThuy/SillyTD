using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStateManager.GameState CurrentState { get { return gameStateManager.CurrentState; } }
    public GameStateManager.GameState savedState;
    public GameStateManager.GameState previousGameState;
    public PlayerStats playerStats;
    public int levelIndex;
    [SerializeField]
    private float sceneLoadingTime;
    [SerializeField]
    private TurretBlueprint turretBlueprint;

    // Hidden fields
    private GameStateManager gameStateManager;
    private GameObject loadingCanvas;
    private WaveSpawner[] waveSpawners;
    private LevelData levelStats;
    private GameData gameData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        gameStateManager = new GameStateManager();
        gameStateManager.StateChanged += GameStateManager_StateChanged;
        loadingCanvas = GameObject.Find("LoadingCanvas");
        loadingCanvas.SetActive(false);
        DontDestroyOnLoad(loadingCanvas);
    }



    //########## State management methods STARTS ##########
    public void SetNewState(GameStateManager.GameState newGameState)
    {
        previousGameState = CurrentState;
        gameStateManager.SetNewState(newGameState);
    }
    public void SubscribeToGameStateChanged(GameStateManager.OnStateChangedHandler handler)
    {
        gameStateManager.StateChanged += handler;
    }
    public void UnsubscribeToGameStateChanged(GameStateManager.OnStateChangedHandler handler)
    {
        gameStateManager.StateChanged -= handler;
    }
    // Loading screen
    IEnumerator SetNewStateAfter(GameStateManager.GameState newGameState, float seconds, bool enableLoadingScreen)
    {
        if (enableLoadingScreen) loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(seconds);
        SetNewState(newGameState);

        loadingCanvas.SetActive(false);
    }
    private void GameStateManager_StateChanged(GameStateManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateManager.GameState.MainMenu:
                break;
            case GameStateManager.GameState.New:
                LoadLevel(1);
                StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Initializing, sceneLoadingTime / 2, true));
                break;

            case GameStateManager.GameState.Resuming:
                gameData = LoadData();
                LoadGameManger(gameData.gameManagerData);
                LoadLevel(levelIndex);
                StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Initializing, sceneLoadingTime / 2, true));
                break;

            case GameStateManager.GameState.Saving:
                SaveData();
                SetNewState(previousGameState);
                break;
            case GameStateManager.GameState.Initializing:
                InitializeGameManager();
                switch(previousGameState)
                {
                    case GameStateManager.GameState.New:
                        StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Preparing, sceneLoadingTime / 2, true));
                        break;
                    case GameStateManager.GameState.Resuming:
                        InitializeLevel();
                        StartCoroutine(SetNewStateAfter(savedState, sceneLoadingTime / 2, true));
                        break;
                }
                break;
            case GameStateManager.GameState.Preparing:
                switch (previousGameState)
                {
                    case GameStateManager.GameState.Initializing:
                        SaveData();
                        break;
                    case GameStateManager.GameState.Pausing:
                        UnPauseGame();
                        break;
                }
                break;
            case GameStateManager.GameState.Playing:
                if (previousGameState == GameStateManager.GameState.Pausing) UnPauseGame();
                break;
            case GameStateManager.GameState.Pausing:
                PauseGame();
                break;
            case GameStateManager.GameState.Victorious:
                break;
            case GameStateManager.GameState.Lost:
                break;
        }
    }
    //########## State management methods ENDS ##########



    //########## Loading methods STARTS ##########
    private GameData LoadData()
    {
        GameData gameData = null;
        string path = Path.Combine(Application.persistentDataPath, "gamesave.dat");
        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            gameData = binaryFormatter.Deserialize(file) as GameData;

            file.Close();

            Debug.Log("Loaded game from " + path);
        }

        return gameData;
    }
    private void LoadAttackTurrets(List<AttackTurretData> attackTurretDatas)
    {
        foreach (AttackTurretData attackTurretData in attackTurretDatas)
        {
            AttackTurret attackTurret = Instantiate(turretBlueprint.models[attackTurretData.id].turretPrefab, null).GetComponent<AttackTurret>();
            attackTurretData.Load(attackTurret);
        }
    }
    private void LoadNodes(List<NodeData> nodeDatas)
    {
        Node[] nodes = FindObjectsOfType<Node>();
        Turret[] turrets = FindObjectsOfType<Turret>();
        foreach (NodeData nodeData in nodeDatas)
            foreach (Node node in nodes)
            {
                Vector3 position = new Vector3(nodeData.position[0], nodeData.position[1], nodeData.position[2]);
                if (node.transform.position == position)
                {
                    foreach (Turret turret in turrets)
                        if (turret.ID == nodeData.turretID)
                        {
                            node.SetTurret(turret);
                            break;
                        }
                    break;
                }
            }
    }
    private void LoadGameManger(GameManagerData gameManagerData)
    {
        gameManagerData.Load(this);
    }
    //########## Loading methods ENDS ##########



    //########## Saving methods STARTS ##########
    private void SaveData()
    {
        List<AttackTurretData> attackTurretDatas = SaveAttackTurrets();
        List<NodeData> nodeDatas = SaveNodes();
        GameManagerData gameManagerData = SaveGameManager();
        GameData gameData = new GameData();
        gameData.Save(attackTurretDatas, nodeDatas, gameManagerData);

        string path = Path.Combine(Application.persistentDataPath, "gamesave.dat");
        FileStream file = File.Create(path);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(file, gameData);
        file.Close();

        Debug.Log("Saved game to " + path);
    }
    private List<AttackTurretData> SaveAttackTurrets()
    {
        AttackTurret[] attackTurrets = FindObjectsOfType<AttackTurret>();
        List<AttackTurretData> attackTurretDatas = new List<AttackTurretData>();
        foreach(AttackTurret attackTurret in attackTurrets)
        {
            AttackTurretData attackTurretData = new AttackTurretData();
            attackTurretData.Save(attackTurret);
            attackTurretDatas.Add(attackTurretData);
        }
        return attackTurretDatas;
    }
    private List<NodeData> SaveNodes()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        List<NodeData> nodeDatas = new List<NodeData>();
        foreach (Node node in nodes)
        {
            NodeData nodeData = new NodeData();
            nodeData.Save(node);
            nodeDatas.Add(nodeData);
        }
        return nodeDatas;
    }
    private GameManagerData SaveGameManager()
    {
        GameManagerData gameManagerData = new GameManagerData();
        gameManagerData.Save(this);
        return gameManagerData;
    }
    //########## Saving methods ENDS ##########



    //########## Utility methods STARTS ##########
    private void InitializeGameManager()
    {
        levelStats = Resources.Load<LevelData>("LevelData" + SceneManager.GetActiveScene().buildIndex);

        // Subscribe to WaveSpawner event
        waveSpawners = FindObjectsOfType<WaveSpawner>();
        foreach (WaveSpawner waveSpawner in waveSpawners) waveSpawner.StateChanged += WaveSpawner_StateChanged;

        // Logics:
        // - Load player stats from file
        // - If saved state = {preparing, victorious} then overwrite player stats with level Stats and return
        // - If saved state = {playing, lost} then load all saved objects
    }
    private void InitializeLevel()
    {
        LoadAttackTurrets(gameData.attackTurretDatas);
        LoadNodes(gameData.nodeDatas);
    }
    private void WaveSpawner_StateChanged(WaveSpawner.WaveSpawnerState waveSpawnerState)
    {
        if (waveSpawnerState == WaveSpawner.WaveSpawnerState.Inactive && WaveSpawner.Counter == 0) SetNewState(GameStateManager.GameState.Victorious);
    }
    //########## Utility methods ENDS ##########



    //########## Game-flow control methods STARTS ##########
    private void LoadLevel(int levelIndex)
    {
        this.levelIndex = levelIndex;
        SceneManager.LoadScene(levelIndex);
    }
    private void PauseGame()
    {
        Turret[] turrets = FindObjectsOfType<Turret>();
        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        Mob[] mobs = FindObjectsOfType<Mob>();
        Node[] nodes = FindObjectsOfType<Node>();

        foreach (Turret turret in turrets) turret.enabled = false;
        foreach (Projectile projectile in projectiles) projectile.enabled = false;
        foreach (Mob mob in mobs) mob.enabled = false;
        foreach (Node node in nodes) node.GetComponent<BoxCollider>().enabled = false;

        Time.timeScale = 0f;
    }
    private void UnPauseGame()
    {
        Turret[] turrets = FindObjectsOfType<Turret>();
        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        Mob[] mobs = FindObjectsOfType<Mob>();
        Node[] nodes = FindObjectsOfType<Node>();

        foreach (Turret turret in turrets) turret.enabled = true;
        foreach (Projectile projectile in projectiles) projectile.enabled = true;
        foreach (Mob mob in mobs) mob.enabled = true;
        foreach (Node mob in nodes) mob.GetComponent<BoxCollider>().enabled = true;

        Time.timeScale = 1f;
    }
    //########## Game-flow control methods ENDS ##########
}
