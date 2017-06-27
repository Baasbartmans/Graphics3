using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template_P3
{
    class Camera
    {
        public Matrix4 camPos;
        public Matrix4 camTrans;
        public Matrix4 totalRotation;

        public Camera(Vector3 camPos)
        {
            camTrans = Matrix4.Identity;
            this.camPos = new Matrix4(new Vector4(1, 0, 0, camPos.X), new Vector4(0, 1, 0, camPos.Y), new Vector4(0, 0, 1, camPos.Y), new Vector4(0, 0, 0, 1));
            this.totalRotation = this.camPos;
        }
    }
}
