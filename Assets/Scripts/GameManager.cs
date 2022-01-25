using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public delegate void OnLevelInitializedHandler();
    public event OnLevelInitializedHandler OnLevelInitialized;

    public GameStateManager.GameState CurrentState { get { return gameStateManager.CurrentState; } }
    public GameStateManager.GameState savedState;
    public GameStateManager.GameState previousGameState;
    public PlayerStats playerStats;
    public int levelIndex;
    [SerializeField]
    private float sceneLoadingTime;
    [SerializeField]
    private int autoSaveFrequency;
    [SerializeField]
    private TurretBlueprint turretBlueprint;

    // Hidden fields
    private GameStateManager gameStateManager;
    private GameObject loadingCanvas;
    private LevelStats levelStats;
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
        gameStateManager.OnStateChanged += GameStateManager_OnStateChanged;
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
    public void SubscribeToOnGameStateChanged(GameStateManager.OnStateChangedHandler handler)
    {
        gameStateManager.OnStateChanged += handler;
    }
    public void UnsubscribeToOnGameStateChanged(GameStateManager.OnStateChangedHandler handler)
    {
        gameStateManager.OnStateChanged -= handler;
    }
    // Loading screen
    IEnumerator SetNewStateAfter(GameStateManager.GameState newGameState, float seconds, bool enableLoadingScreen)
    {
        if (enableLoadingScreen) loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(seconds);
        SetNewState(newGameState);

        loadingCanvas.SetActive(false);

        Debug.Log(newGameState);
    }
    private void GameStateManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateManager.GameState.MainMenu:
                break;
            case GameStateManager.GameState.New:
                LoadLevel(1);
                SetNewState(GameStateManager.GameState.Initializing);
                break;
            case GameStateManager.GameState.Resuming:
                gameData = LoadData();
                LoadGameManger(gameData.gameManagerData);

                // If player wins last game, re-init new level and new game state
                if (savedState == GameStateManager.GameState.Victorious)
                {
                    levelIndex += 1;
                    savedState = GameStateManager.GameState.Preparing;
                }

                LoadLevel(levelIndex);
                SetNewState(GameStateManager.GameState.Initializing);
                break;
            case GameStateManager.GameState.Initializing:
                switch (previousGameState)
                {
                    case GameStateManager.GameState.New:
                        StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Preparing, sceneLoadingTime, true));
                        InitializeGameManager();
                        break;
                    case GameStateManager.GameState.Resuming:
                        StartCoroutine(SetNewStateAfter(savedState, sceneLoadingTime, true));
                        InitializeGameManager();
                        StartCoroutine(InitializeLevelAfter(sceneLoadingTime / 3));
                        break;
                }
                break;
            case GameStateManager.GameState.Preparing:
                if(CurrentState == GameStateManager.GameState.Pausing) UnPauseGame();
                break;
            case GameStateManager.GameState.Playing:
                if (previousGameState == GameStateManager.GameState.Pausing) UnPauseGame();
                break;
            case GameStateManager.GameState.Pausing:
                PauseGame();
                break;
            case GameStateManager.GameState.Victorious:
                Debug.Log("Victorious");
                SaveData();
                break;
            case GameStateManager.GameState.Lost:
                Debug.Log("Lost");
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
    private void LoadWaveManager(WaveManagerData waveManagerData)
    {
        waveManagerData.Load(WaveManager.Instance);
    }
    private void LoadGameManger(GameManagerData gameManagerData)
    {
        gameManagerData.Load(this);
    }
    private void LoadPlayerStats(PlayerData playerData)
    {
        playerData.Load(playerStats);
    }
    //########## Loading methods ENDS ##########



    //########## Saving methods STARTS ##########
    private void SaveData()
    {
        List<AttackTurretData> attackTurretDatas = null;
        List<NodeData> nodeDatas = null;
        WaveManagerData waveManagerData = null;

        // Only save scene if the game is still playing
        if (CurrentState != GameStateManager.GameState.Victorious)
        {
            attackTurretDatas = SaveAttackTurrets();
            nodeDatas = SaveNodes();
            waveManagerData = SaveWaveManager();
        }

        GameManagerData gameManagerData = SaveGameManager();
        PlayerData playerData = SavePlayerStats();
        GameData gameData = new GameData();
        gameData.Save(attackTurretDatas, nodeDatas, waveManagerData, gameManagerData, playerData);

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
    private WaveManagerData SaveWaveManager()
    {
        WaveManagerData waveManagerData = new WaveManagerData();
        waveManagerData.Save(WaveManager.Instance);
        return waveManagerData;
    }
    private GameManagerData SaveGameManager()
    {
        GameManagerData gameManagerData = new GameManagerData();
        gameManagerData.Save(this);
        return gameManagerData;
    }
    private PlayerData SavePlayerStats()
    {
        PlayerData playerData = new PlayerData();
        playerData.Save(playerStats);
        return playerData;
    }
    //########## Saving methods ENDS ##########



    //########## Utility methods STARTS ##########
    private void InitializeGameManager()
    {
        levelStats = Resources.Load<LevelStats>("LevelStats" + levelIndex);

        // Subscribe to WaveManager events
        WaveManager.OnStateChanged -= WaveManager_OnStateChanged;
        WaveManager.OnStateChanged += WaveManager_OnStateChanged;

        playerStats = new PlayerStats();
        playerStats.LoadLevelStats(levelStats);
    }
    private IEnumerator InitializeLevelAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        LoadAttackTurrets(gameData.attackTurretDatas);
        LoadNodes(gameData.nodeDatas);
        LoadWaveManager(gameData.waveManagerData);
        LoadPlayerStats(gameData.playerData);
        OnLevelInitialized?.Invoke();
    }
    private void WaveManager_OnStateChanged(WaveManager.WaveState waveState)
    {
        switch(waveState)
        {
            case WaveManager.WaveState.Started:
                break;
            case WaveManager.WaveState.Ended:
                if (WaveManager.Instance.CurrentWaveIndex % autoSaveFrequency == 0) SaveData();
                break;
            case WaveManager.WaveState.AllEnded:
                SetNewState(GameStateManager.GameState.Victorious);
                break;
        } 
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
