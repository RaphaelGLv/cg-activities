namespace GraphicsComputingApp.components;

public class FunctionsHeader
{
    private const int ButtonDefaultGap = 10;
    private const int ButtonDefaultHorizontalPadding = 4;
    private const int CharWidth = 6;
    private readonly List<Button> _buttons = [];
    private int _containerWidth = ButtonDefaultGap;

    public void CreateButton(string text, EventHandler onClick, Control.ControlCollection controls)
    {
        var button = new Button();
        button.Text = text;
        button.Click += onClick;
        button.Width = (ButtonDefaultHorizontalPadding + text.Length) * CharWidth;
        
        button.Location = new Point(_containerWidth, 10);
        
        _buttons.Add(button);
        _containerWidth += button.Width + ButtonDefaultGap;
        
        controls.Add(button);
    }

    public List<Button> GetButtons()
    {
        return _buttons;
    }
}