using GraphicsComputingApp.components;
using GraphicsComputingApp.enums;
using GraphicsComputingApp.utils;

namespace GraphicsComputingApp;

public class MainCanvas : Form
{
    private Point _mouseStartPoint;
    private Point _mouseCurrentPoint;
    private CanvasHeader _canvasHeader;
    private PictureBox _canvas;
    private Bitmap _bmp;
    private Graphics _g;

    private bool _isDrawing = false;
    private CanvasFunctions _activeFunction = CanvasFunctions.None;
    
    public MainCanvas()
    {
        this.Text = "Trabalho de Computação Gráfica - 2025";
        this.Size = new Size(800, 600);
        
        this._InitializeCanvas();
        
        this._BuildHeader();
    }

    private void _BuildHeader()
    {
        _canvasHeader = new CanvasHeader(Controls);
        
        var drawingFunctionsList = new List<DrawingFunctionOption>
        {
            new("Mouse", (_, _) => _SetActiveFunction(CanvasFunctions.None)),
            new("Linha (Eq. Geral)", (_, _) => _SetActiveFunction(CanvasFunctions.StandardLine)),
            new("Linha (Eq. Paramétrica)", (_, _) => _SetActiveFunction(CanvasFunctions.ParametricLine)),
            new("Círculo (Eq. Geral)", (_, _) => _SetActiveFunction(CanvasFunctions.StandardCircle)),
            new("Círculo (Eq. Paramétrica)", (_, _) => _SetActiveFunction(CanvasFunctions.ParametricCircle)),
            new("Círculo (Por rotações)", (_, _) => _SetActiveFunction(CanvasFunctions.RotationsCircle)),
        };
        
        _canvasHeader.CreateComboBox(drawingFunctionsList);
        _canvasHeader.CreateButton("Limpar canvas", (_, _) => _ClearCanvas());
    }
    private void _ClearCanvas()
    {
        _g.Clear(Color.White);
        _canvas.Refresh();
    }
    private void _SetActiveFunction(CanvasFunctions function)
    {
        Cursor = function == CanvasFunctions.None ? Cursors.Default : Cursors.Cross;

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
        _canvas.MouseMove += canvas_MouseMove;
        _canvas.MouseUp += canvas_MouseUp;
    }

    private void _DrawPixel(int x, int y)
    {
        _g.FillRectangle(new SolidBrush(Color.LightSeaGreen), x, y, 1, 1);
    }

    private void canvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (_activeFunction == CanvasFunctions.None) return;
        
        _SetIsDrawing(true);
        
        _mouseStartPoint = e.Location;
        _mouseCurrentPoint = e.Location;
    }

    private void canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (_activeFunction == CanvasFunctions.None) return;
        
        _mouseCurrentPoint = e.Location;
        _canvas.Invalidate();
    }

    private void canvas_MouseUp(object sender, MouseEventArgs e)
    {
        if (_activeFunction == CanvasFunctions.None || !_isDrawing) return;
        
        _SetIsDrawing(false);

        CanvasDrawingFunctions.Draw(_activeFunction, new Tuple<Point, Point>(_mouseStartPoint, _mouseCurrentPoint), _DrawPixel);
    }

    private void _SetIsDrawing(bool state)
    {
        _isDrawing = state;
    }
}