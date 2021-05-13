using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    [SerializeField] bool dontDestroy = false;
    private static T instance;
    public static T Instance 
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);
                if (instance == null) Debug.LogError(t + "をアタッチしているオブジェクトがありません");
            }
            return instance;
        }
    }

    virtual protected void Awake()
    {
        if(this != Instance)
        {
            Debug.Log("同一オブジェクト存在するので削除します" + Instance.gameObject.name);
            Destroy(this.gameObject);
            return;
        }
        if (dontDestroy) DontDestroyOnLoad(this.gameObject);
    }
}
