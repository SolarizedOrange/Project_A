using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ObserverBool
{
	[SerializeField]  bool value;
	UnityEvent<bool> onValueChanged;

	public bool Value
	{
		get => value;
		set
		{
			if (this.value != value)
			{
				this.value = value;
				onValueChanged?.Invoke(this.value);
			}
		}
	}

	public ObserverBool(ObserverBool serializedData = null)
	{
		value = serializedData != null ? serializedData.Value : false;
		onValueChanged = new ();
	}
	public void AddListenerWrapper(UnityAction<bool> call, bool initCall = true)
	{
		if (initCall) call.Invoke(Value);
		onValueChanged.AddListener(call);
	}

	public void RemoveListenerWrapper(UnityAction<bool> call)
	{
		onValueChanged.RemoveListener(call);
	}
}

[Serializable]
public class ObserverInt
{
	[SerializeField] int value;
	UnityEvent<int> onValueChanged;

	public int Value
	{
		get => value;
		set
		{
			if (this.value != value)
			{
				this.value = value < 0? 0: value;
				onValueChanged?.Invoke(this.value);
			}
		}
	}

	public ObserverInt(ObserverInt serializedData = null)
	{
		value = serializedData != null ? serializedData.Value : 0;
		onValueChanged = new();
	}

	public void AddListenerWrapper(UnityAction<int> call, bool initCall = true)
	{
		if (initCall) call.Invoke(Value);
		onValueChanged.AddListener(call);
	}

	public void RemoveListenerWrapper(UnityAction<int> call)
	{
		onValueChanged.RemoveListener(call);
	}
}

[Serializable]
public class ObserverFloat
{
	[SerializeField] float value;
	public UnityEvent<float> onValueChanged;

	public float Value
	{
		get => value;
		set
		{
			if (this.value != value)
			{
				this.value = value < 0? 0f: value;
				onValueChanged?.Invoke(this.value);
			}
		}
	}

	public ObserverFloat(ObserverFloat serializedData = null)
	{
		value = serializedData != null ? serializedData.Value : 0f;
		onValueChanged = new();
	}

	public void AddListenerWrapper(UnityAction<float> call, bool initCall = true)
	{
		if (initCall) call.Invoke(Value);
		onValueChanged.AddListener(call);
	}

	public void RemoveListenerWrapper(UnityAction<float> call)
	{
		onValueChanged.RemoveListener(call);
	}
}