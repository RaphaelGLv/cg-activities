namespace GraphicsComputingApp.components;

public class FunctionButton : Button
{
    public FunctionButton(string text, EventHandler onClick)
    {
        this.Text = text;
        this.Click += onClick;
    }
}