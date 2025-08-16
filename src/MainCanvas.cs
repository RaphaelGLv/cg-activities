using GraphicsComputingApp.components;
using GraphicsComputingApp.enums;
using GraphicsComputingApp.utils;

namespace GraphicsComputingApp;

public class MainCanvas : Form
{
    private Point mouseStartPoint;
    private Point mouseCurrentPoint;
    private readonly FunctionsHeader functionsHeader = new FunctionsHeader();
    private PictureBox canvas;
    private Bitmap bmp;
    private Graphics g;

    private bool isDrawing = false;
    private CanvasFunctions _activeFunction = CanvasFunctions.None;
    
    public MainCanvas()
    {
        this.Text = "Trabalho de Computação Gráfica - 2025";
        this.Size = new Size(800, 600);
        
        this._InitializeCanvas();
        
        this._CreateButtons();
    }

    private void _CreateButtons()
    {
        functionsHeader.CreateButton("Mouse", (_, _) => _SetActiveFunction(CanvasFunctions.None), Controls);
        functionsHeader.CreateButton("Linha (Eq. Geral)", (_, _) => _SetActiveFunction(CanvasFunctions.StandardLine), Controls);
        functionsHeader.CreateButton("Linha (Eq. Paramétrica)", (_, _) => _SetActiveFunction(CanvasFunctions.ParametricLine), Controls);
        functionsHeader.CreateButton("Círculo (Eq. Geral)", (_, _) => _SetActiveFunction(CanvasFunctions.StandardCircle), Controls);
    }

    private void _SetActiveFunction(CanvasFunctions function)
    {
        Cursor = function == CanvasFunctions.None ? Cursors.Default : Cursors.Cross;

        _activeFunction = function;
    }

    private void _InitializeCanvas()
    {
        canvas = new PictureBox();
        canvas.Location = new Point(10, 50);
        canvas.Size = new Size(760, 500);
        canvas.BorderStyle = BorderStyle.Fixed3D;
        this.Controls.Add(canvas);

        bmp = new Bitmap(canvas.Width, canvas.Height);
        g = Graphics.FromImage(bmp);
        g.Clear(Color.White);
        canvas.Image = bmp;
        
        canvas.MouseDown += canvas_MouseDown;
        canvas.MouseMove += canvas_MouseMove;
        canvas.MouseUp += canvas_MouseUp;
    }

    private void _DrawPixel(int x, int y)
    {
        g.FillRectangle(new SolidBrush(Color.LightSeaGreen), x, y, 1, 1);
    }

    private void canvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (_activeFunction == CanvasFunctions.None) return;
        
        _SetIsDrawing(true);
        
        mouseStartPoint = e.Location;
        mouseCurrentPoint = e.Location;
    }

    private void canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (_activeFunction == CanvasFunctions.None) return;
        
        mouseCurrentPoint = e.Location;
        canvas.Invalidate();
    }

    private void canvas_MouseUp(object sender, MouseEventArgs e)
    {
        if (_activeFunction == CanvasFunctions.None || !isDrawing) return;
        
        _SetIsDrawing(false);

        CanvasDrawingFunctions.Draw(_activeFunction, new Tuple<Point, Point>(mouseStartPoint, mouseCurrentPoint), _DrawPixel);
    }

    private void _SetIsDrawing(bool state)
    {
        isDrawing = state;
    }
}