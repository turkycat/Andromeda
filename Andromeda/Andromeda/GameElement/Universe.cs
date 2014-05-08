using BEPUphysics.Entities.Prefabs;
using BEPUphysicsDemos;
using ConversionHelper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities;

namespace Andromeda.GameElement
{
    class Universe : ElementBase
    {
        private Sphere sphere;

        public override Entity Entity
        {
            get
            {
                return sphere;
            }
        }

        //public override Matrix World
        //{
        //    get
        //    {
        //        return MathConverter.Convert( mesh.WorldTransform );
        //    }
        //}

        public Universe( Game game )
        {
            sphere = new Sphere( BEPUutilities.Vector3.Zero, 2000f );
            sphere.CollisionInformation.CollisionRules.Group = Resources.Instance.GetGroup( "universe" );
            //mesh.CollisionInformation.
            sphere.BecomeKinematic();
        }
    }
}
