using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public enum Turn
{
    Player,
    Enemy
}
public class GameManager : MonoBehaviour
{
    Board m_board;
    PlayerManager m_playerManager;
    List<EnemyManager> m_enemies;
    Turn m_currentTurn = Turn.Player;
    public Turn CurrentTurn { get { return m_currentTurn; } }
    bool m_hasLevelStarted = false;
    public bool HasLevelStarted { get => m_hasLevelStarted; set => m_hasLevelStarted = value; }
    bool m_isGamePlaying = false;
    public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }
    bool m_isGameOver = false;
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
    bool m_hasLevelFinished = false;
    public bool HasLevelFinished { get => m_hasLevelFinished; set => m_hasLevelFinished = value; }

    public float delay = 1f;

    public UnityEvent startLevelEvent;
    public UnityEvent PlayLevelEvent;
    public UnityEvent endLevelEvent;

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        m_playerManager = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        m_enemies = enemies.ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_playerManager != null && m_board != null)
        {
            StartCoroutine(RunGameLoop());
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER ERROR : No Player Or Borad Found! ");
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine(StartLevelRoutine());
        yield return StartCoroutine(PlayLevelRoutine());
        yield return StartCoroutine(EndLevelRoutine());
    }

    IEnumerator StartLevelRoutine()
    {
        m_playerManager.playerInput.InputEnabled = false;
        while (!m_hasLevelStarted)
        {
            //Show Starts Screen
            //Wait For User Input
            //HasLevelStarted = true
            yield return null;
        }
        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        m_playerManager.playerInput.InputEnabled = true;
        if (PlayLevelEvent != null)
        {
            PlayLevelEvent.Invoke();
        }
        while (!m_isGameOver)
        {
            //Check for Game Over Condition
            //Win
            m_isGameOver = IsWinner();
            //Loose
            //IsGameOver = true
            yield return null;
        }
    }

    IEnumerator EndLevelRoutine()
    {
        m_playerManager.playerInput.InputEnabled = false;
        //Show End Screen
        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }
        while (!m_hasLevelFinished)
        {
            //User Press Button to Continue

            //HasLevelFinished = true
            yield return null;
        }

        RestartLevel();
    }

    void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name); 
    }

    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    bool IsWinner()
    {
        if (m_board.PlayerNode != null)
        {
            return (m_board.PlayerNode == m_board.GoalNode);
        }
        return false;
    }

    void PlayPlayerTurn()
    {
        m_currentTurn = Turn.Player;
        m_playerManager.IsTurnComplete = false;
    }

    void PlayEnemyTurn()
    {
        m_currentTurn = Turn.Enemy;
        foreach (EnemyManager enemy in m_enemies)
        {
            if (enemy != null)
            {
                enemy.IsTurnComplete = false;
                enemy.PlayTurn();
            }
        }
    }

    bool IsEnemyTurnComplete()
    {
        foreach (EnemyManager enemy in m_enemies)
        {
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateTurn()
    {
        if (m_currentTurn == Turn.Player && m_playerManager != null)
        {
            if (m_playerManager.IsTurnComplete)
            {
                PlayEnemyTurn();
            }
        }
        else if (m_currentTurn == Turn.Enemy)
        {
            if (IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }
        }
    }
}
