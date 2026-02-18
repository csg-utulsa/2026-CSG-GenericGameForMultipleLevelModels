using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        //unity event for when the weapon hits
        public void OnHit()
        {
            onHit.Invoke();
            //invokes the onHit unity event
        }
    }

}