using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Diagnostics;

using SharpDX;
using SharpDX.DirectWrite;
using SharpDX.Windows;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace GameFramework
{
    public class GameForm : RenderForm
    {
        SharpDX.DirectWrite.Factory dwFactory = new SharpDX.DirectWrite.Factory();

        WindowRenderTarget d2dRenderTarget;

        public void Run()
        {
            game.LoadContent(d2dRenderTarget);

            RenderLoop.Run(this, OnApplicationIdle);
        }

        private IGame game;

        public GameForm(IGame game)
        {
            this.game = game;

            Width = game.Resolution.Width;
            Height = game.Resolution.Height;

            HwndRenderTargetProperties hwndRenderProperties = new HwndRenderTargetProperties();
            hwndRenderProperties.Hwnd = this.Handle;
            hwndRenderProperties.PixelSize = new DrawingSize(game.Resolution.Width, game.Resolution.Height);
            hwndRenderProperties.PresentOptions = PresentOptions.None;

            RenderTargetProperties renderTargetProperties = new RenderTargetProperties();
            renderTargetProperties.PixelFormat = new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);

            d2dRenderTarget = new WindowRenderTarget(new SharpDX.Direct2D1.Factory(), renderTargetProperties, hwndRenderProperties);
        }

        private void OnApplicationIdle()
        {          
            game.Update();

            Render();
        }

        private void Render()
        {
            d2dRenderTarget.BeginDraw();
            d2dRenderTarget.Clear(game.BackgroundColor);
            game.Render(d2dRenderTarget);
            d2dRenderTarget.Flush();
            d2dRenderTarget.EndDraw();
        }
    }
}
