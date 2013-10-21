using UnityEngine;
using System.Collections;

public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>
{
    private static T sInstance = null;
    public static T instance {
        get { return sInstance; }
    }

    protected virtual void Awake() {
        if (sInstance != null)
            Debug.LogError(name + ": error: already initialized", this);

        sInstance = (T)this;
    }
}