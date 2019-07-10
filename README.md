# CPGEng
Crispycat PixelGraphic Engine: A simple 2D C# soft engine

CPGEng provides an easy and quick way to create and animate bitmap graphics.

## Setup
Place CPGEng.cs in your project directory. Add the following usings:
```csharp
using System.Timers;
using CPGEng;
```

### Create a view
```csharp
// Create a new View called MyView
View MyView = new View(1280, 720, 96); // Width, Height, DPI
```

### The loop
```csharp
void MainLoop() {
	// Code to run for each frame
	MyView.Update(MyImageControl); // Update an Image control
}

// Create a timer and run the loop
Timer FrameTimer = new Timer();
FrameTimer.Interval = 1000 / 30; // Target 30 FPS
FrameTimer.Elapsed += new ElapsedEventHandler((object sender, ElapsedEventArgs e) => {
	this.Dispatcher.Invoke(MainLoop);
});
FrameTimer.Start();
```

## Drawing functions
== TODO ==
## Sprites
== TODO ==
