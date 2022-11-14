using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arboles_B_Grupo_3.src
{
    internal class Nodo
    {
        public int[] clave;//Almacena las claves de cada página
        public Nodo[] hijo;//Almacena los hijos de cada nodo
        public int numclaves;//Contador que permite saber cuántas claves hay en una página

        //Método constructor con parámetros
        public Nodo(int tama)
        {
            numclaves = 0;
            clave = new int[tama];
            hijo = new Nodo[tama];
        }

        //Método constructor sin parámetros
        public Nodo()
        {

        }
    }
}
