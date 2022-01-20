using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public GameStateManager.GameState CurrentState { get { return gameStateManager.CurrentState; } }
    public PlayerStats playerStats;
    [SerializeField]
    private float sceneLoadingTime;

    // Hidden fields
    private bool isPausedFromPlaying;
    private bool pauseEnabled;
    private GameStateManager gameStateManager;
    private GameObject loadingCanvas;
    private WaveSpawner[] waveSpawners;
    private GameStateManager.GameState savedState;
    private LevelData levelStats;

    private void Awake()
    {
        if (gameManager != null) Destroy(gameObject);
        else
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        gameStateManager = new GameStateManager();
        savedState = CurrentState;
        gameStateManager.StateChanged += GameStateManager_StateChanged;
        pauseEnabled = false;
    }

    public void SetNewState(GameStateManager.GameState newGameState)
    {
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

    public void NewGame()
    {
        NewLevel();
        playerStats = new PlayerStats(1);
        // Save player stats here
        // Save state here
    }

    public void NewLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // Back to main menu
        if (arg0.buildIndex == 0)
        {
            SetNewState(GameStateManager.GameState.Null);
            return;
        }

        SetNewState(GameStateManager.GameState.Initializing);
    }

    private void InitializeLevel()
    {
        levelStats = Resources.Load<LevelData>("LevelData" + SceneManager.GetActiveScene().buildIndex);

        // Subscribe to WaveSpawner event
        waveSpawners = FindObjectsOfType<WaveSpawner>();
        foreach (WaveSpawner waveSpawner in waveSpawners) waveSpawner.StateChanged += WaveSpawner_StateChanged;

        // Start loading screen with saved state
        StartCoroutine(SetNewStateAfter(GameStateManager.GameState.Playing, sceneLoadingTime));

        // Logics:
        // - Load player stats from file
        // - If saved state = {preparing, victorious} then overwrite player stats with level Stats and return
        // - If saved state = {playing, lost} then load all saved objects
    }
    
    // Loading screen
    IEnumerator SetNewStateAfter(GameStateManager.GameState newGameState, float second)
    {
        loadingCanvas = GameObject.Find("LoadingCanvas");
        loadingCanvas.SetActive(true);
        yield return new WaitForSeconds(second);
        SetNewState(newGameState);
        loadingCanvas.SetActive(false);
    }

    public void ContinueGame()
    {
        // Logics:
        // - Load saved state
        // - Load scene
    }

    public void AutoSave()
    {
        // Save all game objects and player stats
        // Save state
    }

    // Only called while playing
    public void PauseGame()
    {
        if (!pauseEnabled) return;

        Turret[] turrets = FindObjectsOfType<Turret>();
        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        Mob[] mobs = FindObjectsOfType<Mob>();
        Node[] nodes = FindObjectsOfType<Node>();

        foreach (Turret turret in turrets) turret.enabled = false;
        foreach (Projectile projectile in projectiles) projectile.enabled = false;
        foreach (Mob mob in mobs) mob.enabled = false;
        foreach (Node node in nodes) node.GetComponent<BoxCollider>().enabled = false;

        Time.timeScale = 0f;

        SetNewState(GameStateManager.GameState.Pausing);
    }

    // Only called while playing
    public void UnpauseGame()
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

        if(isPausedFromPlaying) SetNewState(GameStateManager.GameState.Playing);
        else SetNewState(GameStateManager.GameState.Preparing);
    }

    

    private void GameStateManager_StateChanged(GameStateManager.GameState gameState)
    {
        switch(gameState)
        {
            case GameStateManager.GameState.Null:
                pauseEnabled = false;
                break;
            case GameStateManager.GameState.Initializing:
                InitializeLevel();
                break;
            case GameStateManager.GameState.Preparing:
                pauseEnabled = true;
                isPausedFromPlaying = false;
                break;
            case GameStateManager.GameState.Playing:
                isPausedFromPlaying = true;
                break;
            case GameStateManager.GameState.Victorious:
                Debug.Log("Victorious");
                break;
            case GameStateManager.GameState.Lost:
                GameManager.gameManager.playerStats.lives = 0;
                Debug.Log("Lost");
                break;
        }
    }

    // Check if player is victorious
    private void WaveSpawner_StateChanged(WaveSpawner.WaveSpawnerState waveSpawnerState)
    {
        if (waveSpawnerState == WaveSpawner.WaveSpawnerState.Inactive && WaveSpawner.Counter == 0) SetNewState(GameStateManager.GameState.Victorious);
    }
}
