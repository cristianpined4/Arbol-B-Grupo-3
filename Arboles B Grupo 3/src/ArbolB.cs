namespace Arboles_B_Grupo_3.src
{
    internal class ArbolB
    {
        private static readonly int M = 5;//orden 
        private static readonly int MAX = M - 1;//máximo de clave en cada página
        //Ceiling Devuelve el valor integral más pequeño que es mayor o igual que el número decimal especificado.
        private static readonly int MIN = (int)Math.Ceiling((double)M / 2) - 1;
        private Rectangle[] arrayRect;//Arreglo de rectángulos que permiten dibujar el cuadro donde están las claves
        // Todos los archivos ira a una carpeta dentro del proyecto llamada Archivos
        public string[] path = new string[] { }; // Donde se guardaran las rutas de los archivos

        private Nodo padre;//Nodo padre

        public ArbolB()//Contructor
        {
            padre = null;//nodo padre inicializa en null
        }

        // Funcion para guardar las hojas en los archivos
        // Parametros:
        //    - Nodo padre
        //    - Un contador para indica el numero de hoja (PUEDE CAMBIAR ESTE PARAMETRO)
        public void guardarEnArchivo(Nodo padre, int archivoNum)
        {
            if (padre != null)
            {
                string text = "";
                for (int i = 1; i <= padre.numclaves; i++)
                {
                    text += padre.clave[i] + ","; // Recorremos las claves para que de guarden asi 1,2,3,
                }
                text = text.TrimEnd(','); // Se elimina el ultimo , para que quede 1,2,3
                string pathFile = "../../../Archivos/Hoja-" + archivoNum + ".txt"; // Se crea la direccion donde se guardad con el nombre hoja-$indice.txt
                //File.WriteAllText(pathFile, text); // Se guarda la informacion del texto y se crea el archivo es la direccion definida anteriomente //seleecc

                for (int i = 0; i <= padre.numclaves; i++)//Hace un recorrido en todas las claves de la página
                {
                    archivoNum++;//Iterador que permite nombrar el archivo según su número de página
                    guardarEnArchivo(padre.hijo[i], archivoNum);//Para cada clave se llama a la función y se pasa como parámetro la página hija
                }
            }
        }
        
        // Funcion para eliminar los archivos del directorio y array path
        public void borrarArchivos()
        {
            if (path.Length != 0) // Se elimina los archivos si hay algo en el array path
            {
                foreach (string fileName in path)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName); // Recorre el array y se elimina cada archivo
                    }
                }
                Array.Clear(path, 0, path.Length);// Se elimina el contenido del array path
            }
            else // Se busca eliminar directamente del diretorio de un solo(ESTO ES CUANDO INICIA Y NO HAY NADA ALMACENADO EN MEMORIA SE ELIMINA TODO DE UN INICIO,RUTA VACIA)
            {
                DirectoryInfo di = new DirectoryInfo("../../../Archivos"); // Se obtiene la info de la ruta
                FileInfo[] files = di.GetFiles(); // Se obtiene todos los archivos que hay en la ruta y se carga en un arreglo de archivos
                foreach (FileInfo file in files)
                {
                    file.Delete(); // Se recorre y se elimina cada archivo
                }
            }
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
        public bool BuscarNodo(int x, Nodo p, ref int n)
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

        /// <!----> FUNCIONES PARA DIBUJAR EL ARBOL <!---->

        public void Mostrar(Graphics grafo, Brush Relleno, Rectangle rect, Font fuente)//metodo  que llama a metodo mostrar esto es para encapsular mas.
        {
            Mostrar(padre, 0, grafo, Relleno, rect, fuente);
            //borrarArchivos();
            guardarEnArchivo(padre, 1);
        }

        private void Mostrar(Nodo p, int masX, Graphics g, Brush Relleno, Rectangle rect, Font fuente)// recibe nodo y espacios entre nodos o paginas
        {
            if (p != null)// si el nodo es diferente de nulo
            {
                arrayRect = new Rectangle[p.numclaves];//Arreglo de rectángulos que almacena las posiciones y tamaños de todos los nodos
                arrayRect[0] = rect;//Se almacena el primer rectángulo que se trae desde "form1" y a partir de él se debujarán los demás hacia la derecha

                int i;
                for (i = 0; i < p.numclaves; i++)// recorre las claves que hay en cada pagina
                {
                    ///----> CREA Y DIBUJA LOS RECTANGULOS DE LA PÁGINA ACTUAL
                    //A partir de la segunda iteración se empiezan a crear todos los rectángulos partiendo de las coordenadas y tamaños del primer rectángulo
                    if (i >= 1)
                    {
                        arrayRect[i] = new Rectangle(arrayRect[i - 1].X + arrayRect[i - 1].Width, arrayRect[i - 1].Y, arrayRect[i - 1].Width, arrayRect[i - 1].Height);
                    }

                    //Se dibuja el rectángulo
                    g.DrawRectangle(Pens.Black, arrayRect[i]);
                    //Se rellena de color
                    g.FillRectangle(Relleno, arrayRect[i]);

                    ///----> DIBUJA LA CLAVE EN MEDIO DE CADA RECTANGULO
                    if (p.clave[i + 1] < 10)//Si la clave a dibujar es menor que 10, cambia las coordenadas para que se muestre más en medio del cuadro
                    {
                        //Dibuja la clave
                        g.DrawString(p.clave[i + 1].ToString(), fuente, Brushes.Black, arrayRect[i].X + ((arrayRect[i].Width / 2) - 8), arrayRect[i].Y + ((arrayRect[i].Height / 2) - 11));
                    }
                    else//Si el dato es mayor o igual a 10, dado que son dos números y cubren más espacio, se cambian las coordenadas en
                        //las que se dibujará la clave
                    {

                        g.DrawString(p.clave[i + 1].ToString(), fuente, Brushes.Black, arrayRect[i].X + ((arrayRect[i].Width / 2) - 14), arrayRect[i].Y + ((arrayRect[i].Height / 2) - 11));
                    }

                }

                rect.Y += 100;//Aumenta en Y el rectángulo para que los demás se dibujem más abajo

                for (i = 0; i <= p.numclaves; i++)//este for recorre las paginas hijas para mostrar las claves
                {
                    if (p.numclaves > 1)//Caso en que hayan más de una clave en la hoja
                    {
                        if (i == 0)//Página derecha
                        {
                            rect.X -= 180;//Permite que las páginas hijas no estén desordenadas (Misma distancia entre página izquierda y derecha)
                            Mostrar(p.hijo[i], masX + 10, g, Relleno, rect, fuente);// muestra las claves de las  páginas hijas
                        }
                        else if (i == 1)//Página central
                        {
                            rect.X += 180;//Permite que las páginas hijas no estén desordenadas (Misma distancia entre página izquierda y derecha)
                            Mostrar(p.hijo[i], masX + 10, g, Relleno, rect, fuente);// muestra las claves de las  páginas hijas
                        }
                        else//Página izquierda
                        {
                            rect.X += 180;//Permite que las páginas hijas no estén desordenadas (Misma distancia entre página izquierda y derecha)
                            Mostrar(p.hijo[i], masX + 10, g, Relleno, rect, fuente);// muestra las claves de las  páginas hijas

                        }
                    }
                    else//En caso que no, que solo
                    {
                        if (i == 0)//Página derecha
                        {
                            rect.X -= 240;//Permite que las páginas hijas no estén desordenadas (Misma distancia entre página izquierda y derecha)
                            Mostrar(p.hijo[i], masX + 10, g, Relleno, rect, fuente);// muestra las claves de las  páginas hijas
                        }
                        else if (i == 1)//Página izquierda
                        {
                            rect.X += 450;//Permite que las páginas hijas no estén desordenadas (Misma distancia entre página izquierda y derecha)
                            Mostrar(p.hijo[i], masX + 10, g, Relleno, rect, fuente);// muestra las claves de las  páginas hijas
                        }
                    }
                }
            }
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

        public bool Buscar(int x)//metodo que busca  una clave entera en el árbol
        {
            if (Buscar(x, padre) == null)// de forma recursiva busca la clave en el árbol(aca se utiliza el metodo buscar que tiene dos parametros, es e qu esta bajo)
                return false;// si buscar retorna null significa que no se encontro el nodo
            return true;// si buscar retorna true significa que el nodo se encuentra en el árbol
        }

        private Nodo Buscar(int x, Nodo p)//este metodo es el que recorre el arbol y busca la clave 
        {
            if (p == null)       /*clave x no presente en el árbol*/
                return null;//retorna null

            int n = 0;// n es el primer nivel del arbol que corresponde al nodo padre
            if (BuscarNodo(x, p, ref n) == true) //busca la clave x en un nodo o pagina, se usa el metodo de abajo
                return p;// retorna la pagina o nodo donde se encontro la clave

            return Buscar(x, p.hijo[n]); /* Buscar en Nodo p.hijo[n] */
        }
        //este metodo busca la clave dentro de una página en particular

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
