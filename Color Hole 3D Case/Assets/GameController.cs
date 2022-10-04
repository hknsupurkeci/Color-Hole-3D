using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Slider sliderFir, sliderSec;
    public GameObject NextLevelObj;
    public Text scoreTxt, currentLevelTxt, nextLevelTxt;
    public List<GameObject> ActiveLevelSteps = new List<GameObject>();
    public Vector3 randomPos;
    public float moveSpeed;
    public static int Score=0, levelId = 0;
    public static bool flag = false;

    GameObject player;
    GameObject door;
    GameObject camera;
    
    Vector3 offset;
    Vector3 offset2;

    int enemyCountSec = 0, enemyCountFir = 0, step1 = 0, step2 = 0;

    void Start()
    {
        flag = false;
        
        currentLevelTxt.text = (levelId + 1).ToString();
        nextLevelTxt.text = (levelId + 2).ToString();
        scoreTxt.text = Score.ToString();
        //Prefab içindeki elementleri aliyorum
        var coins = ActiveLevelSteps[0].transform.GetComponentsInChildren<Transform>();
        foreach (var item in coins)
        {
            if (item.tag != "Enemy") step1++;
            else enemyCountFir++;
        }
        sliderFir.minValue = 0;
        sliderFir.maxValue = step1;
        var coins2 = ActiveLevelSteps[1].transform.GetComponentsInChildren<Transform>();
        foreach (var item in coins2)
        {
            if (item.tag != "Enemy") step2++;
            else enemyCountSec++;
        }
        sliderSec.minValue = 0;
        sliderSec.maxValue = step2;
 
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        door = GameObject.FindGameObjectWithTag("Door");

        //player.transform.position = new Vector3(0, -0.49f, -36.4f);
        offset = camera.transform.position - player.transform.position;
        
    }

    private void Update()
    {
        #region Kinematic False
        var coinsGravitySet = ActiveLevelSteps[0].transform.GetComponentsInChildren<Transform>();

        foreach (var item in coinsGravitySet)
        {
            //max valueden azalan valueyi çıkartıyorum ve engel sayısını ekliyorum
            sliderFir.value = sliderFir.maxValue - coinsGravitySet.Length + enemyCountFir;
            if (Math.Abs(player.transform.position.z - item.transform.position.z) < 1.2)
            {
                //alana yaklaştiginda kinematicigi kapatiyorum, bu sayade gravity etki etmiyor ve devrilme sorunu yasanmiyor
                if (Math.Abs(player.transform.position.x - item.transform.position.x) < 1.2)
                {
                    item.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }

        var coinsGravitySet2 = ActiveLevelSteps[1].transform.GetComponentsInChildren<Transform>();

        foreach (var item in coinsGravitySet2)
        {
            //max valueden azalan valueyi ve çıkartıyorum. ve engel sayısını ekliyorum
            sliderSec.value = sliderSec.maxValue - coinsGravitySet2.Length + enemyCountSec;
            if (Math.Abs(player.transform.position.z - item.transform.position.z) < 1.2)
            {
                //alana yaklaştiginda kinematicigi kapatiyorum, bu sayade gravity etki etmiyor ve devrilme sorunu yasanmiyor
                if (Math.Abs(player.transform.position.x - item.transform.position.x) < 1.2)
                {
                    item.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
        #endregion
        //Debug.Log(levelId);
        #region Step One
        if (ActiveLevelSteps[0].transform.GetComponentsInChildren<Transform>().Length-1 <= enemyCountFir && !flag)
        {
            OnChangePosition.gameOver = false;
            //Birinci aşama 0'x eksenine ilerliyor
            if (player.transform.position.x != 0)
            {
                MoveX();
            }
            else
            {
                OnChangePosition.maxZ = 37.5f;
                moveSpeed = 15;
                DoorClose();
                MoveZ();
                CameraInf();
                NextStep();
            }
        }
        #endregion
        #region Step Two
        else if (ActiveLevelSteps[1].transform.GetComponentsInChildren<Transform>().Length - 1 <= enemyCountSec && flag)
        {
            EnemyEnabled(false, true);
            NextLevelObj.SetActive(true);
            OnChangePosition.gameOver = false;
        }
        #endregion
    }
    public void CameraInf()
    {
        //aradaki farkı kapatmak için sürekli kamerayı yaklaştırıyorum
        if (Mathf.Abs(offset2.z) >= Mathf.Abs(offset.z))
        {
            offset2.z += 0.2f;
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, player.transform.position.z + offset2.z);
        }
        else
        {
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, player.transform.position.z + offset.z);
        }
    }

    public void DoorClose()
    {
        if (door.transform.localScale.y > 0)
        {
            door.transform.localScale = new Vector3(door.transform.localScale.x, door.transform.localScale.y - 0.2f, door.transform.localScale.z);
            if (door.transform.position.y > 0)
            {
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.1f, door.transform.position.z);

            }
        }
        else
        {
            door.transform.localScale = new Vector3(0, 0, 0);
        }
    }
    public void NextStep()
    {
        if (player.transform.position.z == 16.5f)
        {
            OnChangePosition.gameOver = true;
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, player.transform.position.z + offset.z);

            flag = true;
            OnChangePosition.maxZ = 37.31f;
            OnChangePosition.minZ = 16.26f;
            //enemyleri açıyoruz geri
            EnemyEnabled(true, false);
        }
    }
    public void MoveZ()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, 16.5f),
                    moveSpeed * Time.deltaTime);
    }

    public void MoveX()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(0.0f, player.transform.position.y, player.transform.position.z),
                moveSpeed * Time.deltaTime);
        offset2 = camera.transform.position - player.transform.position;

        EnemyEnabled(false,true);
    }
    public void EnemyEnabled(bool gravity, bool kinematic)
    {
        //gravity = false ise kapatma
        var enemyNext = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in enemyNext)
        {
            item.GetComponent<Rigidbody>().useGravity = gravity;
            item.GetComponent<Rigidbody>().isKinematic = kinematic;

        }
    }
    public void NextLevel()
    {
        levelId++;
        Score += 100;
        scoreTxt.text = Score.ToString();
        SceneManager.LoadScene("Level "+ levelId);//or active Sceene
        NextLevelObj.SetActive(false);
    }
}
