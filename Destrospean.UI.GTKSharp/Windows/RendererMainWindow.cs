using System.Collections.Generic;
using Destrospean.Graphics.OpenGL;
using OpenTK;

namespace Destrospean.DestrospeanCASPEditor
{
    public abstract class RendererMainWindow : MainWindowBase
    {
        protected float mFOV = MathHelper.DegreesToRadians(30),
        mMouseX,
        mMouseY,
        mTime = 0;

        protected List<Gdk.Key> mKeysHeld = new List<Gdk.Key>();

        protected Vector2 mLastMousePosition;

        protected MouseButtonsHeld mMouseButtonsHeld = MouseButtonsHeld.None;

        public Gtk.GLWidget GLWidget
        {
            get;
            protected set;
        }

        public OpenTK.Graphics.IGraphicsContext GraphicsContext
        {
            get
            {
                return (OpenTK.Graphics.IGraphicsContext)GLWidget.GetType().GetField("graphicsContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(GLWidget);
            }
        }

        public readonly Graphics.OpenGL.Sims3.Sim Sim;

        public OpenTK.Platform.IWindowInfo WindowInfo
        {
            get
            {
                return (OpenTK.Platform.IWindowInfo)GLWidget.GetType().GetField("windowInfo", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(GLWidget);
            }
        }

        [System.Flags]
        public enum MouseButtonsHeld : byte
        {
            None,
            Left,
            Middle,
            Right = 4
        }

        public RendererMainWindow(Gtk.WindowType windowType) : base(windowType)
        {
            Common.Abstractions.Complate.MarkModelsNeedUpdatedCallback = () => Gtk.Application.Invoke((sender, e) => NextState = NextStateOptions.UpdateModels);
            Sim = new Graphics.OpenGL.Sims3.Sim();
        }

        protected bool OnIdleProcessMain()
        {
            if (GlobalState.GLInitialized)
            {
                GlobalState.OnUpdateFrame(ProcessInput, mFOV, (float)GLWidget.Allocation.Width / GLWidget.Allocation.Height);
                GlobalState.OnRenderFrame((int)(GLWidget.Allocation.Width * WidgetUtils.WineScaleDenominator), (int)(GLWidget.Allocation.Height * WidgetUtils.WineScaleDenominator));
                return true;
            }
            return false;
        }

        [GLib.ConnectBefore]
        protected void OnKeyPress(object sender, Gtk.KeyPressEventArgs args)
        {
            if (!mKeysHeld.Contains(args.Event.Key))
            {
                mKeysHeld.Add(args.Event.Key);
            }
            //args.RetVal = true;
        }

        protected void PrepareGLWidget()
        {
            try
            {
                GLWidget = new Gtk.GLWidget();
                GLWidget.AddEvents((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.PointerMotionMask | Gdk.EventMask.KeyPressMask | Gdk.EventMask.KeyReleaseMask | Gdk.EventMask.ScrollMask));
                GLWidget.ButtonPressEvent += (o, args) => mMouseButtonsHeld |= (MouseButtonsHeld)System.Math.Pow(2, args.Event.Button - 1);
                GLWidget.ButtonReleaseEvent += (o, args) => mMouseButtonsHeld &= (MouseButtonsHeld)(byte.MaxValue - System.Math.Pow(2, args.Event.Button - 1));
                GLWidget.ScrollEvent += (o, args) =>
                    {
                        var delta = MathHelper.DegreesToRadians(1);
                        switch ((int)args.Event.Direction)
                        {
                            case 0:
                                mFOV -= mFOV - delta > 0 ? delta : 0;
                                break;
                            case 1:
                                mFOV += mFOV + delta <= MathHelper.DegreesToRadians(110) ? delta : 0;
                                break;
                        }
                    };
                GLWidget.MotionNotifyEvent += (o, args) =>
                    {
                        if (args.Event.Device.Source == Gdk.InputSource.Mouse)
                        {
                            double currentX = args.Event.X,
                            currentY = args.Event.Y;
                            var currentTime = args.Event.Time;
                            if (mTime > 0)
                            {
                                double deltaX = currentX - mMouseX,
                                deltaY = currentY - mMouseY,
                                distance = System.Math.Sqrt(deltaX * deltaX + deltaY * deltaY),
                                secondsElapsed = (currentTime - mTime) * .001;
                                if (secondsElapsed > 0)
                                {
                                    GlobalState.Camera.MoveSpeed = (float)(distance / secondsElapsed * .0001);
                                }
                            }
                            mMouseX = (float)currentX;
                            mMouseY = (float)currentY;
                            mTime = (float)currentTime;
                        }
                    };
                GLWidget.Initialized += (sender, e) => 
                    {
                        GlobalState.InitProgram();
                        GlobalState.GLInitialized = true;
                        NextState = NextStateOptions.UpdateModels;
                        GLib.Idle.Add(new GLib.IdleHandler(OnIdleProcessMain));
                    };
                KeyPressEvent += OnKeyPress;
                KeyReleaseEvent += (o, args) => mKeysHeld.RemoveAll(x => x == args.Event.Key);
            }
            catch (System.Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
                throw;
            }
        }

        protected void ProcessInput()
        {
            try
            {
                var delta = mLastMousePosition - new Vector2(mMouseX, mMouseY);
                mLastMousePosition += delta;
                if (mMouseButtonsHeld.HasFlag(MouseButtonsHeld.Left))
                {
                    if (mKeysHeld.Contains(Gdk.Key.Control_L))
                    {
                        if (mKeysHeld.Contains(Gdk.Key.Alt_L))
                        {
                            GlobalState.CurrentRotation.X -= delta.Y * GlobalState.Camera.MouseSensitivity;
                            GlobalState.CurrentRotation.Z += delta.X * GlobalState.Camera.MouseSensitivity;
                        }
                        else
                        {
                            GlobalState.CurrentRotation.Y += delta.X * GlobalState.Camera.MouseSensitivity;
                        }
                    }
                    else if (mKeysHeld.Contains(Gdk.Key.Alt_L))
                    {
                        mFOV += delta.Y > 0 && mFOV + delta.Y * GlobalState.Camera.MouseSensitivity <= MathHelper.DegreesToRadians(110) || delta.Y < 0 && mFOV + delta.Y * GlobalState.Camera.MouseSensitivity > 0 ? delta.Y * GlobalState.Camera.MouseSensitivity : 0;
                    }
                    else
                    {
                        GlobalState.Camera.AddTranslation(delta.X, -delta.Y, 0);
                    }
                }
                if (mMouseButtonsHeld.HasFlag(MouseButtonsHeld.Middle))
                {
                    GlobalState.CurrentRotation.X -= delta.Y * GlobalState.Camera.MouseSensitivity;
                    GlobalState.CurrentRotation.Z += delta.X * GlobalState.Camera.MouseSensitivity;
                }
                if (mMouseButtonsHeld.HasFlag(MouseButtonsHeld.Right))
                {
                    if (mKeysHeld.Contains(Gdk.Key.Alt_L))
                    {
                        GlobalState.CurrentRotation.X -= delta.Y * GlobalState.Camera.MouseSensitivity;
                        GlobalState.CurrentRotation.Z += delta.X * GlobalState.Camera.MouseSensitivity;
                    }
                    else
                    {
                        GlobalState.CurrentRotation.Y += delta.X * GlobalState.Camera.MouseSensitivity;
                    }
                }
                mLastMousePosition = new Vector2(mMouseX, mMouseY);
            }
            catch (System.Exception ex)
            {
                Common.ProgramUtils.WriteError(ex);
                throw;
            }
        }
    }
}
