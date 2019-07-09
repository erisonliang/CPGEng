using System;
using System.Timers;
using System.Windows;
using CPGEng;

namespace GraphicsEngineTest {
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			int AreaX = 480, AreaY = 360, VelocityMove = 2, TargetFps = 30;

			// Make a view
			View mainview = new View(AreaX, AreaY, 96);

			// Make the smiley
			Sprite smiley = new Sprite("images\\smiley.png", 32, 32);

			int VelocityX = VelocityMove, VelocityY = VelocityMove;

			// Define main loop
			void MainLoop() {
				// Move smiley
				smiley.PosX = smiley.PosX + VelocityX;
				smiley.PosY = smiley.PosY + VelocityY;
				// Trail
				mainview.SetPixel(smiley.PosX + smiley.SizeX / 2, smiley.PosY + smiley.SizeY / 2, new ColorInt(0, 200, 255));
				// Bounce logic
				if (smiley.PosX + smiley.SizeX > AreaX) VelocityX = -VelocityMove;
				if (smiley.PosY + smiley.SizeY > AreaY) VelocityY = -VelocityMove;
				if (smiley.PosX < 0) VelocityX = VelocityMove;
				if (smiley.PosY < 0) VelocityY = VelocityMove;
				// Add sprite and update view
				mainview.AddSprite(smiley);
				mainview.Update(ViewBox);
			}

			// Run main loop
			Timer frametime = new Timer();
			frametime.Interval = (int)Math.Ceiling((double)(1000 / TargetFps));
			frametime.Elapsed += new ElapsedEventHandler((object sender, ElapsedEventArgs e) => {
				this.Dispatcher.Invoke(MainLoop);
			});
			frametime.Start();
		}
	}
}
