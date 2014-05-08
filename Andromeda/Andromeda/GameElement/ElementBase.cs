using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GregsCameraClass;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;

namespace Andromeda.GameElement
{
    public abstract class ElementBase : ITargetable
    {
        public virtual Matrix World
        {
            get;
            protected set;
        }

        public virtual Entity Entity
        {
            get;
            protected set;
        }
    }
}
