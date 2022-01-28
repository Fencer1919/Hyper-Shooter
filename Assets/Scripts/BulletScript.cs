using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BulletScript : MonoBehaviour
{
    private float timer = 0;
    Rigidbody2D rb2D;
    public UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle operation;
    //public olmalarının nedeni spaceship'ten erişebilir olmalarıdır.
    public float bulletSpeed = 5;
    public float bulletScaleMultiplier = 1;
    public float bulletDamage = 1;
    

    void Start()
    {
        SingletonAudioManager.instance.playAudio(3);
        rb2D = GetComponent<Rigidbody2D>();
    }



    
    void Update()
    {
        
        rb2D.velocity = new Vector3(0, bulletSpeed, 0);

        timer += Time.deltaTime;
        if(timer > 3f) {
            Addressables.ReleaseInstance(operation);
            print(gameObject.name + ":" + " destroyed by time");
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Meteor"){
            if(other.gameObject.GetComponent<IndependentMeteorite>().health >= 1) {
                other.gameObject.GetComponent<IndependentMeteorite>().dropHealth((int)bulletDamage);
                Addressables.ReleaseInstance(operation);
                print(gameObject.name + ":" + " destroyed by meteorite");
            }
        }    
    }

}
