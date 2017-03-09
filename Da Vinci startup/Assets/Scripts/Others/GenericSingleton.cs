using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        private set;
        get;
    }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}