using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Calculadora
{
    /// <summary>
    /// Lógica de interacción para CalculadoraUserControll.xaml
    /// </summary>
    public partial class CalculadoraUserControll : UserControl
    {
        public CalculadoraUserControll()
        {
            InitializeComponent();
        }

       
        // Controlador de eventos para el clic en los botones de números y operadores
        private void ButtonNumberOperator_Click(object sender, RoutedEventArgs e)
        {
            // Agregar el contenido del botón clickeado (número u operador) al cuadro de texto de entrada
            tbInput.Text += (sender as Button).Content.ToString();

            // Enfocar en el cuadro de texto de entrada para recibir más entradas
            tbInput.Focus();

            // Evaluar la expresión actual y mostrar el resultado en el cuadro de texto de salida
            tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
        }

        // Controlador de eventos para la vista previa de la entrada de texto en el cuadro de texto de entrada
        private void tbInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Utilizar una expresión regular para prevenir la entrada de caracteres inválidos
            Regex regex = new Regex("[^0-9.*/+-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Controlador de eventos para el clic en el botón "Borrar"
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Eliminar el último carácter del cuadro de texto de entrada
            if (tbInput.Text.Length > 0)
            {
                tbInput.Text = (tbInput.Text).Substring(0, tbInput.Text.Length - 1);
            }

            // Evaluar la expresión actualizada y mostrar el resultado en el cuadro de texto de salida
            tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
        }

        // Controlador de eventos para el clic en el botón "Borrar Todo"
        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar el cuadro de texto de entrada
            tbInput.Text = "";

            // Establecer el cuadro de texto de salida para mostrar "0"
            tbOutput.Text = "0";
        }

        // Controlador de eventos para el clic en el botón "Igual"
        private void btnIgual_Click(object sender, RoutedEventArgs e)
        {
            // Establecer el contenido del cuadro de texto de entrada igual al contenido actual del cuadro de texto de salida
            // Esto permite al usuario utilizar el resultado de la expresión anterior en un nuevo cálculo.
            AgregarAlHistorial(tbInput.Text);
            tbInput.Text = tbOutput.Text;
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Evaluar la expresión actual y mostrar el resultado en el cuadro de texto de salida
            if (tbInput.Text.Length > 0 && tbOutput != null)
            {
                tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
            }
        }
        private void NuevoLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Manejar el clic en un Label del historial
            if (sender is Label clickedLabel)
            {
                // Cambiar el texto de tbInput al contenido del Label
                tbInput.Text = clickedLabel.Content.ToString();

                // Enfocar en el cuadro de texto de entrada para recibir más entradas
                tbInput.Focus();
            }
        }

        private void AgregarAlHistorial(string expresion)
        {
            // Crear un nuevo Label con la expresión y manejar el evento MouseDown
            Label nuevoLabel = new Label
            {
                Content = expresion,
                Cursor = Cursors.Hand,
                Margin = new Thickness(0, 5, 0, 0),
                FontSize = 22,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = (System.Windows.Media.Brush)this.FindResource("Color.Foreground"),
            };

            // Agregar un controlador de eventos para el clic en el nuevo Label
            nuevoLabel.MouseDown += NuevoLabel_MouseDown;

            // Agregar el nuevo Label al ListBox (historialListBox)
            historialListBox.Items.Add(nuevoLabel);

            // Desplazar el ListBox hacia abajo para mostrar el nuevo elemento
            historialListBox.ScrollIntoView(nuevoLabel);
        }

        private void stack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Si el clic no ocurrió directamente en un elemento secundario del StackPanel,
            // liberar el foco del ratón para evitar problemas con eventos de clic.
            Mouse.Capture(null);
        }
    }
}
