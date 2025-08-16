using GraphicsComputingApp.components;
using GraphicsComputingApp.enums;

namespace GraphicsComputingApp;

public class MainCanvas : Form
{
    private readonly FunctionsHeader functionsHeader = new FunctionsHeader();
    private Button btnLinha;
    private Button btnCirculo;
    private PictureBox canvas;
    private Bitmap bmp;
    private Graphics g;
    
    private CanvasFunctions _activeFunction = CanvasFunctions.None;
    
    public MainCanvas()
    {
        this.Text = "Trabalho de Computação Gráfica - 2025";
        this.Size = new Size(800, 600);
        
        // Canvas
        canvas = new PictureBox();
        canvas.Location = new Point(10, 50);
        canvas.Size = new Size(760, 500);
        canvas.BorderStyle = BorderStyle.Fixed3D;
        this.Controls.Add(canvas);

        // Criar bitmap para desenhar
        bmp = new Bitmap(canvas.Width, canvas.Height);
        g = Graphics.FromImage(bmp);
        g.Clear(Color.White); // fundo branco
        canvas.Image = bmp;
    }
    
}