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
        WaveManager.OnStateChanged += WaveManager_OnStateChanged;
        Exit.OnAttacked += Exit_OnAttacked;
        Mob.OnDead += Mob_OnDead;

        BuildManager.InitializeTurretBlueprint();

        loadingCanvas = GameObject.Find("LoadingCanvas");
        loadingCanvas.SetActive(false);
        DontDestroyOnLoad(loadingCanvas);
    }



    //########## State management methods STARTS ##########
    public void SetNewState(GameStateManager.GameState newGameState)
    {
        if (CurrentState == newGameState) return;

        previousGameState = CurrentState;
        gameStateManager.SetNewState(newGameState);
    }
    public void SubscribeToOnStateChanged(GameStateManager.OnStateChangedHandler handler)
    {
        gameStateManager.OnStateChanged += handler;
    }
    public void UnsubscribeToOnStateChanged(GameStateManager.OnStateChangedHandler handler)
    {
        gameStateManager.OnStateChanged -= handler;
    }
    IEnumerator SetNewStateAfter(GameStateManager.GameState newGameState, float seconds, bool loadingScreenEnabled = true) // Loading screen
    {
        if (loadingScreenEnabled) loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(seconds);
        SetNewState(newGameState);

        loadingCanvas.SetActive(false);
    }
    private void GameStateManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateManager.GameState.Preparing:
                if(previousGameState == GameStateManager.GameState.Pausing) UnPauseGame();
                break;
            case GameStateManager.GameState.Playing:
                if (previousGameState == GameStateManager.GameState.Pausing) UnPauseGame();
                break;
            case GameStateManager.GameState.Victorious:
                Debug.Log("Victorious");
                levelIndex++;
                SaveData();
                break;
            case GameStateManager.GameState.Lost:
                Debug.Log("Lost");
                break;
        }
    }
    //########## State management methods ENDS ##########



    //########## Loading methods STARTS ##########
    public void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "gamesave.dat");
        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            gameData = binaryFormatter.Deserialize(file) as GameData;

            file.Close();

            Debug.Log("Loaded game from " + path);
        }
    }
    private void LoadAttackTurrets(List<AttackTurretData> attackTurretDatas)
    {
        foreach (AttackTurretData attackTurretData in attackTurretDatas)
        {
            AttackTurret attackTurret = Instantiate(BuildManager.turretBlueprints[attackTurretData.id].turretPrefab, null).GetComponent<AttackTurret>();
            attackTurretData.Load(attackTurret);
        }
    }
    private void LoadSupportTurrets(List<SupportTurretData> supportTurretDatas)
    {
        foreach (SupportTurretData supportTurretData in supportTurretDatas)
        {
            SupportTurret supportkTurret = Instantiate(BuildManager.turretBlueprints[supportTurretData.id].turretPrefab, null).GetComponent<SupportTurret>();
            supportTurretData.Load(supportkTurret);
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
                            nodeData.Load(node);
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
        List<SupportTurretData> supportTurretDatas = null;
        List<NodeData> nodeDatas = null;
        WaveManagerData waveManagerData = null;

        // Only save scene if the game is still playing
        if (CurrentState != GameStateManager.GameState.Victorious)
        {
            attackTurretDatas = SaveAttackTurrets();
            supportTurretDatas = SaveSupportTurrets();
            nodeDatas = SaveNodes();
            waveManagerData = SaveWaveManager();
        }

        GameManagerData gameManagerData = SaveGameManager();
        PlayerData playerData = SavePlayerStats();
        GameData gameData = new GameData();
        gameData.Save(attackTurretDatas, supportTurretDatas, nodeDatas, waveManagerData, gameManagerData, playerData);

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
    private List<SupportTurretData> SaveSupportTurrets()
    {
        SupportTurret[] supportTurrets = FindObjectsOfType<SupportTurret>();
        List<SupportTurretData> supportTurretDatas = new List<SupportTurretData>();
        foreach (SupportTurret supportTurret in supportTurrets)
        {
            SupportTurretData supportTurretData = new SupportTurretData();
            supportTurretData.Save(supportTurret);
            supportTurretDatas.Add(supportTurretData);
        }
        return supportTurretDatas;
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
    private IEnumerator InitializeGameManagerAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        levelStats = Resources.Load<LevelStats>("LevelStats" + levelIndex);
        playerStats = new PlayerStats();
        playerStats.LoadLevelStats(levelStats);
    }
    private IEnumerator InitializeLevelAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        LoadAttackTurrets(gameData.attackTurretDatas);
        LoadSupportTurrets(gameData.supportTurretDatas);
        LoadNodes(gameData.nodeDatas);
        LoadWaveManager(gameData.waveManagerData);
        LoadPlayerStats(gameData.playerData);
        OnLevelInitialized?.Invoke();
    }
    private void WaveManager_OnStateChanged(WaveManager.WaveState waveState)
    {
        switch(waveState)
        {
            case WaveManager.WaveState.Ended:
                if (CurrentState != GameStateManager.GameState.Victorious 
                    && CurrentState != GameStateManager.GameState.Lost
                    && WaveManager.Instance.CurrentWaveIndex % autoSaveFrequency == 0) SaveData();
                break;
            case WaveManager.WaveState.AllEnded:
                SetNewState(GameStateManager.GameState.Victorious);
                break;
        } 
    }
    private void Exit_OnAttacked(Mob mob)
    {
        playerStats.lives -= mob.lifeDamage;

        if (playerStats.lives <= 0)
        {
            playerStats.lives = 0;
            SetNewState(GameStateManager.GameState.Lost);
        }
    }
    private void Mob_OnDead(Mob mob)
    {
        playerStats.credits += mob.killCredits;
        playerStats.researchPoints += mob.killResearchPoints;
    }
    //########## Utility methods ENDS ##########



    //########## Game-flow control methods STARTS ##########
    public void LoadNewLevel()
    {
        levelIndex = 1;
        SceneManager.LoadScene(levelIndex);
        StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Preparing, sceneLoadingTime));
        StartCoroutine(InitializeGameManagerAfter(sceneLoadingTime / 3));
    }
    public void LoadNextLevel() // Only called when victory
    {
        SceneManager.LoadScene(levelIndex);
        StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Preparing, sceneLoadingTime));
        StartCoroutine(InitializeGameManagerAfter(sceneLoadingTime / 3));
    }
    public void LoadSavedLevel()
    {
        LoadData();
        LoadGameManger(gameData.gameManagerData);
        SceneManager.LoadScene(levelIndex);

        // If player wins last game, re-init new game state
        if (savedState == GameStateManager.GameState.Victorious) savedState = GameStateManager.GameState.Preparing;

        StartCoroutine(SetNewStateAfter(savedState, sceneLoadingTime));
        StartCoroutine(InitializeGameManagerAfter(sceneLoadingTime / 3));
        StartCoroutine(InitializeLevelAfter(sceneLoadingTime / 2));
    }
    public void RestartLevel()
    {
        if (CurrentState == GameStateManager.GameState.Victorious) levelIndex--;

        SceneManager.LoadScene(levelIndex);
        StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Preparing, sceneLoadingTime));
        StartCoroutine(InitializeGameManagerAfter(sceneLoadingTime / 3));
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        SetNewState(GameStateManager.GameState.MainMenu);
    }
    public void PauseGame(bool stateChangeEnabled = true)
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
        if (stateChangeEnabled) SetNewState(GameStateManager.GameState.Pausing);
    }
    public void UnPauseGame(bool stateChangeEnabled = true)
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
        if(stateChangeEnabled) SetNewState(previousGameState);
    }
    //########## Game-flow control methods ENDS ##########
}
