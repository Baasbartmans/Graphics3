﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template_P3;

namespace template_P3
{
    class SceneGraph
    {
        public Node master = new Node(null);

        public void Render(Shader shader, Camera cam)
        {
            master.Render(shader, cam);
        }

        public void Add(Mesh m)
        {
            Node child = new Node(m);
            master.children.Add(child);
        }
    }

    class Node
    {
        public List<Node> children = new List<Node>();
        Matrix4 parentTransform = Matrix4.CreateTranslation(new Vector3(0,0,0));
        public Matrix4 thisTransform = Matrix4.CreateTranslation(new Vector3(0,0,0));
        public Matrix4 meshTransform = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
        public Matrix4 rotate = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
        public Matrix4 parentRot = Matrix4.CreateTranslation(new Vector3(0,0,0));
        Mesh mesh;

        public Node(Mesh mesh)
        {
            if (mesh != null)
            {
                thisTransform = parentTransform * mesh.transform;
                meshTransform = mesh.transform;
                this.mesh = mesh;
            }
            else
            {
                thisTransform = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            }
        }

        public void Rotate(Matrix4 r)
        {
            rotate = rotate * r;
            ChildRotate(r);
        }

        public void ChildRotate(Matrix4 r)
        {
            foreach (Node n in children)
            {
                n.parentRot *= r;
                n.ChildRotate(r);
            }
        }

        public void Add(Node child)
        {
            child.parentTransform = thisTransform;
            children.Add(child);
        }

        public void Render(Shader shader, Camera cam)
        {
            foreach (Node n in children)
            {
                n.Render(shader, cam);
            }
            Matrix4 myMatrix = new Matrix4(new Vector4(1, 0, 0, cam.camPos.Column0.W), new Vector4(0, 1, 0, cam.camPos.Column1.W), new Vector4(0, 0, 1, cam.camPos.Column2.W), new Vector4(0, 0, 0, 1));
            Matrix4 newPos = myMatrix * cam.camPos;
            Vector3 camPos = new Vector3(newPos.Column0.W, newPos.Column1.W, newPos.Column2.W);
            Console.WriteLine(newPos);
            if (mesh != null)
            mesh.Render(shader,Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 10000), mesh.texture, rotate * parentTransform * meshTransform * parentRot * cam.camPos, camPos);
        }


    }
}
