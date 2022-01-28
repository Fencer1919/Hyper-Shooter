using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAudioManager : MonoBehaviour
{
    public static SingletonAudioManager instance;

    private AudioSource audioSource;

    [SerializeField] private SFX[] sfxs;

    [System.Serializable]
    private class SFX {
        public AudioClip[] audioClips;
        public float[] volumeClip;
    }


    void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void playAudio(int audioIndex) {
        audioSource.volume = sfxs[0].volumeClip[audioIndex];
        audioSource.pitch = Random.Range(0.8f,1.2f);
        audioSource.PlayOneShot(sfxs[0].audioClips[audioIndex]);
    }
    
}