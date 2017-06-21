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

        public void Render(Shader shader, Matrix4 camPos)
        {
            master.Render(shader, camPos);
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
        Matrix4 thisTransform = new Matrix4();
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

        public void Render(Shader shader, Matrix4 camPos)
        {
            foreach (Node n in children)
            {
                n.Render(shader, camPos);
            }
            if(mesh != null)
            mesh.Render(shader,camPos * thisTransform, mesh.texture);
        }


    }
}
