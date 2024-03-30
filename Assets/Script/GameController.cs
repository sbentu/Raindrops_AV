using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject TriggerY3;
    public GameObject TriggerY2;
    public GameObject TriggerY1;
    public SpawnManager SpawnManager;
    public TMP_InputField inputField;
    public GameObject pauseMenuPanel;
    public GameObject gamePanel;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject lv0Panel;
    public GameObject lv1Panel;
    public GameObject lv2Panel;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public TMP_Text TimerText;

    private int score;
    private int highscore;
    private float controlloIntervalloSpawn = 3f;
    private float controlloIntervalloSpawnSpeciale = 16f;
    private bool isPaused = false;
    private string tagDaDisattivare = "Goccia";
    private float nuovaSpeed = 0.5f;
    private float tempoTrascorso;

    private void Start()
    {
        SpawnManager.intervalloSpawn = controlloIntervalloSpawn;
        SpawnManager.intervalloSpawnSpeciale = controlloIntervalloSpawnSpeciale;
        SpawnManager.speed = nuovaSpeed;
        int loadedHighscore = PlayerPrefs.GetInt("Highscore", 0);
        highscore = loadedHighscore;

        TriggerY2.SetActive(false);
        TriggerY1.SetActive(false);
        TriggerY3.SetActive(false);
        lv0Panel.SetActive(false);
        lv1Panel.SetActive(false);
        lv2Panel.SetActive(false);

        scoreText.text = "Score: 0";
        highscoreText.text = "Highscore: " + highscore;
        inputField.text = "IMMETTI QUI IL RISULTATO!";

        InvokeRepeating("ModificaVel", 10f, 10f);
    }

    private void Awake()
    {
        Instance = this;
        startPanel.SetActive(true);
        TogglePause();
    }

    private void Update()
    {
        tempoTrascorso += Time.deltaTime;
        Timer();

        if (Input.GetKeyDown(KeyCode.Return) && !isPaused)
        {
            Risultato();
            inputField.text = "IMMETTI IL RISULTATO QUI!";
        }

        if (Input.inputString.Length > 0 && !isPaused && Input.GetKey(KeyCode.Return) == false)
        {
            if (inputField.text == "IMMETTI IL RISULTATO QUI!")
            {
                inputField.text = "";
            }
            inputField.text += Input.inputString;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused && Input.GetKeyDown(KeyCode.Return))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Return) && startPanel.activeSelf == true)
        {
            DisattivaInizio();
        }

        if (Input.GetKeyDown(KeyCode.Return) && gameOverPanel.activeSelf == true)
        {
            ResetGame();
        }
    }

    public void Risultato()
    {
        int risultatoInserito;

        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = "";
            return;
        }

        if (int.TryParse(inputField.text, out risultatoInserito))
        {
            GameObject[] oggettiSpeciali = GameObject.FindGameObjectsWithTag("Speciale");
            bool risultatoSpecialeTrovato = false;

            foreach (GameObject oggettoSpeciale in oggettiSpeciali)
            {
                GocciaConTesto specialeScript = oggettoSpeciale.GetComponent<GocciaConTesto>();
                if (specialeScript != null && specialeScript.Risultato == risultatoInserito)
                {
                    risultatoSpecialeTrovato = true;
                    break;
                }
            }

            if (risultatoSpecialeTrovato)
            {
                GameObject[] oggetti = GameObject.FindGameObjectsWithTag("Goccia");
                foreach (GameObject oggetto in oggetti)
                {
                    oggetto.SetActive(false);
                }
                foreach (GameObject oggettoSpeciale in oggettiSpeciali)
                {
                    oggettoSpeciale.SetActive(false);
                }
                score += 500;
                scoreText.text = "Score: " + score.ToString();
                if (score >= highscore)
                {
                    highscore = score;
                    PlayerPrefs.SetInt("Highscore", highscore);
                    PlayerPrefs.Save();
                    highscoreText.text = "Highscore: " + highscore;
                }
            }
            else
            {
                GameObject[] oggetti = GameObject.FindGameObjectsWithTag("Goccia");
                foreach (GameObject oggetto in oggetti)
                {
                    GocciaConTesto gocciaScript = oggetto.GetComponent<GocciaConTesto>();
                    if (gocciaScript != null && gocciaScript.Risultato == risultatoInserito)
                    {
                        score += 100;
                        oggetto.SetActive(false);
                        scoreText.text = "Score: " + score.ToString();
                        if (score >= highscore)
                        {
                            highscore = score;
                            PlayerPrefs.SetInt("Highscore", highscore);
                            PlayerPrefs.Save();
                            highscoreText.text = "Highscore: " + highscore;
                        }
                        break;
                    }
                }
            }
        }

        inputField.text = "";
    }

    public void HandleCollision()
    {
        if (TriggerY3.activeSelf)
        {
            TriggerY3.SetActive(false);
            TriggerY2.SetActive(true);
            lv0Panel.SetActive(false);
            lv1Panel.SetActive(true);
        }
        else if (TriggerY2.activeSelf)
        {
            TriggerY2.SetActive(false);
            TriggerY1.SetActive(true);
            lv1Panel.SetActive(false);
            lv2Panel.SetActive(true);
        }
        else if (TriggerY1.activeSelf)
        {
            gameOverPanel.SetActive(true);
            gamePanel.SetActive(false);
            lv1Panel.SetActive(false);
            lv2Panel.SetActive(false);
            TriggerY1.SetActive(false);
            DisattivaOggettiByTag();
            Time.timeScale = 0;
        }
    }

    public void TogglePause()
    {
        if (startPanel.activeSelf == true)
        {
            gameOverPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            gamePanel.SetActive(false);
            Time.timeScale = 0;
        }

        else if (gameOverPanel.activeSelf == true && startPanel.activeSelf == false)
        {
            pauseMenuPanel.SetActive(false);
            gamePanel.SetActive(false);
            Time.timeScale = 0;
        }

        else
        {
            isPaused = !isPaused;

            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(isPaused);
            }

            if (gamePanel != null)
            {
                gamePanel.SetActive(!isPaused);
            }

            Time.timeScale = isPaused ? 0 : 1;
        }
    }

    private void DisattivaInizio()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        lv0Panel.SetActive(true);
        Time.timeScale = 1;
        TriggerY3.SetActive(true);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void DisattivaOggettiByTag()
    {
        GameObject[] oggettiConTag = GameObject.FindGameObjectsWithTag(tagDaDisattivare);

        foreach (GameObject oggetto in oggettiConTag)
        {
            oggetto.SetActive(false);
        }
    }

    private void ModificaVel()
    {
        nuovaSpeed += 0.2f;
        SpawnManager.speed = nuovaSpeed;
    }

    private void Timer()
    {
        int secondiTrascorsi = Mathf.FloorToInt(tempoTrascorso);
        TimerText.text = "Timer: " + secondiTrascorsi + "s";
    }

    public void QuitGame()
    {
            Application.Quit();
    }
}
