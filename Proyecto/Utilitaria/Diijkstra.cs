using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Utilitaria
{
    public class Diijkstra
    {
        public class DijkstraClass
        {
            private int rango = 0;
            private int[,] L;
            // matriz de adyacencia
            private int[] C;
            // arreglo de nodos 
            public int[] D;
            // arreglo de distancias
            private int trango = 0;

            public DijkstraClass(int paramRango, int[,] paramArreglo,int posicion)
            {
                L = new int[paramRango, paramRango];
                C = new int[paramRango];
                D = new int[paramRango];
                rango = paramRango;


                for (int i = 0; i < rango; i++)
                {
                    for (int j = 0; j < rango; j++)
                    {
                        L[i, j] = paramArreglo[i, j];
                    }
                }

                for (int i = 0; i < rango; i++)
                {
                    C[i] = i;
                }

                C[0] = -1;
                for (int i = 1; i < rango; i++)
                {
                    D[i] = L[posicion, i];
                }

            }

            public void SolDijkstra()
            {
                int minValor = Int32.MaxValue;
                int minNodo = 0;

                for (int i = 0; i < rango; i++)
                {
                    if (C[i] == -1)
                        continue;
                    if (D[i] > 0 && D[i] < minValor)
                    {
                        minValor = D[i];
                        minNodo = i;
                    }
                }

                C[minNodo] = -1;

                for (int i = 0; i < rango; i++)
                {
                    if (L[minNodo, i] < 0)
                        continue;
                    if (D[i] < 0)
                    {
                        D[i] = minValor + L[minNodo, i];
                        continue;
                    }

                    if ((D[minNodo] + L[minNodo, i]) < D[i])
                        D[i] = minValor + L[minNodo, i];

                }

            }

            public void CorrerDijkstra()
            {
                for (trango = 1; trango < rango; trango++)
                {
                    SolDijkstra();
                    Console.WriteLine("Iteracion No." + trango);
                    Console.WriteLine("Matriz de distancias: ");

                    for (int i = 0; i < rango; i++)
                        Console.Write(i + " ");

                    Console.WriteLine("");

                    for (int i = 0; i < rango; i++)
                        Console.Write(D[i] + " ");

                    Console.WriteLine(" ");
                    Console.WriteLine(" ");


                }
            }


        }
    }
}