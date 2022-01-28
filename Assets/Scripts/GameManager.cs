using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    public int currentLevel;
    public int meteoritesExploded = 0;
    public int meteoritesHit = 0;
    public int goldsEarned = 0;

    [SerializeField] Text textLevel;

    [SerializeField] Text textGold;

    [SerializeField] MoveObjectsOnMap moveObject;

    [SerializeField] Transform parentMovingObject;
    [SerializeField] List<MeteoriteLayout> layoutList;
    [SerializeField] AssetReference meteoriteAsset;
    [SerializeField] GameObject overTriggerPrefab;
    [SerializeField] Text gameOverText;

    [SerializeField] GameObject youWonWindow;
    [SerializeField] GameObject gameOverWindow;
    
    [SerializeField] Text youWonText;
    
    [SerializeField] Image healthFill;

    [SerializeField] float difficultyIncreaseByLevel;

    public float health = 100f;
    public float maxHealth = 100f;

    public float levelDifficulty = 1f; // (1-1000)

    void Update() {
        textGold.text = PlayerPrefs.GetInt("gold").ToString();
        healthFill.fillAmount = health / maxHealth;
        if(health < 0) {
            gameOver();
        }
    }

    public void gameOver() {
        gameOverWindow.SetActive(true);
        moveObject.speed = 0;
        gameOverText.text = "You've exploded " + meteoritesExploded + " meteorites.";
    }
    

    public void healthDown(float minusHealth) {
        health -= minusHealth;
    }
    
    void createMeteorite(Vector3 positionMeteorite, int meteoriteHealth = 1, int meteoriteScaleMultiplier = 1) {
        var op = Addressables.LoadAssetAsync<GameObject>(meteoriteAsset); // load the meteoriteAsset prefab
        op.Completed += (operation) => { // when the operation is completed instantiate the loaded asset
            instantiateMeteorite(meteoriteAsset,positionMeteorite, meteoriteHealth, meteoriteScaleMultiplier);
        };
    }

    void instantiateMeteorite(AssetReference asset, Vector3 pos, int meteoriteHealth = 1, int meteoriteScaleMultiplier = 1) {
        asset.InstantiateAsync(pos,Quaternion.identity).Completed += (asyncOperationHandle) =>
        {
            GameObject ins = asyncOperationHandle.Result;
            ins.GetComponent<IndependentMeteorite>().health = meteoriteHealth;
            ins.GetComponent<IndependentMeteorite>().scaleMultiplier = meteoriteScaleMultiplier;
            ins.GetComponent<IndependentMeteorite>().operation = asyncOperationHandle;
            ins.transform.SetParent(parentMovingObject);
            ins.transform.localScale = meteoriteScaleMultiplier * ins.transform.localScale;

        };
    }

    void Start() {
        maxHealth = 100f+ PlayerPrefs.GetInt("upgrade1") * 3f + PlayerPrefs.GetInt("upgrade1") * 5f + PlayerPrefs.GetInt("upgrade1") * 50f;
        maxHealth = health;
        if(GameObject.Find("New Game Object") != null) {
        currentLevel = GameObject.Find("New Game Object").GetComponent<LevelInfoPasser>().level;
        Destroy(GameObject.Find("New Game Object"));
        levelDifficulty = difficultyIncreaseByLevel * currentLevel;
        } else {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
            
        }
        textLevel.text = currentLevel.ToString();
        createMap((int)Mathf.Round(Mathf.Clamp(currentLevel*3,8,30)), levelDifficulty);
        print(levelDifficulty);
        
    }

    void createLayoutFromLayout(MeteoriteLayout layout, float addHeight){
        foreach(Vector3 vec3 in layout.objectPositionList) {
            int randomMeteoriteHealth = (int)(Mathf.Clamp(Mathf.Floor(Random.Range(1,10)*levelDifficulty/10),1,100));
            createMeteorite(vec3 + new Vector3(0,addHeight,0),randomMeteoriteHealth,1);
        }
    }

    void createMap(int layoutCount, float difficulty) {
        int lastSelectedIndex = -1;
        float heightCursor = 0f;
        for(int a = 0; a < layoutCount; a++) {
            bool isCorrectDifficultyFound = false;
            print(isCorrectDifficultyFound);
            while(isCorrectDifficultyFound == false) {
                int randomIndex = Random.Range(0,layoutList.Count);
                MeteoriteLayout selectedLayout = layoutList[randomIndex];
                if(difficulty > selectedLayout.difficulty && randomIndex != lastSelectedIndex) {
                    print(layoutList[randomIndex]);
                    lastSelectedIndex = randomIndex;
                    heightCursor += selectedLayout.yHeight;
                    createLayoutFromLayout(selectedLayout, heightCursor);
                    
                    isCorrectDifficultyFound = true;
                }
            }
        }
        GameObject overObject = Instantiate(overTriggerPrefab,new Vector3(0,heightCursor,0),Quaternion.identity);
        overObject.transform.SetParent(parentMovingObject);
    }

    public void OpenYouWonMenu() {
        youWonWindow.SetActive(true);
        youWonText.text = "You've exploded " + meteoritesExploded + " meteorites.";
        if(currentLevel == PlayerPrefs.GetInt("currentLevel")) {
            PlayerPrefs.SetInt("currentLevel",PlayerPrefs.GetInt("currentLevel")+1);
        }
        moveObject.speed = 0;
    }

    public void goToScene(int scene) {
        if(scene == 1) {
            GameObject levelInfo = new GameObject();
            LevelInfoPasser lip = levelInfo.AddComponent<LevelInfoPasser>();
            lip.level = currentLevel+1;
            DontDestroyOnLoad(levelInfo);
            SceneManager.LoadScene(scene);
        } else if(scene == -1) {
            GameObject levelInfo = new GameObject();
            LevelInfoPasser lip = levelInfo.AddComponent<LevelInfoPasser>();
            lip.level = currentLevel;
            DontDestroyOnLoad(levelInfo);
            SceneManager.LoadScene(1);
        } else {
            SceneManager.LoadScene(scene);
        }
    }
}
