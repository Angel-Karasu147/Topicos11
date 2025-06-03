using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static volatile int sumaTotal = 0;
    static object lockObject = new object();

    static void CalcularPrimos(object rango)
    {
        (int inicio, int fin) = ((int, int))rango;
        int suma = 0;

        for (int i = inicio; i <= fin; i++)
        {
            if (EsPrimo(i))
            {
                suma += i;
            }
        }

        lock (lockObject)
        {
            sumaTotal += suma;
        }
    }

    static bool EsPrimo(int numero)
    {
        if (numero < 2) return false;
        if (numero == 2 || numero == 3) return true;
        if (numero % 2 == 0) return false;

        for (int i = 3; i * i <= numero; i += 2)
        {
            if (numero % i == 0) return false;
        }

        return true;
    }

    static int CalcularPrimosSecuencial(int N)
    {
        int suma = 0;
        for (int i = 1; i <= N; i++)
        {
            if (EsPrimo(i))
            {
                suma += i;
            }
        }
        return suma;
    }

    static void Main()
    {
        Console.Write("Ingrese el número límite: ");
        if (!int.TryParse(Console.ReadLine(), out int N) || N < 1)
        {
            Console.WriteLine("Número inválido. Debe ser un entero positivo.");
            return;
        }

        Console.Write("Ingrese la cantidad de hilos (M): ");
        if (!int.TryParse(Console.ReadLine(), out int M) || M < 1)
        {
            Console.WriteLine("Número de hilos inválido. Debe ser un entero positivo.");
            return;
        }

        if (N < M) M = N;  

        // Ejecucion secuencial 
        Stopwatch swSecuencial = Stopwatch.StartNew();
        int sumaSecuencial = CalcularPrimosSecuencial(N);
        swSecuencial.Stop();
        Console.WriteLine($"\n[Secuencial] Suma de primos hasta {N}: {sumaSecuencial}");
        Console.WriteLine($"Tiempo de ejecución secuencial: {swSecuencial.ElapsedMilliseconds} ms\n");

        // Ejecucion concurrente
        sumaTotal = 0;
        int rango = N / M;
        Thread[] hilos = new Thread[M];

        Stopwatch swConcurrente = Stopwatch.StartNew();
        for (int i = 0; i < M; i++)
        {
            int inicio = i * rango + 1;
            int fin = (i == M - 1) ? N : inicio + rango - 1;
            hilos[i] = new Thread(CalcularPrimos);
            hilos[i].Start((inicio, fin));
        }

        foreach (var hilo in hilos)
        {
            hilo.Join();
        }
        swConcurrente.Stop();

        Console.WriteLine($"\n[Concurrente] Suma de primos hasta {N}: {sumaTotal}");
        Console.WriteLine($"Tiempo de ejecución concurrente con {M} hilos: {swConcurrente.ElapsedMilliseconds} ms");

        // Comparación de tiempos
        Console.WriteLine($"\nDiferencia de tiempo: {swSecuencial.ElapsedMilliseconds - swConcurrente.ElapsedMilliseconds} ms");
    }
}
