using OpenTK;
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
        Mesh mesh;

        public Node(Mesh mesh)
        {
            if (mesh != null)
            {
                thisTransform = parentTransform * mesh.transform;
                this.mesh = mesh;
            }
            else
            {
                thisTransform = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
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
            if(mesh != null)
            mesh.Render(shader, cam.camPos * thisTransform * Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000), mesh.texture);
        }


    }
}
