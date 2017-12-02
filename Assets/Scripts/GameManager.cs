using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public static int DEATHS_COUNT = 0;

    Player player;
    public GameObject boss1;
    Camera gameCamera;
    public GameObject aspectRatioGuards;
    public GameObject onScreenControls;
    public GameObject stats;
    Vector3 aspectRGPosition;
    Vector3 cameraResetPosition;
    Vector3 playerResetPosition;
    GameObject boss;
    Vector3 bossPositionReset;
    int lvlNumber = 0;
    public Image score1, score2;
    public static int score = 0;
    public int maxPlayerLives;
    public Image[] livesImages;
    public GameObject[] enemies;
    public GameObject[] stones;


    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            score = 0;
            DEATHS_COUNT = 0;
        }
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            InitGame();
            lvlNumber++;
        }
        
        
    }
    // Use this for initialization
    void Start () {
        boss = GameObject.FindGameObjectWithTag("Boss");
        if(boss != null)
        {
            bossPositionReset = boss.transform.position;
        }
        

    }

    public void HideUi()
    {
        livesImages = new Image[player.lives];
        for (int i = 0; i < stats.transform.GetChild(0).childCount; i++)
        {
            Image life = stats.transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>();
            livesImages[i] = life.GetComponent<Image>();
            livesImages[i].enabled = false;
        }
        for (int i = 0; i < stats.transform.GetChild(1).childCount; i++)
        {
            Image score = stats.transform.GetChild(1).GetChild(i).gameObject.GetComponent<Image>();
            if (i == 0)
            {
                score2 = score;
                score2.enabled = false;
            }
            else
            {
                score1 = score;
                score1.enabled = false;
            }

        }
    }

    // Update is called once per frame
    void Update () {
    }
		
	

    public void InitGame()
    {
        aspectRatioGuards = GameObject.FindGameObjectWithTag("AspRatGuard");
        player = GameObject.FindObjectOfType<Player>();

        gameCamera = GameObject.FindObjectOfType<Camera>();
        playerResetPosition = player.transform.position;
        aspectRGPosition = aspectRatioGuards.transform.position;
        cameraResetPosition = gameCamera.transform.position;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        stones = GameObject.FindGameObjectsWithTag("Throwable");
        stats = GameObject.FindGameObjectWithTag("Stats");
        
        player.lives = maxPlayerLives;
        
        livesImages = new Image[player.lives];
        for(int i =0; i<stats.transform.GetChild(0).childCount; i++)
        {
            Image life = stats.transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>();
            livesImages[i] = life.GetComponent<Image>();
        }
        for (int i = 0; i < stats.transform.GetChild(1).childCount; i++)
        {
            Image score = stats.transform.GetChild(1).GetChild(i).gameObject.GetComponent<Image>();
            if (i == 0)
            {
                score2 = score;
            }
            else
            {
                score1 = score;
            }
        }
        foreach(GameObject stone in stones)
        {
            stone.GetComponent<Stone>().SetIfDispensable(false);
        }
        UpdateScore(0);
        Instantiate(stats);
        ResetLives();
    }

    public void ResetGame()
    {
        //playe reset animation
        player.gameObject.SetActive(true);
        player.ResetAnimation();
        GameObject.FindObjectOfType<BlockBarScript>().GetComponent<BoxCollider2D>().isTrigger = true;
        GameObject.FindObjectOfType<FinishLevelTrigger>().GetComponent<BoxCollider2D>().isTrigger = true;



        Invoke("ResetPlayerPosition", 0.5f);

    }

    private void ResetPlayerPosition()
    {
        //reset camera
        gameCamera.transform.position = cameraResetPosition;
        //reset aspectratioGuards
        aspectRatioGuards.transform.position = aspectRGPosition;
        //reset player position
        player.transform.position = playerResetPosition;
        StopFollowingCamera();
        //reset player velocity
        player.StopMoving();
        if (boss == null && false)
        {
            boss = Instantiate(boss1, bossPositionReset, Quaternion.identity);
            ResetBoss();
        }
        else
        {
            Destroy(boss);
            boss = Instantiate(boss1, bossPositionReset, Quaternion.identity);
            ResetBoss();
        }
        
        gameCamera.ResetEndLevel();
        foreach(GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }
        GameObject disposable = GameObject.Find("Disposable");
        if (disposable != null)
        {
            for(int i = 0; i < disposable.transform.childCount; i++){
                Destroy(disposable.transform.GetChild(i).gameObject);
            }
        }
        foreach(GameObject stone in stones)
        {
            stone.SetActive(true);
            stone.GetComponent<BoxCollider2D>().enabled = true;
        }
        player.lives = maxPlayerLives;
        GameObject.FindObjectOfType<EndLevel>().SetTriggered(false);

        
        ResetLives();
        player.ResetHasStone();

    }

    public void ResetBoss()
    {
        if (boss != null)
        {
            boss.GetComponent<Boss1>().SetMayhame(false);
            boss.GetComponent<Enemy>().lives = 8;
            boss.transform.position = bossPositionReset;
        }
        
    }
    
    public void StopFollowingCamera()
    {
        //reset camera.following
        gameCamera.ResetFollowing();
    }

    public void ToggleControlls()
    {
        onScreenControls.SetActive(!onScreenControls.activeSelf);
    }

    public void UpdateScore(int sc)
    {
        score += sc;
        int secondDecimalNumber = score / 10;
        score1.gameObject.GetComponent<ScoreScript>().ChangeScore(secondDecimalNumber);
        int firstDecimalNumber = score - 10 * secondDecimalNumber;
        score2.gameObject.GetComponent<ScoreScript>().ChangeScore(firstDecimalNumber);
    }

    public void LooseLife(int lives)
    {
        if (lives == 3)
        {
            livesImages[0].gameObject.SetActive(false);
        } else if (lives == 2)
        {
            livesImages[0].gameObject.SetActive(false);
            livesImages[1].gameObject.SetActive(false);
        }
        else if(lives == 1)
        {
            livesImages[0].gameObject.SetActive(false);
            livesImages[1].gameObject.SetActive(false);
            livesImages[2].gameObject.SetActive(false);
        }
        else if (lives == 0)
        {
            livesImages[0].gameObject.SetActive(false);
            livesImages[1].gameObject.SetActive(false);
            livesImages[2].gameObject.SetActive(false);
            livesImages[3].gameObject.SetActive(false);
        }
    }

    public void ResetLives()
    {
        for(int i =0; i<livesImages.Length; i++)
        {
            livesImages[i].gameObject.SetActive(true);
        }
    }

    public void AddLife()
    {
        if(player.lives< maxPlayerLives)
        {
            Debug.Log("Adding life");
            for (int i = livesImages.Length - 1; i >= 0; i--)
            {
                if (!livesImages[i].gameObject.activeSelf)
                {
                    livesImages[i].gameObject.SetActive(true);
                    break;
                }
            }
            player.lives++;
        }else
        {
            Debug.Log("Adding score");
            UpdateScore(1);
        }
        
    }

    public void AddToDeathCount()
    {
        DEATHS_COUNT++;
        Debug.Log("death count: " + DEATHS_COUNT);
        if (DEATHS_COUNT >= 10)
        {
            
            SceneManager.LoadScene(4);
        }
    }
}
