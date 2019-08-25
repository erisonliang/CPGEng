# CPGEng
Crispycat PixelGraphic Engine: A simple 2D C# and soft engine

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
	this.Dispatcher.Invoke(() => { MyView.Update(MyImageControl); }); // Update an Image control
	BitmapSource _ = MyView.Output(); // Get image data as a BitmapSource
}

// Create a timer and run the loop
Timer FrameTimer = new Timer();
FrameTimer.Interval = 1000 / 30; // Target 30 FPS
FrameTimer.Elapsed += new ElapsedEventHandler((object sender, ElapsedEventArgs e) => {
	try{
		MainLoop();
	} catch (Exception e) {
		Console.WriteLine(e);
	}
});
FrameTimer.Start();
```

## Drawing functions
### SetPixel / DrawPoint
Sets pixel (x, y) to the color specified.
```csharp
MyView.SetPixel(0, 0, new ColorInt(0, 255, 255));
MyView.DrawPoint(0, 1, new ColorInt(0, 255, 255));
```

### DrawLine
Draws a line from point (x1, y1) to point (x2, y2).
```csharp
MyView.DrawLine(0, 0, 500, 300, new ColorInt(0, 255, 0));
```

### DrawRectangle
Draws a rectangle from point (x1, y1) to point (x2, y2).
```csharp
MyView.DrawRectangle(0, 0, 500, 300, new ColorInt(0, 255, 0));
```

### DrawFilledRectangle
Draws a filled rectangle from point (x1, y1) to point (x2, y2).
```csharp
MyView.DrawFilledRectangle(0, 0, 500, 300, new ColorInt(0, 255, 0));
```

### DrawTriangle
Draws a triangle with points (x1, y1), (x2, y2), (x3, y3).
```csharp
MyView.DrawTriangle(100, 100, 600, 200, 300, 250, new ColorInt(255, 255, 0));
```

### DrawFilledTriangle
Draws a filled triangle with points (x1, y1), (x2, y2), (x3, y3).
```csharp
MyView.DrawFilledTriangle(100, 100, 600, 200, 300, 250, new ColorInt(255, 255, 0));
```

### DrawEllipse
Draws an ellipse from point (x1, y1) to point (x2, y2).
```csharp
MyView.DrawEllipse(0, 0, 500, 300, new ColorInt(0, 255, 255);
```

### DrawFilledElipse
Draws a filled ellipse from point (x1, y1) to point (x2, y2).
```csharp
MyView.DrawFilledEllipse(0, 0, 500, 300, new ColorInt(0, 0, 255));
```

### DrawPoly
Draws a polygon of color c using points (x1, y1, x2, y2, ...).
```csharp
MyView.DrawPoly(new ColorInt(0, 0, 255), 100, 200, 300, 150, 0, 150); // Draw a trapezoid
```

### DrawFilledPoly
Draws a filled polygon of color c using points (x1, y1, x2, y2, ...).
```csharp
MyView.DrawPoly(new ColorInt(0, 0, 255), 100, 200, 300, 150, 0, 150); // Draw a trapezoid
```

### DrawBitmap
Draws a bitmap image represented as a `MyCPGBitmapData` at location (x, y).
```csharp
MyView.DrawBitmap(2, 2, MyCPGBitmapData);
```

## Sprites
== TODO ==
