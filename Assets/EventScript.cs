using UnityEngine;
using UnityEngine.Events;

public class EventScript : MonoBehaviour
{
    [SerializeField]UnityEvent eventToTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TriggerEvent()
    {
        if (eventToTrigger != null) 
            eventToTrigger.Invoke();
    }
}
