using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Calculadora
{
    /// <summary>
    /// Lógica de interacción para GraficaUserControl.xaml
    /// </summary>
    public partial class GraficaUserControl : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> AxisXFormatter { get; set; }
        public ChartValues<double> Points { get; set; }

        public GraficaUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void DrawPolynomial(double[] coefficients, int xMin, int xMax)
        {
            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();

            for (int x = xMin; x <= xMax; x++)
            {
                xValues.Add(x);
                double y = EvaluatePolynomial(coefficients, x);
                yValues.Add(y);
            }

            SeriesCollection = new SeriesCollection
    {
        new LineSeries
        {
            Title = "Polynomial",
            Values = new ChartValues<double>(yValues),
            PointGeometry = DefaultGeometries.Circle, // Mostrar puntos usando la geometría de círculo
            PointGeometrySize = 8 // Tamaño de los puntos en el gráfico
        }
    };

            // Configurar el eje X con los valores de xMin a xMax
            chart.AxisX.Clear();
            chart.AxisX.Add(new Axis
            {
                Title = "x",
                Labels = xValues.Select(x => x.ToString()).ToList(),
                Separator = new LiveCharts.Wpf.Separator
                {
                    IsEnabled = false
                }
            });

            // Configurar la leyenda del eje Y (etiqueta)
            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis
            {
                Title = "f(x)", // Etiqueta para el eje Y
                Separator = new LiveCharts.Wpf.Separator
                {
                    IsEnabled = false
                }
            });

            // Actualizar el gráfico
            chart.Series = SeriesCollection;
        }



        private double EvaluatePolynomial(double[] coefficients, double x)
        {
            double result = 0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] * Math.Pow(x, coefficients.Length - i - 1);
            }
            return result;
        }
    }
}
