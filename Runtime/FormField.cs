using System;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UPatterns
{
    public record UFormField(string Key,Func<bool> Validate = null)
    {
        public virtual KeyValuePair<string, object> KeyValue => new();
        public virtual void Interactable(bool value) {}
        public virtual void Reset() {}
        public virtual void UnFocus() {}
        public virtual void Focus() {} 
        public virtual void AddOnChanged(UnityAction action) {}
    }
    public record UFormField<T,TValue>(T field,string Key,Func<bool> Validate) : UFormField(Key,Validate)
    {
        public virtual TValue Value => default(TValue);
        public override KeyValuePair<string, object> KeyValue => new (Key, Value);
        public virtual void AddOnChanged(UnityAction<TValue> action) {}

    }
    
    // Unity UI Components
    public record UFormField_TMP_InputField(TMP_InputField Field,string Key, Func<bool> Validate) : UFormField<TMP_InputField,string>(Field,Key,Validate)
    {
        public override string Value => Field.text;
        public override void Interactable(bool value) => Field.interactable = value;
        public override void Focus() => Field.Select();
        public override void UnFocus() => Field.DeactivateInputField();

        public override void AddOnChanged(UnityAction action) => Field.onValueChanged.AddListener(value => action?.Invoke());
        public override void AddOnChanged(UnityAction<string> action) => Field.onValueChanged.AddListener(action);

        public override void Reset()
        {
            base.Reset();
            Field.text = "";
        }
    }
    public record UFormField_TMP_DropDown(TMP_Dropdown Field,string Key, Func<bool> Validate) : UFormField<TMP_Dropdown,int>(Field,Key,Validate)
    {
        public override int Value => Field.value;
        public override void Interactable(bool value) => Field.interactable = value;
        public override void Focus() => Field.Select();

        public override void AddOnChanged(UnityAction action) => Field.onValueChanged.AddListener(value => action?.Invoke());
        public override void AddOnChanged(UnityAction<int> action) => Field.onValueChanged.AddListener(action);

        public override void Reset()
        {
            base.Reset();
            Field.value = 0;
        }
    }
    public record UFormField_Toggle(Toggle Field,string Key, Func<bool> Validate) : UFormField<Toggle,bool>(Field,Key,Validate)
    {
        public override bool Value => Field.isOn;
        public override void Interactable(bool value) => Field.interactable = value;
        public override void Focus() => Field.Select();

        public override void AddOnChanged(UnityAction action) => Field.onValueChanged.AddListener(value => action?.Invoke());
        public override void AddOnChanged(UnityAction<bool> action) => Field.onValueChanged.AddListener(action);

        public override void Reset()
        {
            base.Reset();
            Field.isOn = false;
        }
    }
}