using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public static class EventBusManager
    {
        public static event UnityAction<bool> OnChangeLightState;
        public static void RaiseLightChange(bool state)
        {
            OnChangeLightState?.Invoke(state);
        }
        
    }
}
