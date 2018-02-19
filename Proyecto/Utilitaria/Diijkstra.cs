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
                for (int i = 0; i < rango; i++)
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

    public class Dijkstra
    {
        public int[] D;

        private static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }


        public void DijkstraAlgo(int[,] graph, int source, int verticesCount)
        {
            D = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                D[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            D[source] = 0;

            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(D, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && D[u] != int.MaxValue && D[u] + graph[u, v] < D[v])
                        D[v] = D[u] + graph[u, v];
            }

            
            
        }
    }

}