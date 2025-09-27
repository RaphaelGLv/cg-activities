using GraphicsComputingApp.components;
using GraphicsComputingApp.enums;
using GraphicsComputingApp.services;
using GraphicsComputingApp.src.shapes;
using GraphicsComputingApp.src.utils;
using GraphicsComputingApp.utils;

namespace GraphicsComputingApp;

public class MainCanvas : Form
{
    private static MainCanvas _instance;
    private static readonly object _lock = new object();

    private CanvasHeader _canvasHeader;
    private PictureBox _canvas;
    private Bitmap _bmp;
    private Graphics _g;

    private readonly Stack<Point> _clickStack = new();
    public AppRectangle clippingRect = null;
    private CanvasFunctions _activeFunction = CanvasFunctions.None;
    
    private MainCanvas()
    {
        this.Text = "Trabalho de Computação Gráfica - 2025";
        this.Size = new Size(800, 600);
        
        this._InitializeCanvas();
        
        this._BuildHeader();
    }

    public static MainCanvas GetInstance()
    {
        if (_instance != null) return _instance;

        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new MainCanvas();
            }
        }
        return _instance;
    }

    private void _BuildHeader()
    {
        _canvasHeader = new CanvasHeader(Controls);

        var drawingFunctionsList = new List<DrawingFunctionOption>
        {
            new("Mouse", (_, _) => _SetActiveFunction(CanvasFunctions.None)),
            new(CanvasFunctions.StandardLine.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.StandardLine)),
            new(CanvasFunctions.ParametricLine.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.ParametricLine)),
            new(CanvasFunctions.StandardCircle.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.StandardCircle)),
            new(CanvasFunctions.ParametricCircle.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.ParametricCircle)),
            new(CanvasFunctions.RotationsCircle.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.RotationsCircle)),
            new(CanvasFunctions.BresenhamCircle.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.BresenhamCircle)),
            new(CanvasFunctions.CohenRectangle.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.CohenRectangle)),
            new(CanvasFunctions.CohenLine.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.CohenLine))

        };
        
        _canvasHeader.CreateComboBox(drawingFunctionsList);
        _canvasHeader.CreateButton("Limpar canvas", (_, _) => _ClearCanvas());
    }
    private void _ClearCanvas()
    {
        ClearClippingRect();
        _g.Clear(Color.White);
        _canvas.Refresh();
    }
    public void ClearClippingRect()
    {
        if (clippingRect == null) return;

        var fineTune = 1;
        int x = Math.Min(clippingRect.topLeft.X, clippingRect.bottomRight.X);
        int y = Math.Min(clippingRect.topLeft.Y, clippingRect.bottomRight.Y);
        int width = Math.Abs(clippingRect.topRight.X - clippingRect.topLeft.X) + fineTune;
        int height = Math.Abs(clippingRect.bottomLeft.Y - clippingRect.topLeft.Y) + fineTune;

        using (SolidBrush brush = new SolidBrush(Color.White))
        {
            _g.FillRectangle(brush, x, y, width, height);
        }
        _canvas.Invalidate();

        clippingRect = null;
    }

    public void SelectDrawingFunctionByTitle(string title)
    {
        _canvasHeader.SelectComboBoxItemByTitle(title);
    }

    private void _SetActiveFunction(CanvasFunctions function)
    {
        Cursor = function == CanvasFunctions.None ? Cursors.Default : Cursors.Cross;

        _clickStack.Clear();

        _activeFunction = function;
    }

    private void _InitializeCanvas()
    {
        _canvas = new PictureBox();
        _canvas.Location = new Point(10, 50);
        _canvas.Size = new Size(760, 500);
        _canvas.BorderStyle = BorderStyle.Fixed3D;
        this.Controls.Add(_canvas);

        _bmp = new Bitmap(_canvas.Width, _canvas.Height);
        _g = Graphics.FromImage(_bmp);
        _g.Clear(Color.White);
        _canvas.Image = _bmp;
        
        _canvas.MouseDown += canvas_MouseDown;
        _canvas.MouseUp += canvas_MouseUp;
    }

    private void _DrawPixel(int x, int y)
    {
        _g.FillRectangle(new SolidBrush(Color.LightSeaGreen), x, y, 1, 1);
        _canvas.Invalidate();
    }

    private void canvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (_activeFunction == CanvasFunctions.None) return;

        _clickStack.Push(e.Location);

        if (_clickStack.Count == 2)
        {
            var lastClick = _clickStack.Pop();
            var firstClick = _clickStack.Pop();
            _clickStack.Clear();
            CanvasDrawingServices.Draw(_activeFunction, new Tuple<Point, Point>(firstClick, lastClick), _DrawPixel);
        }
    }

    private void canvas_MouseUp(object sender, MouseEventArgs e)
    {
        var currentPoint = e.Location;

        var mouseMovementThreshold = 5;
        if (
            _activeFunction == CanvasFunctions.None ||
            _clickStack.Count == 0 ||
            CanvasDrawingUtils.CalculatePointsDistance(new Tuple<Point, Point>(_clickStack.Peek(), currentPoint)) < mouseMovementThreshold
           ) 
        {
            return;
        }
        
        var mouseStartPoint = _clickStack.Pop();

        CanvasDrawingServices.Draw(_activeFunction, new Tuple<Point, Point>(mouseStartPoint, currentPoint), _DrawPixel);
    }

}