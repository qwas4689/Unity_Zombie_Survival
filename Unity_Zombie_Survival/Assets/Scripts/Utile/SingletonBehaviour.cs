using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // template <typename T>
public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    // where 는 T에 대한 컴포넌트 제약을 걸어주는것 제네릭에 사용하는것이다.
    // MonoBehaviour를 상속받는 것이여야만 한다.
{
    private static T _instance;

    public static T Instance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance; 
        } 
    }

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }

            return;
            // 유일성 보장
        }
        _instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
    
}
