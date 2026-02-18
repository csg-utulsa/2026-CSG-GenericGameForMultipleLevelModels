using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    //adds a new interface option into the editor when you right click, can now create new weapon
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        //component that will override the players animations into the animations of the weapon class
        [SerializeField] Weapon equippedPrefab = null;
        //prefab for equipped weapon
        [SerializeField] float weaponRange = 2f;
        //sets the players weapon range, or the distance away from the enemy that the player stops to attack.
        [SerializeField] float weaponDamage = 5f;
        //sets the damage of player attacks, will be replaced by weapon properties later
        [SerializeField] float weaponDamagePercentageBonus = 5f;
        //sets the percent modifier of the weapon damage stat
        [SerializeField] bool isRightHanded = true;
        //set if the weapon is left or right handed
        [SerializeField] Projectile projectile = null;
        //sets the weapons projectile

        const string weaponName = "Weapon";
        //sets a constant string name to make refrence easy and consistant

        public Weapon Spawn(Transform rightHand,Transform leftHand, Animator animator)
        {
            DesroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;
            //defaults weapon to null for return
            if(equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                //sets the equipped weapon at the set hand transform
                weapon.gameObject.name = weaponName;
                //sets the name component of the weapon
            }

            var overideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            //casts the controller as an override controller, if it is the default controller it will not have
            //a parent refrence and return null
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
                //changes the animator controller to the animatorOverride controller
            }
            else if (overideController != null)
            {
                animator.runtimeAnimatorController = overideController.runtimeAnimatorController;
                //sets the animator controller to the overrides parent controller (default player controller)
                //(runtimeAnimatorController is the parent controller)
            }

            return weapon;
            //returns the equipped weapon

        }

        private void DesroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            //searches the right hand for the weapon name
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
                //not in the right hand, check the left
            }
            if (oldWeapon == null) { return; }
            //no weapon in either hand

            oldWeapon.name = "DESTROYING";
            //prevents confusion between picked up weapon and old weapon, prevents potential buggs
            Destroy(oldWeapon.gameObject);
            //if the weapon was found, destroy it
        }

        public bool HasProjectile()
        {
            return projectile != null;
            //lets us know if we have a projectile equipped
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            //creates a projectile from the proper hand and rotation
            projectileInstance.SetTarget(target,instigator,calculatedDamage);
            //sets the target for the projectile and its damage
        }
        
        public float GetRange()
        {
            return weaponRange;
        }
        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetDamagePercentageBonus()
        {
            return weaponDamagePercentageBonus;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) { handTransform = rightHand; }
            else { handTransform = leftHand; }
            //sets if the weapon needs to spawn in the left or right hand
            return handTransform;
        }
    }
}
