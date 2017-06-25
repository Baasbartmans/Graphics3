using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using template_P3;
using OpenTK.Input;
using System;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{

    class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh mesh, floor, eendM, skyBoxM;        // a mesh to draw using OpenGL
        const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Shader postproc;                        // shader to use for post processing
        Texture wood, eend, skyBox;              // texture to use for rendering
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        bool useRenderTarget = true;

        SceneGraph graph = new SceneGraph();    // add the scenegraph which will contain all objects
        Camera cam = new Camera(new Vector3(0, 0, 0));
        float moveSpeed = 0.5f;
        float camSpeed = 0.2f;

        float oldMouseX = 0;
        float newMouseX = 0;
        float oldMouseY = 0;
        float newMouseY = 0;



        // initialize
        public void Init()
        {
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            eend = new Texture("../../assets/eend texture (1).jpg");
            skyBox = new Texture("../../assets/skybox2.jpg");


            //load transform
            Matrix4 transform = Matrix4.CreateTranslation(new Vector3(0, 9, -8));//Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);

            Matrix4 transformEend = Matrix4.CreateTranslation(new Vector3(0, 11.2f, -8));//Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);

            Matrix4 skyBoxTrans = Matrix4.CreateTranslation(Vector3.Zero);


            //Matrix4 transformEend = Matrix4.Identity;
            //transformEend *= Matrix4.CreateTranslation(0, 0, -9);

            // load teapot
            mesh = new Mesh("../../assets/teapot.obj", wood, transform);
            floor = new Mesh("../../assets/floor.obj", wood, transform);
            eendM = new Mesh("../../assets/4Voet.obj", eend, transformEend);
            skyBoxM = new Mesh("../../assets/SBB.obj", skyBox, skyBoxTrans);

            //fill the scenegraph
            //graph.master = new Node(floor);
            //graph.Add(mesh);         
            
            graph.Add(floor);
            graph.Add(skyBoxM);
            graph.master.children[0].Add(new Node(eendM));//Add(eendM);

            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");

            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);

            oldMouseX = newMouseX;
            newMouseX = Mouse.GetState().X;

            oldMouseY = newMouseY;
            newMouseY = Mouse.GetState().Y;


            if (Keyboard.GetState().IsKeyDown(Key.A)) cam.camPos *= Matrix4.CreateTranslation(new Vector3(moveSpeed, 0, 0));
            if (Keyboard.GetState().IsKeyDown(Key.D)) cam.camPos *= Matrix4.CreateTranslation(new Vector3(-1 * moveSpeed, 0, 0));
            if (Keyboard.GetState().IsKeyDown(Key.W)) cam.camPos *= Matrix4.CreateTranslation(new Vector3(0, 0, moveSpeed));
            if (Keyboard.GetState().IsKeyDown(Key.S)) cam.camPos *= Matrix4.CreateTranslation(new Vector3(0, 0, -1 * moveSpeed));

            if (newMouseX != oldMouseX)//Y rotation
                cam.camPos *= Matrix4.CreateRotationY(camSpeed * (newMouseX - oldMouseX) * 0.01f );
            if (newMouseY != oldMouseY)//X rotation
                cam.camPos *=  Matrix4.CreateRotationX(camSpeed * (newMouseY - oldMouseY) * 0.01f );

        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // prepare matrix for vertex shader
            Matrix4 transform = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0.01f);

            graph.master.children[0].Rotate(transform);
            //graph.master.children[2].rotate = graph.master.children[2].rotate * transform;

            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                // render scene to render target
                //mesh.Render(shader, transform, wood);
                //floor.Render(shader, transform, wood);

                //render the scenegraph
                graph.Render(shader, cam);

                // render quad
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                // render scene directly to the screen
                mesh.Render(shader, transform, wood);
                floor.Render(shader, transform, wood);
            }
        }
    }

} // namespace Template_P3