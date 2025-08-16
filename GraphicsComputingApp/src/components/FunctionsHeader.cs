namespace GraphicsComputingApp.components;

public class FunctionsHeader
{
    private const int ButtonDefaultWidth = 100;
    private const int ButtonDefaultGap = 10;
    private readonly List<Button> _buttons = [];

    public void AddButton(Button button)
    {
        var buttonLocationX = _buttons.Count * (ButtonDefaultWidth + ButtonDefaultGap) + 10;
        button.Location = new Point(buttonLocationX, 10);
        
        _buttons.Add(button);
    }

    public List<Button> GetButtons()
    {
        return _buttons;
    }
}