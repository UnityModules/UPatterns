using UnityEngine;

namespace UPatterns
{
    public abstract class UMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance => 
            instance ??= GameObject.FindAnyObjectByType<T>() ?? new GameObject(typeof(T).Name).AddComponent<T>();
        public static void SetInstance(T value) =>
            instance = value;

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}