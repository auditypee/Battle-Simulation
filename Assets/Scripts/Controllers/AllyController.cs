using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Actors;

namespace Controllers
{
    public class AllyController : Controller
    {
        public Ally Ally;

        protected override void Start()
        {

            base.Start();
        }

        protected override void Update()
        {
            
        }

        public override void TargetSelected(GameObject target)
        {
            
        }

        protected override void CreateHealthBar()
        {
            
        }

        protected override void EngageTarget(GameObject target)
        {
            
        }

        protected override IEnumerator TimeForAction()
        {
            return null;
        }

        
    }
}
