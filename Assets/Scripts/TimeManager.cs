using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : MonoBehaviour, ISaveable
{
    public static TimeManager Instance;

    [SerializeField] private int _timeOfDay, _daysPassed;
    [SerializeField] private int _lengthOfDay;
    private float _rotationIncrement;
    private bool _isDay;

    [SerializeField] private Transform _cosmos;
    [SerializeField] private Light _sun, _moon;

    private List<IGetTimeNotifications> _listeners;
    //private List<(int, IGetTimeNotifications)> _events;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        _rotationIncrement = 360f / _lengthOfDay;
        _listeners = FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                    .OfType<IGetTimeNotifications>()
                    .ToList();
    }

    private void SetDay(bool isDay)
    {
        _isDay = isDay;
        _moon.shadows = LightShadows.None;
        _sun.shadows = LightShadows.None;
        if (isDay)
            _sun.shadows = LightShadows.Hard;
        else
            _moon.shadows = LightShadows.Hard;
    }

    private void SetTimeOfDay(int time)
    {
        _timeOfDay = time;
        if (_timeOfDay > 0 && _timeOfDay <= (_lengthOfDay / 2) && !_isDay)
            SetDay(true);
        else if (_timeOfDay > (_lengthOfDay / 2) && _timeOfDay <= _lengthOfDay && _isDay)
            SetDay(false);
    }

    private void FixedUpdate()
    {
        SetTimeOfDay(_timeOfDay + 1);

        if (_listeners != null && _timeOfDay % 60 == 0)
            _listeners.ForEach(l => l.Notify(_timeOfDay / 60));

        if (_timeOfDay >= _lengthOfDay)
        {
            _daysPassed++;
            _timeOfDay = 0;
            _cosmos.rotation = Quaternion.Euler(0, _cosmos.rotation.y, _cosmos.rotation.z);
        }
        _cosmos.Rotate(Vector3.right, _rotationIncrement);
        //Cosmos.rotation = Quaternion.Euler(Cosmos.rotation.x + _rotationIncrement, Cosmos.rotation.y, Cosmos.rotation.z);
    }

    public void Save(SaveData data)
    {
        data.TimeOfDay = _timeOfDay;
        data.DaysPassed = _daysPassed;
    }

    public void Load(SaveData data)
    {
        SetDay(false);
        SetTimeOfDay(data.TimeOfDay);
        _daysPassed = data.DaysPassed;
        _cosmos.rotation = Quaternion.Euler(_timeOfDay * _rotationIncrement, _cosmos.rotation.y, _cosmos.rotation.z);
    }
}
