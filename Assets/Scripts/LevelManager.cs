using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public  class LevelManager : MonoBehaviour {

    const int pixelsBetweenLevelButtons = 300;
    const int totalLevelCount = 100;
    const int taskEveryXSeconds = 500;

    const int upgrade1Multiplier = 100;
    const int upgrade2Multiplier = 200;
    const int upgrade3Multiplier = 2000;


    [SerializeField] GameObject taskBlock;
    [SerializeField] RectTransform scrollElements; 
    [SerializeField] GameObject TaskMenuUnclickable; 
    [SerializeField] GameObject LevelMenuUnclickable; 
    [SerializeField] GameObject UpgradeMenuUnclickable; 
    [SerializeField] List<Tasks> tasksList;

    [SerializeField] Text upgrade1; 
    [SerializeField] Text upgrade1Level; 
    [SerializeField] Text upgrade2; 
    [SerializeField] Text upgrade2Level; 
    [SerializeField] Text upgrade3; 
    [SerializeField] Text upgrade3Level; 

    [SerializeField] Button upgrade1Button; 
    [SerializeField] Button upgrade2Button; 
    [SerializeField] Button upgrade3Button; 

    [SerializeField] Text taskText;
    [SerializeField] Text timeLeftText;
    [SerializeField] Text rewardText;
    [SerializeField] Text moneyText;
    [SerializeField] Button collectButton;

    int timeLeft = 0;


    public int selectedLevelNo;

    void Start() {
        if(PlayerPrefs.HasKey("notNewPlayer") == false) { // if notNewPlayer doesn't exist, player is playing the game for the first time
            resetSaveFile(); // creates save file
        }
        
        Addressables.LoadAssetAsync<GameObject>("LevelButton").Completed += initLevels;
    }
    
    void Update() {
        
        moneyText.text = PlayerPrefs.GetInt("gold").ToString();

        timeLeft = (int)CurrentEpochTime() % taskEveryXSeconds; // Get the modulo of epoch to taskEveryXSeconds 
        timeLeft = taskEveryXSeconds - timeLeft;                    // reverse the time so it goes backwards
        if(timeLeft > 1) {
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timeLeft);
            string timeText = string.Format("{0:D2} hours {1:D2} mins {2:D2} secs", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            timeLeftText.text = timeText;
        } else {
            ToggleTaskMenu(false);
        }
    }

    void resetSaveFile() { // resets the save file
        PlayerPrefs.SetInt("notNewPlayer",1);
        PlayerPrefs.SetInt("gold",200);
        PlayerPrefs.SetInt("currentLevel",1);

        PlayerPrefs.SetInt("upgrade1",1);
        PlayerPrefs.SetInt("upgrade2",1);
        PlayerPrefs.SetInt("upgrade3",1);

        PlayerPrefs.SetInt("lastTaskStartedTime",-1);
        PlayerPrefs.SetInt("lastPlayTime",-2);
        PlayerPrefs.SetInt("lastRewardGotTime",-3);

        PlayerPrefs.SetInt("sMeteoritesExploded",0);
        PlayerPrefs.SetInt("sMeteoritesHit",0);
        PlayerPrefs.SetInt("sGoldEarned",0);

        PlayerPrefs.SetString("currentTask","notset");
    }

    private void initLevels(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj) {
        int curLevel = PlayerPrefs.GetInt("currentLevel");
        scrollElements.sizeDelta = new Vector2(scrollElements.sizeDelta.x,pixelsBetweenLevelButtons*(totalLevelCount+2)) ; // map sizing
        scrollElements.anchoredPosition = new Vector2(scrollElements.anchoredPosition.x, totalLevelCount * pixelsBetweenLevelButtons);
        for(int a = 0; a<totalLevelCount; a++) {
            GameObject ins = Instantiate(obj.Result, new Vector3(0,0,0) ,Quaternion.identity);
            ins.transform.SetParent(scrollElements);
            ins.transform.localScale = new Vector3(1,1,1);
            ins.GetComponent<RectTransform>().anchoredPosition  = new Vector3(Random.Range(200,800),(a*pixelsBetweenLevelButtons)-(pixelsBetweenLevelButtons*((totalLevelCount-1)/2)), 0); // Bolumleri yerlestirme
            ins.transform.GetChild(0).GetComponent<Text>().text = (a+1).ToString();
            if(a >= curLevel) ins.GetComponent<Button>().interactable = false;
            
        }
    }

    public void Upgrade(int upgradeNo) {
        switch(upgradeNo) {
            case 1:
            if(PlayerPrefs.GetInt("gold") >= Mathf.Pow(2,PlayerPrefs.GetInt("upgrade1")-1) * upgrade1Multiplier) {
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold")-(int)Mathf.Pow(2,PlayerPrefs.GetInt("upgrade1")-1) * upgrade1Multiplier);
                print(Mathf.Pow(2,PlayerPrefs.GetInt("upgrade1")-1) * upgrade1Multiplier);
                PlayerPrefs.SetInt("upgrade1",PlayerPrefs.GetInt("upgrade1")+1);
            } else {}
            break;
            case 2:
            if(PlayerPrefs.GetInt("gold") >= Mathf.Pow(2,PlayerPrefs.GetInt("upgrade2")-1) * upgrade2Multiplier) {
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold")-(int)Mathf.Pow(2,PlayerPrefs.GetInt("upgrade2")-1) * upgrade2Multiplier);
                PlayerPrefs.SetInt("upgrade2",PlayerPrefs.GetInt("upgrade2")+1);
            } else {}
            break;
            case 3:
            if(PlayerPrefs.GetInt("gold") >= Mathf.Pow(2,PlayerPrefs.GetInt("upgrade3")-1) * upgrade3Multiplier) {
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold")-(int)Mathf.Pow(2,PlayerPrefs.GetInt("upgrade3")-1) * upgrade3Multiplier);
                PlayerPrefs.SetInt("upgrade3",PlayerPrefs.GetInt("upgrade3")+1);
            } else {}
            break;
        }
        ToggleUpgradeMenu(true);
    }

    public void ToggleUpgradeMenu(bool status) {
        if(status) {
            if(PlayerPrefs.GetInt("upgrade1") <= 20) {
                upgrade1.text = (Mathf.Pow(2,PlayerPrefs.GetInt("upgrade1")-1) * upgrade1Multiplier).ToString();
                upgrade1Level.text = "LV" + PlayerPrefs.GetInt("upgrade1");
            } else {
                upgrade1Button.interactable = false;
                upgrade1.text = "999999";
                upgrade1Level.text = "MAX";
            }

            if(PlayerPrefs.GetInt("upgrade2") <= 15) {
            upgrade2.text = (Mathf.Pow(2,PlayerPrefs.GetInt("upgrade2")-1) * upgrade2Multiplier).ToString();
            upgrade2Level.text = "LV" + PlayerPrefs.GetInt("upgrade2");
            } else {
                upgrade2Button.interactable = false;
                upgrade2.text = "999999";
                upgrade2Level.text = "MAX";
            }

            if(PlayerPrefs.GetInt("upgrade3") <= 5) {
                upgrade3.text = (Mathf.Pow(2,PlayerPrefs.GetInt("upgrade3")-1) * upgrade3Multiplier).ToString();
                upgrade3Level.text = "LV" + PlayerPrefs.GetInt("upgrade3");
            } else {
                upgrade3Button.interactable = false;
                upgrade3.text = "999999";
                upgrade3Level.text = "MAX";
            }
            UpgradeMenuUnclickable.SetActive(true);
        } else {
            UpgradeMenuUnclickable.SetActive(false);
        }
    }

    public void ToggleTaskMenu(bool status) { // to toggle the task pop-up from the buttons

        if(PlayerPrefs.GetInt("lastTaskStartedTime") == -1){
            PlayerPrefs.SetString("currentTask", initializeTask());
            PlayerPrefs.SetInt("lastTaskStartedTime", CurrentEpochTime() - ((int)CurrentEpochTime() % taskEveryXSeconds));
        } 
        if(status) {
            TaskMenuUnclickable.SetActive(true);
            if((CurrentEpochTime() - ((int)CurrentEpochTime() % taskEveryXSeconds)) != PlayerPrefs.GetInt("lastTaskStartedTime")) {
                PlayerPrefs.SetString("currentTask", initializeTask());
                
                PlayerPrefs.SetInt("lastTaskStartedTime", (CurrentEpochTime() - ((int)CurrentEpochTime() % taskEveryXSeconds)));
            } else {
                print((CurrentEpochTime() - ((int)CurrentEpochTime() % taskEveryXSeconds)));
                loadTask();
            }
            
            
            TaskMenuUnclickable.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
        } else {
            TaskMenuUnclickable.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
            TaskMenuUnclickable.SetActive(false);
        }
    }

    public void OpenLevelMenu(int levelNo = 1) { // to open the level play menu from the buttons
            selectedLevelNo = levelNo;
            LevelMenuUnclickable.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Level " + levelNo; 
            LevelMenuUnclickable.SetActive(true);
            LevelMenuUnclickable.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
    }

    public void CloseLevelMenu() {
            LevelMenuUnclickable.SetActive(false); // to close the level play menu from the buttons
            LevelMenuUnclickable.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
    }

    public void onPlay() {
        GameObject levelInfo = new GameObject();
        LevelInfoPasser lip = levelInfo.AddComponent<LevelInfoPasser>();
        lip.level = selectedLevelNo;
        DontDestroyOnLoad(levelInfo);
        SceneManager.LoadScene(1);
    }

    public static int CurrentEpochTime()
     {
         System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
         int current = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
 
         return current;
     }

    public string initializeTask() { // reset task records and select a new one
        print("a: " + PlayerPrefs.GetInt("lastRewardGotTime"));
        print("a: " + PlayerPrefs.GetInt("lastTaskStartedTime"));
        
            
        
            PlayerPrefs.SetInt("sMeteoritesExploded",0);
            PlayerPrefs.SetInt("sMeteoritesHit",0);
            PlayerPrefs.SetInt("sGoldEarned",0);
        
            int randomSelectedTask = Random.Range(0,tasksList.Count);
            Tasks selectedTask = tasksList[randomSelectedTask];

            int taskCountRandom = Random.Range(0, selectedTask.possibleTaskNumbers.Length); // select tasks randomly from SO's
            int taskCount = selectedTask.possibleTaskNumbers[taskCountRandom];
            int taskRewardRandom = Random.Range(0, selectedTask.possibleTaskRewards.Length);
            int taskReward = selectedTask.possibleTaskRewards[taskRewardRandom];
            switch(selectedTask.taskType) {
                case taskType.BreakMeteorite:
                    taskText.text = "Destroy "+taskCount+" meteorites.";
                    break;
                case taskType.Hits: 
                    taskText.text = "Hit meteorites "+taskCount+" times.";
                    break;
                case taskType.EarnGold: 
                    taskText.text = "Earn "+taskCount+" gold";
                break;
            }
            rewardText.text = taskReward.ToString();

            return randomSelectedTask.ToString("D2") + taskCountRandom.ToString("D2") + taskRewardRandom.ToString("D2"); 
            
            timeLeftText.text = (PlayerPrefs.GetInt("lastTaskStartedTime") + taskEveryXSeconds - CurrentEpochTime()).ToString();

            PlayerPrefs.SetInt("lastTaskStartedTime", CurrentEpochTime() - ((int)CurrentEpochTime() % taskEveryXSeconds));


            

        return "000000";
    }

    public void loadTask() {
        print("a: "  + PlayerPrefs.GetInt("lastRewardGotTime"));
        print("a: " + PlayerPrefs.GetInt("lastTaskStartedTime"));
        if(PlayerPrefs.GetInt("lastRewardGotTime") == CurrentEpochTime() - ((int)CurrentEpochTime() % taskEveryXSeconds)) {
            taskBlock.SetActive(false);
        }
        if(PlayerPrefs.GetString("currentTask") != "notset") {

        string save = PlayerPrefs.GetString("currentTask");
        print(save);
        string selectedTask = save.Substring(0, 2);
        int taskCount = tasksList[int.Parse(selectedTask)].possibleTaskNumbers[int.Parse(save.Substring(2, 2))];
        int taskReward = tasksList[int.Parse(selectedTask)].possibleTaskRewards[int.Parse(save.Substring(4, 2))];
        rewardText.text = taskReward.ToString();
        switch(selectedTask) {
            case "00":
                taskText.text = "Destroy "+taskCount+" meteorites.\n" + PlayerPrefs.GetInt("sMeteoritesExploded") + "/"+taskCount;
                if(PlayerPrefs.GetInt("sMeteoritesExploded") >= taskCount) {
                    collectButton.interactable = true;
                }
                break;
            case "01": 
                taskText.text = "Hit meteorites "+taskCount+" times.\n" + PlayerPrefs.GetInt("sMeteoritesHit") + "/"+taskCount;
                if(PlayerPrefs.GetInt("sMeteoritesHit") >= taskCount) {
                    collectButton.interactable = true;
                }
                break;
            case "02": 
                taskText.text = "Earn "+taskCount+" gold\n" + PlayerPrefs.GetInt("sGoldEarned") + "/"+taskCount;
                if(PlayerPrefs.GetInt("sGoldEarned") >= taskCount) {
                    collectButton.interactable = true;
                }
            break;
        }
        print(PlayerPrefs.GetString("currentTask"));

        timeLeftText.text = (PlayerPrefs.GetInt("lastTaskStartedTime") + taskEveryXSeconds - CurrentEpochTime()).ToString();
        } else {

        }
    }

    public void getReward() {
        collectButton.interactable = false;
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold")+int.Parse(rewardText.text));
        ToggleTaskMenu(false);
        PlayerPrefs.SetInt("lastRewardGotTime",  CurrentEpochTime() - ((int)CurrentEpochTime() % taskEveryXSeconds));
    }

}
