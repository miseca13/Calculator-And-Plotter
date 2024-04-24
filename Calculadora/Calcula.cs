using System;
using System.Collections.Generic;

namespace Calculadora
{
    /// <summary>
    /// Una clase estática que proporciona métodos para evaluar expresiones matemáticas utilizando notación postfija.
    /// </summary>
    public static class Calcula
    {
        /// <summary>
        /// Evalúa una expresión matemática en notación postfija y devuelve el resultado como una cadena.
        /// </summary>
        /// <param name="expresion">La expresión matemática a evaluar.</param>
        /// <returns>El resultado de la evaluación de la expresión como una cadena.</returns>
        public static string EvaluarExpresion(string expresion)
        {
            try
            {
                // Convertir la expresión en una lista de tokens y almacenarla en una cola.
                Queue<string> cola = new Queue<string>(ExpresionALista(expresion));
                // Pilas para mantener los números y operadores durante la evaluación.
                Stack<double> pilaNumeros = new Stack<double>();
                Stack<string> pilaOperadores = new Stack<string>();
                int aux = 0;
                string Mensaje = "";

                // Procesar cada token en la cola.
                while (cola.Count > 0)
                {
                    string token = cola.Dequeue();
                    double numero;

                    // Manejo especial para el primer token en la expresión.
                    if (aux == 0)
                    {
                        if (EsOperador(token))
                        {
                            switch (token)
                            {
                                case "-":
                                    Mensaje = "COMIENZA CON MENOS"; // Indicar que la expresión comienza con un signo menos.
                                    continue; // Continuar con el próximo token sin procesar más operaciones.
                                case "+":
                                    continue; // Si es un signo más, simplemente continuar con el próximo token.
                                case "*":
                                    throw new Exception("No se puede comenzar con *"); // Lanzar una excepción si la expresión comienza con un asterisco.
                                case "/":
                                    throw new Exception("No se puede comenzar con /"); // Lanzar una excepción si la expresión comienza con una barra inclinada.
                                default:
                                    break;
                            }
                        }
                        aux = 1;
                    }

                    // Si el token es un número, agregarlo a la pila de números.
                    if (double.TryParse(token, out numero))
                    {
                        if (Mensaje == "COMIENZA CON MENOS")
                        {
                            Mensaje = "";
                            pilaNumeros.Push(-numero); // Si la expresión comenzó con un signo menos, agregar el número negativo a la pila.
                        }
                        else
                        {
                            pilaNumeros.Push(numero);
                        }
                    }
                    // Si el token es un operador, procesarlo.
                    else if (EsOperador(token))
                    {
                        // Aplicar primero los operadores con mayor precedencia.
                        while (pilaOperadores.Count > 0 && Precedencia(token) <= Precedencia(pilaOperadores.Peek()))
                        {
                            double resultadoParcial = AplicarOperacion(pilaNumeros.Pop(), pilaOperadores.Pop(), pilaNumeros.Pop());
                            pilaNumeros.Push(resultadoParcial);
                        }
                        // Agregar el operador actual a la pila de operadores.
                        pilaOperadores.Push(token);
                    }
                    // Token inválido (ni número ni operador).
                    else
                    {
                        throw new ArgumentException("Carácter inválido en la expresión: " + token);
                    }
                }

                // Después de procesar todos los tokens, aplicar cualquier operador restante en la pila de operadores.
                while (pilaOperadores.Count > 0)
                {
                    double resultadoParcial = AplicarOperacion(pilaNumeros.Pop(), pilaOperadores.Pop(), pilaNumeros.Pop());
                    pilaNumeros.Push(resultadoParcial);
                }

                // El resultado final será el número restante en la pila de números.
                return pilaNumeros.Pop().ToString();
            }
            catch (DivideByZeroException ex)
            {
                return "Math Error"; // Si ocurre una división por cero, devolver un mensaje de error.
            }
            catch (Exception ex)
            {
                return "Syntax Error"; // Si ocurre cualquier otra excepción, devolver un mensaje de error de sintaxis.
            }
        }

        // Convierte la expresión en una lista de tokens (números y operadores).
        static List<string> ExpresionALista(string expresion)
        {
            List<string> lista = new List<string>();
            string numeroActual = "";
            string operadorActual = "";

            foreach (char c in expresion)
            {
                // Si el carácter es un dígito o un punto decimal, agregarlo al token de número actual.
                if (char.IsDigit(c) || c == '.')
                {
                    if (!string.IsNullOrEmpty(operadorActual))
                    {
                        lista.Add(operadorActual);
                        operadorActual = "";
                    }
                    numeroActual += c;
                }
                // Si el carácter es un operador, agregar el token de número actual (si hay alguno) y luego agregar el token de operador.
                else
                {
                    if (!string.IsNullOrEmpty(numeroActual))
                    {
                        lista.Add(numeroActual);
                        numeroActual = "";
                    }

                    if (!char.IsWhiteSpace(c))
                    {
                        operadorActual += c;
                    }
                    else
                    {
                        lista.Add(operadorActual);
                    }
                }
            }

            if (!string.IsNullOrEmpty(numeroActual))
            {
                lista.Add(numeroActual);
            }

            return lista;
        }

        // Verifica si el token es un operador.
        static bool EsOperador(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "*+" || token == "*-" || token == "/+" || token == "/-";
        }

        // Asigna valores de precedencia a los operadores para el orden de evaluación.
        static int Precedencia(string operador)
        {
            switch (operador)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                case "*-":
                case "*+":
                case "/-":
                case "/+":
                    return 2;
                default:
                    return 0;
            }
        }

        // Realiza las operaciones aritméticas según el operador y los dos operandos.
        static double AplicarOperacion(double b, string operador, double a)
        {
            switch (operador)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    if (b == 0)
                        throw new DivideByZeroException("No se puede dividir entre cero.");
                    return a / b;

                case "*-":
                    return a * -b;
                case "*+":
                    return a * b;
                case "/-":
                    if (-b == 0)
                        throw new DivideByZeroException("No se puede dividir entre cero.");
                    return a / -b;
                case "/+":
                    if (b == 0)
                        throw new DivideByZeroException("No se puede dividir entre cero.");
                    return a / b;
                default:
                    throw new ArgumentException("Operador inválido: " + operador);
            }
        }
    }
}
