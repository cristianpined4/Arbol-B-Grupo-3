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
    }
}
