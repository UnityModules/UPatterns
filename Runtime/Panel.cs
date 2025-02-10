using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UPatterns
{
    public abstract class UPanel : MonoBehaviour
    {
        private static Dictionary<Type, UPanel> Panels = new();

        public static T GetInstance<T>(Transform parent = null) where T:UPanel
        {
            if (Panels.ContainsKey(typeof(T)))
            {
                var ins = Panels[typeof(T)] as T;
                ins.transform.SetParent(parent);
                return ins;
            }
            
            var instance = Instantiate(Resources.Load<GameObject>(typeof(T).Name)).GetComponent<T>();
            instance.Initialize();
            Panels.Add(typeof(T), instance);
            instance.transform.SetParent(parent);
            AddEventSystem();
            return instance;
        }

        protected static void RemoveInstance<T>() where T: UPanel
        {
            if (Panels.ContainsKey(typeof(T)))
                Panels.Remove(typeof(T));
        }

        public static UPanel CurrentPanel { private set; get; }
        public void ChangePanel()
        {
            if (CurrentPanel)
                CurrentPanel.Hide();
            CurrentPanel = this;
            CurrentPanel.Show();
        }

        public static void HideCurrentPanel()
        {
            if (CurrentPanel)
                CurrentPanel.Hide();
        }

        public bool IsShow { private set; get; }

        public abstract void Initialize();
        public virtual void Show() 
        {
            IsShow = true;
            gameObject.SetActive(true);
        }

        public virtual void Hide() 
        {
            IsShow = false;
            gameObject.SetActive(false);
        }
    
        private static EventSystem eventSystem;
        private static void AddEventSystem()
        {
            if (eventSystem ??= GameObject.FindObjectOfType<EventSystem>())
                return;

            (eventSystem = (new GameObject(nameof(EventSystem)))
                .AddComponent<EventSystem>())
                .gameObject.AddComponent<StandaloneInputModule>();
        }
    }

    public abstract class UPanel<T> : UPanel where T:UPanel
    {
        public static T Instance =>
            GetInstance<T>();

        public static T GetInstance(Transform parent = null) =>
             GetInstance<T>(parent);

        public virtual void OnDestroy() =>
            RemoveInstance<T>();
    }
}