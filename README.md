# MonoWrapper

A Monogame wrapper library.

## Why

This is a dynamic library in order to work with monogame framework.

This was written to simplify the access to window, input and other systems without having to re-write the code every time.

## How to use

Just download and import the dll.

Here is an example of how to use it to draw a red rectangle in a single scene:

```csharp
public static class Program {
    [STAThread]
    static void Main(){
        Window.Title = "Some Title!";
        Window.IsMouseVisible = true;
        Window.Size = new Point(800, 450);
        Window.ApplyChanges();

        SceneManager.AddScene("root", () => new RootScene());

        Application.Run();
    }
}

public class RootScene : Scene {

    private SpriteBatch _batch;

    public override void Initialize() {
        _batch = new SpriteBatch(Window.GraphicsDevice);
    }

    public override void Dispose()
    {
        _batch.Dispose();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Exit();
        }
    }

    public override void Draw()
    {
        _batch.Begin();
        Gizmos.DrawRectangle(_batch, new Rectangle(10, 10, 100, 200), Color.Red);
        _batch.End();
    }
}
```

## Components

This lib also contains other components to help with some math and drawing stuff to the screen.

Curves, Mathf operations, Camera2D, Flipbook, Gizmos, ImageFit, ImageNinePath, Transform2D, etc...