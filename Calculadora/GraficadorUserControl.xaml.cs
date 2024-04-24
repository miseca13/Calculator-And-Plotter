using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculadora
{
    /// <summary>
    /// Lógica de interacción para GraficadorUserControl.xaml
    /// </summary>
    public partial class GraficadorUserControl : UserControl
    {
        public GraficadorUserControl()
        {
            InitializeComponent();

            double[] coefficients = EvaluadorCoeficientesPolinomio.EvaluarCoeficientes(tbInput.Text);
            int xMin = -10; // Límite inferior del eje x
            int xMax = 10;  // Límite superior del eje x

            GraficaUser.DrawPolynomial(coefficients, xMin, xMax);
        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            double[] coefficients = EvaluadorCoeficientesPolinomio.EvaluarCoeficientes(tbInput.Text);
            if (coefficients.Length>0)
            {
                lbError.Visibility = Visibility.Collapsed;
                GraficaUser.Visibility = Visibility.Visible;
                int xMin; // Límite inferior del eje x
                int xMax;  // Límite superior del eje x

                if(Int32.TryParse(tbX1.Text,out xMin) && Int32.TryParse(tbX2.Text,out xMax))
                {
                    GraficaUser.DrawPolynomial(coefficients, xMin, xMax);
                }
                else
                {
                    GraficaUser.Visibility = Visibility.Collapsed;
                    lbError.Visibility = Visibility.Visible;
                }
            }
            else
            {
                GraficaUser.Visibility = Visibility.Collapsed;
                lbError.Visibility = Visibility.Visible;
            }
        }
    }
}
