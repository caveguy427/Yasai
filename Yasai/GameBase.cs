using System;
using System.Drawing;
using ManagedBass;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Yasai.Debug.Logging;
using Yasai.Structures.DI;

namespace Yasai
{
    /// <summary>
    /// The bare essentials of a Yasai application
    /// </summary>
    public class GameBase : IDisposable
    {
        public readonly GameWindow Window;
        
        // make the field static for now, we'll worry about thread safety later
        public static Matrix4 Projection;

        public DependencyContainer Dependencies { get; }
        
        //protected Container Children;

        internal static readonly Logger YasaiLogger = new ("yasai.log");

        private Color bgCol = Color.CornflowerBlue;
        public Color BackgroundColour
        {
            get => bgCol;
            set
            {
                bgCol = value;
                GL.ClearColor(bgCol);
            }
        }

        public GameBase(string title, GameWindowSettings gameSettings, NativeWindowSettings nativeSettings, string[] args = null)
        {
            // initialise audio engine
            Bass.Init();
            
            // Window
            Window = new GameWindow(gameSettings, nativeSettings);
            Window.Title = title;

            // events
            Window.Load += () =>
            {
                Load(Dependencies);
                LoadComplete(Dependencies);
            };
            Window.Unload += () => Unload(Dependencies);
            Window.UpdateFrame += Update;
            Window.RenderFrame += draw;
            Window.Resize += Resize;
            Window.KeyDown += KeyDown;
            Window.KeyUp += KeyUp;
            Window.MouseMove += MouseMove;
            Window.MouseDown += MouseDown;
            Window.MouseUp += MouseUp;
            Window.MouseWheel += MouseWheel;

            // tests
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            
            // Initialise dependencies
            Dependencies = new DependencyContainer();
            Dependencies.Register<GameWindow>(Window);
            Projection = Matrix4.CreateOrthographicOffCenter(0, Window.Size.X, Window.Size.Y, 0, -1, 1); 

            //Children = new Container();
        }

        public virtual void Resize(ResizeEventArgs args)
        {
            GL.Viewport(0,0,args.Width, args.Height);
            Projection = Matrix4.CreateOrthographicOffCenter(0, Window.Size.X, Window.Size.Y, 0, -1, 1);
        }

        protected int VertexArrayObject;
        
        public virtual void Load(DependencyContainer dependencies)
        { 
            GL.ClearColor(BackgroundColour);
            
            // VertexArrayObject
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
        }

        public virtual void LoadComplete(DependencyContainer container)
        {
            
        }

        public virtual void Update(FrameEventArgs args)
        {
        }

        void draw(FrameEventArgs args)
        {
            GL.ClearColor(BackgroundColour);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindVertexArray(VertexArrayObject);

            Draw(args);
            
            Window.SwapBuffers();
        }
        
        protected virtual void Draw(FrameEventArgs args)
        { }

        public virtual void Unload(DependencyContainer dependencies)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            GL.DeleteVertexArray(VertexArrayObject);
        }

        public void Run() => Window.Run();
        
        #region input
        protected virtual void MouseWheel(MouseWheelEventArgs args)
        { }

        protected virtual void MouseUp(MouseButtonEventArgs args)
        { }

        protected virtual void MouseDown(MouseButtonEventArgs args)
        { }

        protected virtual void MouseMove(MouseMoveEventArgs args)
        { }

        protected virtual void KeyUp(KeyboardKeyEventArgs args)
        { }

        protected virtual void KeyDown(KeyboardKeyEventArgs args)
        { }
        #endregion
        
        public void Dispose()
        {
            // TODO: dispose disposable dependencies
            Bass.Free();
            YasaiLogger.LogInfo("Disposed of resources and exited successfully");
        }
    }
}
