using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class IndependentMeteorite : MonoBehaviour
{
    public int health;
    public int scaleMultiplier;
    public UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle operation;
    [SerializeField] Text healthText;
    GameManager gManager;
    // Start is called before the first frame update
    void Start()
    {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        if(health<= 0) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold")+5);
            PlayerPrefs.SetInt("sGoldEarned",PlayerPrefs.GetInt("sGoldEarned")+5);
            SingletonAudioManager.instance.playAudio(2);
            gManager.meteoritesExploded++;
            PlayerPrefs.SetInt("sMeteoritesExploded", PlayerPrefs.GetInt("sMeteoritesExploded")+1);
            Addressables.ReleaseInstance(operation);
        }
    }

    public void dropHealth(int a) {
        PlayerPrefs.SetInt("sMeteoritesHit", PlayerPrefs.GetInt("sMeteoritesHit")+1);
        gManager.meteoritesHit++;
        health -= a;
    }
}
