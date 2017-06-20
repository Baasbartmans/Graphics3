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
        public Node master;

        public void Render(Shader shader)
        {
            master.Render(shader);
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
        Matrix4 parentTransform = new Matrix4();
        Matrix4 thisTransform = new Matrix4();
        Mesh mesh;

        public Node(Mesh mesh)
        {
            thisTransform = parentTransform + mesh.transform;
            this.mesh = mesh;
        }

        public void Add(Node child)
        {
            child.parentTransform = thisTransform;
            children.Add(child);
        }

        public void Render(Shader shader)
        {
            foreach (Node n in children)
            {
                n.Render(shader);
            }
            mesh.Render(shader, thisTransform, mesh.texture);
        }


    }
}
