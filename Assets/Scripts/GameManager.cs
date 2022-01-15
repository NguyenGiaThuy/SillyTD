using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public delegate void OnStateChangedHandler(GameState gameState);
    public event OnStateChangedHandler StateChanged;

    public enum GameState
    {
        Null,
        PausedFromPreparing,
        PausedFromPlaying,
        Preparing,
        Playing,
        Victorious,
        Lost
    }

    [field: SerializeField]
    public GameState State { get; private set; }

    public delegate void OnLostHandler();
    public event OnLostHandler Lost;
    public delegate void OnWonHandler();
    public event OnWonHandler Won;

    [field: SerializeField]
    public PlayerStats playerStats;

    [SerializeField]
    private LevelData levelStats;

    private WaveSpawner[] waveSpawners;

    private void Awake()
    {
        if (gameManager != null) Destroy(gameObject);
        else gameManager = this;

        StateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        StateChanged -= GameManager_OnStateChanged;
        foreach (WaveSpawner waveSpawner in waveSpawners) waveSpawner.StateChanged -= WaveSpawner_StateChanged;
    }

    private void Start()
    {
        waveSpawners = FindObjectsOfType<WaveSpawner>();
        foreach(WaveSpawner waveSpawner in waveSpawners) waveSpawner.StateChanged += WaveSpawner_StateChanged;
        SetState(GameState.Playing);
    }

    private void Update()
    {
        switch(State)
        {
            case GameState.PausedFromPreparing:
                //if (Input.GetKeyDown(KeyCode.Escape)) SetState(GameState.Preparing);
                break;
            case GameState.PausedFromPlaying:
                //if (Input.GetKeyDown(KeyCode.Escape)) SetState(GameState.Playing);
                break;
            case GameState.Preparing:
                //if (Input.GetKeyDown(KeyCode.Escape)) SetState(GameState.PausedFromPreparing);
                break;
            case GameState.Playing:
                //if (Input.GetKeyDown(KeyCode.Escape)) SetState(GameState.PausedFromPlaying);
                break;
            case GameState.Victorious:
                // Logics for victory
                break;
            case GameState.Lost:
                // Logics for losing
                break;
        }
    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == State) return;

        State = newGameState;
        StateChanged?.Invoke(newGameState);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        levelStats = Resources.Load<LevelData>("LevelData1");
        playerStats = new PlayerStats(levelStats.initialCredits, levelStats.initialLives, 0);
        SetState(GameState.Preparing);
    }

    public void NewLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        levelStats = Resources.Load<LevelData>("LevelStats" + SceneManager.GetActiveScene().buildIndex + 1);
        playerStats.UpdateStats(levelStats.initialCredits, levelStats.initialLives);
        SetState(GameState.Preparing);
    }

    public void PlayGame()
    {
        // Logic here
        SetState(GameState.Playing);
    }

    public void LoadGame()
    {
        // Load game here
        SetState(GameState.Playing);
    }

    public void AutoSave()
    {
        // Save game here
        SetState(GameState.Playing);
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

    private void UnpauseGame()
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

    // React to each game state
    private void GameManager_OnStateChanged(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.PausedFromPreparing:
                PauseGame();
                break;
            case GameState.PausedFromPlaying:
                PauseGame();
                break;
            case GameState.Preparing:
                UnpauseGame();
                break;
            case GameState.Playing:
                UnpauseGame();
                break;
            case GameState.Victorious:
                // Logics for victory
                break;
            case GameState.Lost:
                // Logics for losing
                break;
        }
    }

    // Check if player is victorious
    private void WaveSpawner_StateChanged(WaveSpawner.WaveSpawnerState waveSpawnerState)
    {
        if (waveSpawnerState == WaveSpawner.WaveSpawnerState.Inactive && WaveSpawner.Counter == 0) SetState(GameState.Victorious);
    }
}
