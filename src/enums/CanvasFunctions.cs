using System.ComponentModel;

namespace GraphicsComputingApp.enums;

public enum CanvasFunctions
{
    None,
    [Description("Linha (Eq. Geral)")]
    StandardLine,
    [Description("Linha (Eq. Paramétrica)")]
    ParametricLine,
    [Description("Círculo (Eq. Geral)")]
    StandardCircle,
    [Description("Círculo (Eq. Paramétrica)")]
    ParametricCircle,
    [Description("Círculo (Por Rotações)")]
    RotationsCircle,
    [Description("Círculo (Por Bresenham)")]
    BresenhamCircle,
    [Description("Quadro de Cohen")]
    CohenRectangle,
    [Description("Linha de Cohen")]
    CohenLine,
    [Description("Projeção 3D - 2D: Desenhar Casa")]
    Projection3D,
}