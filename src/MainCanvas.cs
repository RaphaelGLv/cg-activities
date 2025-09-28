using GraphicsComputingApp.components;
using GraphicsComputingApp.enums;
using GraphicsComputingApp.services;
using GraphicsComputingApp.src.components;
using GraphicsComputingApp.src.components.entities;
using GraphicsComputingApp.src.services;
using GraphicsComputingApp.src.shapes;
using GraphicsComputingApp.src.utils;
using GraphicsComputingApp.utils;
using System.Numerics;

namespace GraphicsComputingApp;

public class MainCanvas : Form
{
    const int CANVAS_WIDTH = 1600;
    const int CANVAS_HEIGHT = 900;

    private static MainCanvas _instance;
    private static readonly object _lock = new object();

    private CanvasHeader _canvasHeader;
    private ProjectionSideBar _projectionSideBar;
    private Panel _canvasPanel;
    private Panel _sidebarPanel;
    private PictureBox _canvas;
    private Bitmap _bmp;
    private Graphics _g;

    private bool isDrawing = false;
    private readonly Stack<Point> _clickStack = new();
    private CanvasFunctions _activeFunction = CanvasFunctions.None;

    public AppRectangle clippingRect = null;
    public HouseShape3D house = new HouseShape3D();

    private MainCanvas()
    {
        Text = "Trabalho de Computação Gráfica - 2025";
        WindowState = FormWindowState.Maximized;
        Width = 800;
        Height = 600;
        Dock = DockStyle.Fill;

        _BuildHeader();

        _BuildSideBar();

        _InitializeCanvas();
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
        _canvasHeader = new CanvasHeader(this.Controls);

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
            new(CanvasFunctions.CohenLine.GetDescription(), (_, _) => _SetActiveFunction(CanvasFunctions.CohenLine)),
            new(CanvasFunctions.Projection3D.GetDescription(), (_, _) =>
            {
                house ??= new HouseShape3D();
                _SetActiveFunction(CanvasFunctions.Projection3D);
                CanvasDrawingServices.Draw(_activeFunction, new(new(200, 200), new(0, 0)), _DrawPixel);
            }),
        };

        _canvasHeader.CreateComboBox(drawingFunctionsList);
        _canvasHeader.CreateButton("Limpar canvas", (_, _) => _ClearCanvas());
    }

    private void _BuildSideBar()
    {
        if (_sidebarPanel == null)
        {
            _sidebarPanel = new Panel
            {
                Width = 250,
                Dock = DockStyle.Right
            };
            this.Controls.Add(_sidebarPanel);
        }
        _projectionSideBar = new ProjectionSideBar(_sidebarPanel.Controls, 10);

        _projectionSideBar.ExecuteClicked += (s, e) => ApplySidebarTransformations();
    }

    private void _ClearCanvas()
    {
        ClearClippingRect();
        _ClearHouse();
        _g.Clear(Color.White);
        _canvas.Refresh();
    }

    private void _ClearHouse()
    {
        if (house is null) return;

        _EraseHouse();

        house = null;
    }

    private void _EraseHouse()
    {
        if (house is null) return;

        CanvasDrawingServices.Draw(CanvasFunctions.Projection3D, new(new(0, 0), new(0, 0)), _ErasePixel);

        _canvas.Invalidate();
    }
    public void ClearClippingRect()
    {
        if (clippingRect is null) return;

        var fineTune = 1;
        int x = Math.Min(clippingRect.topLeft.X, clippingRect.bottomRight.X);
        int y = Math.Min(clippingRect.topLeft.Y, clippingRect.bottomRight.Y);
        int width = Math.Abs(clippingRect.topRight.X - clippingRect.topLeft.X) + fineTune;
        int height = Math.Abs(clippingRect.bottomLeft.Y - clippingRect.topLeft.Y) + fineTune;

        SolidBrush brush = new SolidBrush(Color.White);
        _g.FillRectangle(brush, x, y, width, height);
        _canvas.Invalidate();

        clippingRect = null;
    }

    public void SelectDrawingFunctionByTitle(string title)
    {
        _canvasHeader.SelectComboBoxItemByTitle(title);
    }

    private void _SetActiveFunction(CanvasFunctions function)
    {
        if (function == CanvasFunctions.None || function == CanvasFunctions.Projection3D)
        {
            Cursor = Cursors.Default;
            isDrawing = false;
        }
        else
        {
            Cursor = Cursors.Cross;
            isDrawing = true;
        }

        _clickStack.Clear();

        _activeFunction = function;
    }

    private void _InitializeCanvas()
    {
        if (_canvasPanel == null)
        {
            _canvasPanel = new Panel
            {
                Width = CANVAS_WIDTH,
                Height = CANVAS_HEIGHT,
            };
            this.Controls.Add(_canvasPanel);
        }

        _canvas = new PictureBox
        {
            Width = CANVAS_WIDTH,
            Height = CANVAS_HEIGHT,
            BorderStyle = BorderStyle.Fixed3D
        };
        _canvasPanel.Controls.Add(_canvas);

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

    private void _ErasePixel(int x, int y)
    {
        _g.FillRectangle(new SolidBrush(Color.White), x, y, 1, 1);
    }

    private void canvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (!isDrawing) return;

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
        if (!isDrawing) return;

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

    private void ApplySidebarTransformations()
    {
        if (house is null) return;

        Matrix4x4 transform = Matrix4x4.Identity;

        var houseCenter = house.Get3DCenter();
        var toOrigin = ShapeProjectionServices.CreateTranslation(-houseCenter.X, -houseCenter.Y, -houseCenter.Z);
        var fromOrigin = ShapeProjectionServices.CreateTranslation(houseCenter.X, houseCenter.Y, houseCenter.Z);

        if (_projectionSideBar.radioLocal.Checked)
        {
            float sx = float.Parse(_projectionSideBar.inputLocalX.Text);
            float sy = float.Parse(_projectionSideBar.inputLocalY.Text);
            float sz = float.Parse(_projectionSideBar.inputLocalZ.Text);

            var scale = ShapeProjectionServices.CreateScale(sx, sy, sz);

            transform = transform * toOrigin;
            transform = transform * scale;
            transform = transform * fromOrigin;
        }

        else if (_projectionSideBar.radioGlobal.Checked)
        {
            float s = float.Parse(_projectionSideBar.inputGlobal.Text);

            var scale = ShapeProjectionServices.CreateScale(s, s, s);

            transform = transform * toOrigin;
            transform = transform * scale;
            transform = transform * fromOrigin;
        }

        else if (_projectionSideBar.radioTranslation.Checked)
        {
            float tx = float.Parse(_projectionSideBar.inputTX.Text);
            float ty = float.Parse(_projectionSideBar.inputTY.Text);
            float tz = float.Parse(_projectionSideBar.inputTZ.Text);
            transform = transform * ShapeProjectionServices.CreateTranslation(tx, ty, tz);
        }

        else if (_projectionSideBar.radioOrigem.Checked)
        {   
            float degrees = (float)_projectionSideBar.degreesInput.Value;
            string axis = _projectionSideBar.axisComboBox.SelectedItem.ToString();
            transform = transform * ShapeProjectionServices.CreateRotation(axis, degrees);
        }

        else if (_projectionSideBar.radioCentro.Checked)
        {
            float degrees = (float)_projectionSideBar.degreesInput.Value;
            string axis = _projectionSideBar.axisComboBox.SelectedItem.ToString();

            var rotation = ShapeProjectionServices.CreateRotation(axis, degrees);

            transform = transform * toOrigin;
            transform = transform * rotation;
            transform = transform * fromOrigin;
        }

        if (_projectionSideBar.radioShearing.Checked)
        {
            float[,] shear = new float[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    shear[i, j] = float.Parse(_projectionSideBar.shearingMatrix[i, j].Text);

            transform = transform * toOrigin;
            transform = transform * ShapeProjectionServices.CreateShearing(shear);
            transform = transform * fromOrigin;
        }

        _EraseHouse();

        house.ApplyTransformation(transform);

        CanvasDrawingServices.Draw(CanvasFunctions.Projection3D, new(new(0, 0),new(0,0)), _DrawPixel);
        _canvas.Invalidate();
    }

}