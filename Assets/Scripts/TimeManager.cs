using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class TimeManager : MonoBehaviour, ISaveable
{
    public static TimeManager Instance;

    [SerializeField] private int _timeOfDay, _daysPassed;
    [SerializeField] private int LengthOfDay;
    private float _rotationIncrement;

    [SerializeField] private Transform Cosmos;

    private List<IGetTimeNotifications> _listeners;
    //private List<(int, IGetTimeNotifications)> _events;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        _rotationIncrement = 360f / LengthOfDay;
    }

    public void Subscribe(IGetTimeNotifications listener)
    {
        if (_listeners == null)
            _listeners = new List<IGetTimeNotifications>();
        _listeners.Add(listener);
    }

    private void FixedUpdate()
    {
        _timeOfDay++;
        if (_listeners != null && _timeOfDay % 60 == 0)
        {
            _listeners.ForEach(l => l.Notify(_timeOfDay / 60));
        }
        if (_timeOfDay >= LengthOfDay)
        {
            _daysPassed++;
            _timeOfDay = 0;
            Cosmos.rotation = Quaternion.Euler(0, Cosmos.rotation.y, Cosmos.rotation.z);
        }
        Cosmos.Rotate(Vector3.right, _rotationIncrement);
        //Cosmos.rotation = Quaternion.Euler(Cosmos.rotation.x + _rotationIncrement, Cosmos.rotation.y, Cosmos.rotation.z);
    }

    public void Save(SaveData data)
    {
        data.TimeOfDay = _timeOfDay;
        data.DaysPassed = _daysPassed;
    }

    public void Load(SaveData data)
    {
        _timeOfDay = data.TimeOfDay;
        _daysPassed = data.DaysPassed;
        Cosmos.rotation = Quaternion.Euler(_timeOfDay * _rotationIncrement, Cosmos.rotation.y, Cosmos.rotation.z);
    }
}
