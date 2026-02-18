using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1.0f;
        //projectile speed
        [SerializeField] bool isHoming = true;
        //if the projectile tracks a taret or not
        [SerializeField] float maxLifeTime = 10f;
        //how long a projectile will live in the scene
        [SerializeField] GameObject hitEffect = null;
        //if the game object has an effect to play on hit, attach it here
        [SerializeField] GameObject[] destroyOnHit = null;
        //list of game objects to destroy on projectile impact.
        [SerializeField] float lifeAfterImpact = 2f;
        //how long the non destroyOnHit components of a projectile live after impact.
        [SerializeField] UnityEvent onHit;
        //creates a unity event to be played on projectile hit


        Health target = null;
        //the target for the projectile
        GameObject instigator = null;
        //the gameObject that shot the projectile
        float damage = 0;
        //projectile damage

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            //look at the projectiles target
        }

        void Update()
        {
            if (target == null) { return; }
            //if no target, return
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
                //look at the projectiles target
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //moves the projectile on the z axis at a set speed over time

        }
        public void SetTarget(Health target,GameObject instigator, float damage)
        {
            this.target = target;
            //sets the projectiles target to the passed in target
            this.damage = damage;
            //sets the projectile damage to the passed in damage
            this.instigator = instigator;
            //sets the projectime instigator

            Destroy(gameObject, maxLifeTime);
            //destroy the projectile after the max life time
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
                //if no capsule collider return the targets default position
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
            //Vector3.up is short for writing Vector3(0,1,0), basically adding 1 on the y axis
            //we then add 1 * capsule height / 2 to find the midle of the capsule
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) { return; }
            //if the object hit is not our target, ignore
            if (target.IsDead()) { return; }
            //if the target is dead, dont deal damage and dont destroy the projectile
            target.TakeDamage(instigator, damage);
            //if it is the right target, deal damage

            speed = 0;
            //stops the projectile on impact

            onHit.Invoke();
            //invokes the onHit unity event

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                //spawn the hit effect at the impact location
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
                //destroys all impact destrucable components in the projectile
            }

            Destroy(gameObject, lifeAfterImpact);
            //destroy the shot projectile
        }
    }

}