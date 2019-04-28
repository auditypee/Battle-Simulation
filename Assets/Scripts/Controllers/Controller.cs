using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Actors;

namespace Controllers
{
    public abstract class Controller : MonoBehaviour
    {
        // will make a reference to the main BattleManager
        protected BattleManager _bm;

        protected Vector2 _startPosition;
        protected static bool _actionStarted = false;
        protected static float _animSpeed = 30f;

        [SerializeField] protected GameObject HealthBar;
        protected HealthBar _changeHealthBar;

        // initiate fields
        protected virtual void Start()
        {
            _bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
            _startPosition = transform.position;

            CreateHealthBar();
        }

        protected abstract void CreateHealthBar();
        

        // update for the state machine
        protected abstract void Update();

        // handles which target is selected
        public abstract void TargetSelected(GameObject target);

        // determines how the object will move towards its target
        protected abstract IEnumerator TimeForAction();

        // movement of the object towards the target
        protected virtual bool MoveTowardsTarget(Vector3 target)
        {
            return target != (transform.position = Vector3.MoveTowards(transform.position, target, _animSpeed * Time.deltaTime));
        }

        // deals damage/something towards the target
        protected abstract void EngageTarget(GameObject target);

        // calculates the damage done, will tweak for balance changes
        protected virtual int CalculateDamage(Actor attacker, Actor defender)
        {
            int attackerATK = attacker.Attack;
            int defenderDEF = defender.Defense;
            int attackerLVL = attacker.Level;

            int damage = (((2 * attackerLVL) / 5 + 2) * (attackerATK / defenderDEF)) / 2 + 2;

            return damage;
        }

        // gameObject.SendMessage(Method) calls all the scripts attached to the gameObject with a Method, could use for AOE spells?
    }
}
