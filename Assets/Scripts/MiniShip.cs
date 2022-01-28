using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MiniShip : MonoBehaviour
{
    [SerializeField] int IDOfShip;
    [SerializeField] GameManager gameManager;
    [SerializeField] AssetReference bulletAsset;

    [SerializeField] SpaceShip spaceShip;

    [SerializeField] float xDistance = 1;

    [SerializeField] float bulletDelay = 0.4f; // float değerlerinin sonunda f olmak zorundadır
    [SerializeField] float bulletSpeed = 0.4f; 
    private float timer = 0;
    bool isGameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("upgrade3") <= IDOfShip) {
            Destroy(gameObject);
        }
        bulletDelay = 0.4f - PlayerPrefs.GetInt("upgrade1") * 0.012f;
        bulletSpeed = 5f + PlayerPrefs.GetInt("upgrade1") * 0.2f;
    }



    // Update is called once per frame
    void Update()
    {  
        isGameStarted = spaceShip.isGameStarted;
        if(isGameStarted) {
            HandleMovement();
            SpawnBullet();
        }   
    }

    void HandleMovement() {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButton(0)) {
            transform.position = new Vector2(mouseWorldPos.x + (xDistance) * Mathf.Pow(-1,IDOfShip) * Mathf.Clamp((IDOfShip-1),1,2), transform.position.y) ;
        }
    }

    void SpawnBullet() {
        timer += Time.deltaTime;
        if(Input.GetMouseButton(0)) {
            if(timer >= bulletDelay) {
                timer = 0;
                //instantiate bullet with adressable
                var op = Addressables.LoadAssetAsync<GameObject>(bulletAsset); // load the bulletAsset prefab
                op.Completed += (operation) => // when the operation is completed instantiate the loaded asset
                { 
                instantiateBullet(bulletAsset,transform.position, PlayerPrefs.GetInt("upgrade2"));
                //
                };
            }
        }
    }

    void instantiateBullet(AssetReference asset, Vector3 pos, int bulletDamage) {
        asset.InstantiateAsync(pos,Quaternion.Euler(0,0,90)).Completed += (asyncOperationHandle) =>
        {
            GameObject ins = asyncOperationHandle.Result;
            ins.GetComponent<BulletScript>().operation = asyncOperationHandle;
            ins.GetComponent<BulletScript>().bulletSpeed = bulletSpeed;
            ins.GetComponent<BulletScript>().bulletDamage = bulletDamage;
        };
    }


}
