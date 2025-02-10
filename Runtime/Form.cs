using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UPatterns
{
    public class UForm
    {
        private UFormField[] fields { init; get; }
        private Button submitBtn { init; get; }
        private int activeFieldIndex = 0;
        public UFormField ActiveField => fields[activeFieldIndex];
        
        public UForm(UFormField[] fields,Button submitBtn = null)
        {
            (this.fields = fields).ForEach(field => field.AddOnChanged(()=> Validate()));
            this.submitBtn = submitBtn;
            Reset();
        }

        public void SetOnSubmit(Action<KeyValuePair<string,object>[]> onSubmit)
        {
            submitBtn.onClick.RemoveAllListeners();
            submitBtn.onClick.AddListener(() => onSubmit?.Invoke(GetValues()));
        }

        public KeyValuePair<string,object>[] GetValues()
        {
            var lst = new List<KeyValuePair<string,object>>();
            fields.ForEach(field => lst.Add(field.KeyValue));
            return lst.ToArray();
        }
        public string ToJson() => GetValues().ToJson();
        
        public virtual void Reset()
        {
            activeFieldIndex = 0;
            fields.ForEach(field => { field.Reset(); field.UnFocus(); });
            ActiveField.Focus();
            if(submitBtn) submitBtn.interactable = false;
        }

        public virtual bool Validate()
        {
            for (int i = 0; i < fields.Length; i++)
                if (!fields[i].Validate())
                {
                    if(submitBtn) submitBtn.interactable = false;
                    return false;
                }

            if(submitBtn) submitBtn.interactable = true;
            return true;
        }
        public virtual void NextField() => ChangeFocusField((activeFieldIndex + 1) % fields.Length);
        public virtual void PreviousField() => ChangeFocusField((activeFieldIndex + fields.Length - 1) % fields.Length);
        public virtual void ChangeFocusField(int index)
        {
            ActiveField.UnFocus();
            activeFieldIndex = index;
            ActiveField.Focus();
        }
        
        public virtual void KeyboardInput()
        { 
            if((Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Tab)) || Input.GetKeyDown(KeyCode.LeftArrow)) 
                PreviousField();
            else
                if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.RightArrow)) 
                    NextField();

            if (Input.GetKeyDown(KeyCode.Return) && submitBtn && Validate())
                submitBtn.onClick.Invoke();
        }
    }
}