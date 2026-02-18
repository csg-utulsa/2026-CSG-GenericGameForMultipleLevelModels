using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;
        //local refrence to the damage text

        public void Spawn(float damageAmmount)
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
            //spawns the damage text prefab with the desired damage amount
            instance.SetValue(damageAmmount);
            //passes the damage amount to the DamageText so it can set the proper text
        }
    }
}
