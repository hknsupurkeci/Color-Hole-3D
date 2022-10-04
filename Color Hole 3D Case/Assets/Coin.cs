using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

//SUBSCRIBE TO VIN CODES FOR MORE FREE SCRIPTS IN FUTURE VIDEOS :)

public class Coin : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 5f;
    CoinMove coinMoveScript;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("CoinDetector").transform;
        coinMoveScript = gameObject.GetComponent<CoinMove>();
        coinMoveScript.enabled = false;
    }

    void Update()
    {
        //Mıknatıs----Eğer hareket var ise miknatis kodu calissin-?
        //z eksen kontrolu
        if (Math.Abs(playerTransform.position.z - transform.position.z) < 1.5)
        {
            //x alan kontrol
            if (Math.Abs(playerTransform.position.x - transform.position.x) < 1.5) coinMoveScript.enabled = true;
            else coinMoveScript.enabled = false;
        }
        else coinMoveScript.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "KillCoins")
        {
            Vibrator.Vibrate();
            Destroy(gameObject);
        }
    }
}

public static class Vibrator
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject androidJavaObject;
    public static AndroidJavaObject vibrator;
#endif
    public static void Vibrate(long milliseconds = 100)
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            Handheld.Vibrate();
        }
    }
    public static void Cancel()
    {
        if (IsAndroid())
            vibrator.Call("cancel");
    }

    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
