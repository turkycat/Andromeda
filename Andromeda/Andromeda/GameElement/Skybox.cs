using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GregsCameraClass;

namespace Andromeda.GameElement
{
    class Skybox : RenderableElement
    {
        private Ship ship;

        public override Matrix World
        {
            get
            {
                Vector3 camPosition = ConversionHelper.MathConverter.Convert( ship.Entity.Position );
                return Matrix.CreateScale( 6000f ) * Matrix.CreateTranslation( new Vector3( camPosition.X, camPosition.Y, camPosition.Z ) );
                //return Matrix.CreateScale( 6000f ) * Matrix.CreateTranslation( new Vector3( camera.View.M14, camera.View.M24, camera.View.M34 ) );
            }
        }

        //public Skybox( Game game, Camera camera ) : base( game, "skybox" )
        //{
        //    this.camera = camera;
        //}

        public Skybox( Game game, Ship ship ) : base( game, "skybox" )
        {
            this.ship = ship;
        }

    }
}
