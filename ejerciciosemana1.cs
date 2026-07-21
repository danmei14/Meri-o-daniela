int tarjetasValidas = 0;
int tarjetasInvalidas = 0;
int visa = 0;
int mastercard = 0;
int amex = 0;
int discover = 0;
int desconocida = 0;
bool salir = false;
Console.WriteLine("Bienvenidos al algoritmo de Luhn");
void MenuPrincipal()    
{
    
    

    do{
        Console.WriteLine("Seleccione una opción:");
        Console.WriteLine("1. Verificar número de tarjeta");
        Console.WriteLine("2. validar desde archivo");
        Console.WriteLine("3. generar numero valido");
        Console.WriteLine("4. estadisticas");
        Console.WriteLine("5. Salir");
        Console.Write("Ingrese el número de la opción deseada: ");
        int opcion = Convert.ToInt32(Console.ReadLine());
        switch (opcion)
        {
            case 1:
                VerificarNumeroTarjeta();
            break;
            case 2:
                Console.Write("Ingrese la ruta del archivo: ");
                string ruta = Console.ReadLine() ?? "";
                ValidarDesdeArchivo(ruta);
            break;
            case 3:
                GenerarNumeroValido();
            break;
            case 4:
                MostrarEstadisticas();  
            break;
            case 5:
                Console.WriteLine("Saliendo");
            break;
        }
      
    }while (!salir);      
}    

MenuPrincipal();
bool ValidarTarjeta(string numero)
{
    int suma = 0;
    bool duplicar = false;

    for (int i = numero.Length - 1; i >= 0; i--)
    {
        int digito = numero[i] - '0';

        if (duplicar == true)
        {
            digito = digito * 2;

            if (digito > 9)
            {
                digito = digito - 9;
            }
        }

        suma = suma + digito;

        if (duplicar == true)
        {
            duplicar = false;
        }
        else
        {
            duplicar = true;
        }
    }

    if (suma % 10 == 0)
    {
        return true;
    }
    else
    {
        return false;
    }
}

string IdentificarMarca(string numero)
{
    if (numero.StartsWith("4") && (numero.Length == 13 || numero.Length == 16))
    {
        return "Visa";
    }

    if (numero.Length == 16)
    {
        int prefijo = Convert.ToInt32(numero.Substring(0, 2));

        if (prefijo >= 51 && prefijo <= 55)
        {
            return "Mastercard";
        }
    }

    if (numero.Length == 15 && (numero.StartsWith("34") || numero.StartsWith("37")))
    {
        return "American Express";
    }

    if (numero.StartsWith("6011") || numero.StartsWith("65"))
    {
        return "Discover";
    }

    return "Desconocida";
}

void GenerarNumeroValido()
{
    Console.Write("Ingrese el prefijo (Ej: 4, 51, 34, 6011): ");
    string prefijo = Console.ReadLine() ?? "";

    Random random = new Random();
    int longitud = 16;

    string numeroBase = prefijo;

    while (numeroBase.Length < longitud - 1)
    {
        int digitoAleatorio = random.Next(0, 10);
        numeroBase = numeroBase + digitoAleatorio;
    }

    bool encontrado = false;

    for (int i = 0; i <= 9; i++)
    {
        string tarjeta = numeroBase + i;

        if (ValidarTarjeta(tarjeta) == true)
        {
            Console.WriteLine("Número generado:");
            Console.WriteLine(tarjeta);
            Console.WriteLine("Marca: " + IdentificarMarca(tarjeta));
            encontrado = true;
            break;
        }
    }

    if (encontrado == false)
    {
        Console.WriteLine("No se pudo generar un número válido.");
    }
}

void MostrarEstadisticas()
{
    Console.WriteLine("\n===== ESTADÍSTICAS =====");
    Console.WriteLine("Válidas: " + tarjetasValidas);
    Console.WriteLine("Inválidas: " + tarjetasInvalidas);
    Console.WriteLine();
    Console.WriteLine("Visa: " + visa);
    Console.WriteLine("Mastercard: " + mastercard);
    Console.WriteLine("American Express: " + amex);
    Console.WriteLine("Discover: " + discover);
    Console.WriteLine("Desconocida: " + desconocida);
}



void VerificarNumeroTarjeta()
{
    int suma = 0;
    int digito = 0;
    bool esValido = false;
    Console.WriteLine("Ingrese el número de tarjeta:");
    string tarjeta = Console.ReadLine() ?? "";
    for (int i = tarjeta.Length - 1; i >= 0; i--)
    {
        if (esValido)
        {
            digito = tarjeta[i] - '0';
            digito *= 2;
            if (digito > 9)
            {
                digito -= 9;
            }
        }
        else
        {
            digito = tarjeta[i] - '0';
        }

        suma += digito;
        esValido = !esValido;
    }

    if (suma % 10 == 0)
    {
        Console.WriteLine("El número de tarjeta es válido");
    }
    else
    {
        Console.WriteLine("El número de tarjeta no es válido");
    }
}
void ValidarDesdeArchivo(string ruta)
{
    try
    {
        string[] tarjetas = File.ReadAllLines(ruta);

        foreach (string tarjeta in tarjetas)
        {
            Console.WriteLine("------------------");
            Console.WriteLine(tarjeta);

            bool resultado = ValidarTarjeta(tarjeta);
            string marca = IdentificarMarca(tarjeta);

            Console.WriteLine("Marca: " + marca);

            if (resultado == true)
            {
                Console.WriteLine("VÁLIDA");
                tarjetasValidas = tarjetasValidas + 1;
            }
            else
            {
                Console.WriteLine("INVÁLIDA");
                tarjetasInvalidas = tarjetasInvalidas + 1;
            }

            if (marca == "Visa")
            {
                visa = visa + 1;
            }
            else if (marca == "Mastercard")
            {
                mastercard = mastercard + 1;
            }
            else if (marca == "American Express")
            {
                amex = amex + 1;
            }
            else if (marca == "Discover")
            {
                discover = discover + 1;
            }
            else
            {
                desconocida = desconocida + 1;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al leer el archivo: " + ex.Message);
    }
}