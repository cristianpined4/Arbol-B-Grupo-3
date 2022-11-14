namespace Arboles_B_Grupo_3.src
{
    internal class ArbolB
    {
        private static readonly int M = 5;//orden 
        private static readonly int MAX = M - 1;//máximo de clave en cada página
        //Ceiling Devuelve el valor integral más pequeño que es mayor o igual que el número decimal especificado.
        private static readonly int MIN = (int)Math.Ceiling((double)M / 2) - 1;

        private Nodo padre;//Nodo padre

        public ArbolB()//Contructor
        {
            padre = null;//nodo padre inicializa en null
        }

        public void Insert(int x)//ingresar una clave al arbol b
        {
            int iclave = 0;//clave inicial en cero
            Nodo iclaveDhijo = null;//clave hijos en null

            bool agregar = Insert(x, padre, ref iclave, ref iclaveDhijo);//metodo recursivo inserta la clave

            if (agregar)  /* Altura aumentada en uno, hay que crear nueva pagina en el arbol */
            {
                Nodo temp = new Nodo(M);// se crea una pagina temporal
                temp.hijo[0] = padre;// se coloca al padre actual como hijo (ya que las paginas rompen para arriba
                padre = temp;//el padre es el nuevo nodo
                //inicializa valores de nueva pagina
                padre.numclaves = 1;// nuevo padre tiene una clave inicialmente
                padre.clave[1] = iclave;//se asigana clave el valor de cero
                padre.hijo[1] = iclaveDhijo;// se asigna a hijo el valor de null
            }
        }

        //este metodo busca la clave dentro de una página en particular
        private bool BuscarNodo(int x, Nodo p, ref int n)
        {
            if (x < p.clave[1])// verifica si la clave x es menor que la clave 
            {
                n = 0;// las claves en la pagina son cero 
                return false;//retorna falso
            }

            n = p.numclaves;//n=al numero de claves de la pagina
            while ((x < p.clave[n]) && n > 1)//si la clave x es menor  que cualquier clave dentro de la pagina y mayor a uno 
                n--;//se decrementa n(lo que permitira recorrer todas las claves que se encuentre en un nodo o pagina del árbol b


            if (x == p.clave[n])//si la clave coincide 
                return true;//retorna true(se llama en el metodo buscar)
            else
                return false;//sino false
        }

        private bool Insert(int x, Nodo p, ref int iclave, ref Nodo iclaveDhijo)
        {
            if (p == null)  /*Primer caso base: clave no encontrada*/
            {
                iclave = x;
                iclaveDhijo = null;
                return true;
            }

            int n = 0;
            if (BuscarNodo(x, p, ref n) == true)// se busca la pagina indicada
            /*Segundo caso base: clave encontrada*/
            {
                MessageBox.Show("La clave ya existe en el árbol...", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false; /*No es necesario insertar la clave*/
            }

            bool bandera = Insert(x, p.hijo[n], ref iclave, ref iclaveDhijo);//se llama recursivamente cada nodo

            if (bandera == true)// si se inserto la pagina 
            {
                if (p.numclaves < MAX)// numero de claves debe ser menor a 4
                {
                    InsertarPorCambio(p, n, iclave, iclaveDhijo);//se inserta la clave en la pagina exitosamente
                    return false; /*Inserción en la página*/
                }
                else
                {   //en caso de ser mayor hay que romper la pagina
                    Separar(p, n, ref iclave, ref iclaveDhijo);
                    return true;  /*Inserción no terminada: clave mediana aún por insertar*/
                }
            }
            return false;
        }

        //permite agregar una clave agregandola a la página(aca se aplica cuando hay cupo en la página)
        //     5
        //134   6789
        //(nodo 1,3,2, nodo 3)
        private void InsertarPorCambio(Nodo p, int n, int iclave, Nodo iclaveDhijo)
        {
            //(i=3; 3>3;
            for (int i = p.numclaves; i > n; i--)//recorre la pagina de derecha a izquierda
            {
                //p.clave[4]=p.clave[3]
                //
                p.clave[i + 1] = p.clave[i];//mueve las claves a la izquierda
                p.hijo[i + 1] = p.hijo[i];
            }
            //
            p.clave[n + 1] = iclave;//ingresa la nueva clave
            p.hijo[n + 1] = iclaveDhijo;
            p.numclaves++;
        }

        private void Separar(Nodo p, int n, ref int iclave, ref Nodo iclaveDhijo)
        {
            int i, j;
            int ultimaclave;
            Nodo ultimohijo;

            if (n == MAX)// si ya hay 4 claves en la página
            {
                ultimaclave = iclave;  //ultima clave= clave a insertar
                ultimohijo = iclaveDhijo;//ultimo hijo=
            }
            else
            {// si es menor a 4
                ultimaclave = p.clave[MAX];
                ultimohijo = p.hijo[MAX];

                for (i = p.numclaves - 1; i > n; i--)//se recorre la pagina
                {
                    p.clave[i + 1] = p.clave[i];//se reordenan las claves en la pagina
                    p.hijo[i + 1] = p.hijo[i];//se reordenan las paginas hijas
                }

                p.clave[i + 1] = iclave;
                p.hijo[i + 1] = iclaveDhijo;
            }
            //se busca la clave central
            int d = (M + 1) / 2;// 3
            int medianclave = p.clave[d];// se establece la clave mediana
            Nodo nuevoNodo = new Nodo(M);//se crea una nueva pagina o nodo
            nuevoNodo.numclaves = M - d;//se establece el numero de claves que cben en el nodo 5-3=2

            nuevoNodo.hijo[0] = p.hijo[d];//

            for (i = 1, j = d + 1; j <= MAX; i++, j++)
            {
                nuevoNodo.clave[i] = p.clave[j];//se asigana clave a nueva pagina
                nuevoNodo.hijo[i] = p.hijo[j];
            }
            nuevoNodo.clave[i] = ultimaclave;
            nuevoNodo.hijo[i] = ultimohijo;

            p.numclaves = d - 1;

            iclave = medianclave;
            iclaveDhijo = nuevoNodo;
        }
        public void Borrar(int x)
        {
            if (padre == null)
            {
                MessageBox.Show("El árbol está vacío...", "ERROR.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Borrar(x, padre);//borra nodo recursivamente

            if (padre != null && padre.numclaves == 0)
                /*La altura del árbol disminuyó en 1*/
                padre = padre.hijo[0];
        }

        private void Borrar(int x, Nodo p)
        {
            int n = 0;

            if (BuscarNodo(x, p, ref n) == true) /* clave x encontrada en Nodo p */
            {
                if (p.hijo[n] == null)     /* Nodo p es un Nodo hoja */
                {
                    BorrarByShift(p, n);
                    return;
                }
                else                    /* Nodo p es un Nodo sin hoja */
                {
                    Nodo s = p.hijo[n];
                    while (s.hijo[0] != null)
                        s = s.hijo[0];
                    p.clave[n] = s.clave[1];
                    Borrar(s.clave[1], p.hijo[n]);
                }
            }
            else /*clave x no encontrada en Nodo p */
            {
                if (p.hijo[n] == null) /* p es una hoja Nodo */
                {
                    MessageBox.Show("EL VALOR QUE DESEA ELIMINAR [" + x + "] NO EXISTE EN EL ARBOL.", "ERROR.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else  /* p es un Nodo que no es hoja */
                    Borrar(x, p.hijo[n]);
            }

            if (p.hijo[n].numclaves < MIN)
                Restaurar(p, n);
        }

        private void BorrarByShift(Nodo p, int n)
        {
            for (int i = n + 1; i <= p.numclaves; i++)
            {
                p.clave[i - 1] = p.clave[i];
                p.hijo[i - 1] = p.hijo[i];
            }
            p.numclaves--;
        }

        // Llamado cuando p.hijo[n] pasa a estar bajo flujo
        private void Restaurar(Nodo p, int n)
        {
            if (n != 0 && p.hijo[n - 1].numclaves > MIN)
                PrestadoIzquierdo(p, n);
            else if (n != p.numclaves && p.hijo[n + 1].numclaves > MIN)
                PrestadoDerecha(p, n);
            else
            {
                if (n != 0) //si hay una hermana izquierda
                    Combinar(p, n);   // *Combinar con hermana izquierda * /
                else
                    Combinar(p, n + 1);  /*Combinar con el hermano derecho*/
            }
        }

        private void PrestadoIzquierdo(Nodo p, int n)
        {
            Nodo bajoflujonodo = p.hijo[n];
            Nodo hermanoizquierdo = p.hijo[n - 1];

            bajoflujonodo.numclaves++;

            /*Desplazar todas las claves e hijosen en underflow Nodo una posición a la derecha*/
            for (int i = bajoflujonodo.numclaves; i > 0; i--)
            {
                bajoflujonodo.clave[i + 1] = bajoflujonodo.clave[i];
                bajoflujonodo.hijo[i + 1] = bajoflujonodo.hijo[i];
            }
            bajoflujonodo.hijo[1] = bajoflujonodo.hijo[0];

            /* Mueve la clave del separador del padre Nodo p a bajoflujonodo */
            bajoflujonodo.clave[1] = p.clave[n];

            /* Mueve la clave más a la derecha de Nodo hermanoizquierdo al padre Nodo p */
            p.clave[n] = hermanoizquierdo.clave[hermanoizquierdo.numclaves];

            /*El hijo más a la derecha de hermanoizquierdo se convierte en el hijo más a la izquierda de bajoflujonodo */
            bajoflujonodo.hijo[0] = hermanoizquierdo.hijo[hermanoizquierdo.numclaves];

            hermanoizquierdo.numclaves--;
        }

        private void PrestadoDerecha(Nodo p, int n)
        {
            Nodo bajoflujonodo = p.hijo[n];
            Nodo hermanoDerecho = p.hijo[n + 1];

            //Mueve la clave del separador del padre Nodo p a bajoflujonodo */
            bajoflujonodo.numclaves++;
            bajoflujonodo.clave[bajoflujonodo.numclaves] = p.clave[n + 1];

            /* El hijo más a la izquierda de hermanoDerecho se convierte en el hijo más a la derecha de underflowNode */
            bajoflujonodo.hijo[bajoflujonodo.numclaves] = hermanoDerecho.hijo[0];


            /*Mueve la clave más a la izquierda de hermanoDerecho al padre Nodo p */
            p.clave[n + 1] = hermanoDerecho.clave[1];
            hermanoDerecho.numclaves--;

            /* Cambia todas las claves e hijos de hermanoDerecho una posición a la izquierda */
            hermanoDerecho.hijo[0] = hermanoDerecho.hijo[1];
            for (int i = 1; i <= hermanoDerecho.numclaves; i++)
            {
                hermanoDerecho.clave[i] = hermanoDerecho.clave[i + 1];
                hermanoDerecho.hijo[i] = hermanoDerecho.hijo[i + 1];
            }
        }

        private void Combinar(Nodo p, int m)
        {
            Nodo NodoA = p.hijo[m - 1];
            Nodo NodoB = p.hijo[m];

            NodoA.numclaves++;

            /* Mueve la clave del separador del padre Nodo p a NodoA */
            NodoA.clave[NodoA.numclaves] = p.clave[m];


            int i;
            for (i = m; i < p.numclaves; i++)
            {
                p.clave[i] = p.clave[i + 1];
                p.hijo[i] = p.hijo[i + 1];
            }
            p.numclaves--;

            /* El hijo más a la izquierda de NodoB se convierte en el hijo más a la derecha de NodoA */
            NodoA.hijo[NodoA.numclaves] = NodoB.hijo[0];

            /* Inserta todas las claves e hijos de NodoB al final de NodoA */
            for (i = 1; i <= NodoB.numclaves; i++)
            {
                NodoA.numclaves++;
                NodoA.clave[NodoA.numclaves] = NodoB.clave[i];
                NodoA.hijo[NodoA.numclaves] = NodoB.hijo[i];
            }
        }

    }
}
