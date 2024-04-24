using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculadora
{
    /// <summary>
    /// Clase que permite evaluar y obtener los coeficientes de un polinomio en español.
    /// </summary>
    public static class EvaluadorCoeficientesPolinomio
    {
        /// <summary>
        /// Evalúa un polinomio y devuelve sus coeficientes como un arreglo de tipo double.
        /// </summary>
        /// <param name="polinomio">La cadena que representa el polinomio en español.</param>
        /// <returns>Un arreglo de tipo double que contiene los coeficientes del polinomio.</returns>
        public static double[] EvaluarCoeficientes(string polinomio)
        {
            try
            {
                // Eliminar espacios en blanco y caracteres no deseados
                string polinomioLimpio = Regex.Replace(polinomio, @"\s+", "");

                // Encontrar todos los términos del polinomio usando expresiones regulares
                MatchCollection coincidencias = Regex.Matches(polinomioLimpio, @"([-+]?\s?\d*\.?\d*)?x(\^\d+)?");

                List<double> coeficientes = new List<double>();
                foreach (Match coincidencia in coincidencias)
                {
                    string valorSigno = coincidencia.Groups[1].Value;
                    string numPotencia = coincidencia.Groups[2].Value;

                    double coeficiente = 1.0;
                    if (!string.IsNullOrEmpty(valorSigno))
                    {
                        if (valorSigno == "-")
                            coeficiente = -1.0;
                        else
                            coeficiente = double.Parse(valorSigno);
                    }

                    if (!string.IsNullOrEmpty(numPotencia))
                    {
                        int potencia = int.Parse(numPotencia.Replace("^", ""));
                        AsegurarCapacidad(coeficientes, potencia + 1);
                        coeficientes[potencia] = coeficiente;
                    }
                    else
                    {
                        AsegurarCapacidad(coeficientes, 2);
                        coeficientes[1] = coeficiente;
                    }
                    polinomioLimpio = polinomioLimpio.Replace(coincidencia.ToString(), "");
                }
                if (!string.IsNullOrEmpty(polinomioLimpio))
                {
                    coeficientes[0] = double.Parse(polinomioLimpio);
                }
                else
                {
                    coeficientes[0] = 0.0;
                }
                coeficientes.Reverse();
                return coeficientes.ToArray();

            }
            catch (Exception)
            {
                List<double> arr=new List<double>();
                return arr.ToArray();
            }
        }

        private static void AsegurarCapacidad(List<double> lista, int capacidad)
        {
            int cantidad = lista.Count;
            if (capacidad > cantidad)
            {
                for (int i = 0; i < capacidad - cantidad; i++)
                {
                    lista.Add(0.0);
                }
            }
        }
    }

}
