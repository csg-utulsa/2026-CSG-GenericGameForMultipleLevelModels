using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        //refrence to the objects health component
        [SerializeField] RectTransform foreground = null;
        //refrence to the foreground component
        [SerializeField] Canvas rootCanvas = null;
        //refrence to the parent (root) canvas

        void Update()
        {
            if (Mathf.Approximately(healthComponent.GetFraction(), 0) || Mathf.Approximately(healthComponent.GetFraction(), 1))
            //ensures that math done by different CPU's is close enough to 0 or 1
            {
                rootCanvas.enabled = false;
                return;
                //if health is 0 or full, disable healthbar
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthComponent.GetFraction(),1,1);
            //sets the new scaling factor for the foreground component based off of the fraction of health remaning.
            //(percent remaning just 0 to 1 not 0 to 100)
        }
    }
}
