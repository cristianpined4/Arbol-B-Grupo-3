using Arboles_B_Grupo_3.src;

namespace Arboles_B_Grupo_3
{
    public partial class Form1 : Form
    {

        Graphics g;//Permite dibujar en pantalla
        bool guardar = false;//Ayuda a saber si todos los txt que almacenan (cantidad de nodos por página y clave) están llenos
        ArbolB arbolB = new ArbolB();
        private int clave;


        public Form1()
        {
            InitializeComponent();
            lblError.Text = "";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtClave.Text != "")//Verifica si la caja de texto está llena
            {
                clave = int.Parse(txtClave.Text);//Almacena la clave a insertar
                if (clave > 0 && clave < 100)//Solo permite valores de 1 a 99
                {
                    this.Cursor = Cursors.AppStarting;
                    arbolB.Insert(clave);
                    guardar = true;//Pone en true para que se pueda pintar el la página
                    Refresh();//Refresca el formulario (Llama a la función que permite pintar la página)
                    
                    txtClave.Clear();//Limpia las cajas de texto
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Solo se permiten valores entre 1 y 99...", "ERROR.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Clear();
                }
            }
            else
            {
                //Si hay al menos una caja de texto que no tiene datos, muestra un mensaje de error
                MessageBox.Show("Por favor llene todos los campos del formulario...", "ERROR.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtClave.Focus();
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            ///<!----> VALIDACIONES DE DATOS (TAMBIEN PERMITE INSERTAR UNA CLAVE AL PRECIONAR [ENTER]) <!---->
            if (e.KeyChar == Convert.ToChar(Keys.Enter))//Si preciona [ENTER]
            {
                clave = int.Parse(txtClave.Text);//Almacena la clave a insertar
                if (clave > 0 && clave < 100)//Solo permite valores de 1 a 99
                {
                    this.Cursor = Cursors.AppStarting;//Cambia el estilo del cursor
                    arbolB.Insert(clave);
                    guardar = true;//Pone en true para que se pueda pintar el la página
                    Refresh();//Refresca el formulario (Llama a la función que permite pintar la página)
                    txtClave.Clear();//Limpia las cajas de texto
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Solo se permiten valores entre 1 y 99...", "ERROR.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Clear();
                }
            }
            else if (!(char.IsNumber(e.KeyChar)) && true != char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                lblError.ForeColor = Color.Red;
                lblError.Text = "X";
            }
            else
            {
                e.Handled = false;
                lblError.Text = "";
            }
            txtClave.Focus();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Limpia el formulario
            e.Graphics.Clear(this.BackColor);
            //Agrega propiedades para poder dibujar en el formulario
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g = e.Graphics;

            if (guardar)//Si ya se almacenó una nueva clave, da paso a pintar ó actualizar el árbol
            {
                ///<!----> ACÁ VA EL CÓDIGO PARA PINTAR O ACTUALIZAR EL ARBOL-B <!---->
                ///

                guardar = false;//Reinicia guardar a false para esperar una nueva clave
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtClave.Text != "")//Verifica si las cajas de texto están llenas
            {
                clave = int.Parse(txtClave.Text);
                if (clave > 0 && clave < 100)
                {
                    this.Cursor = Cursors.AppStarting;
                    arbolB.Borrar(clave);
                    guardar = true;//Pone en true para que se pueda pintar el la página
                    Refresh();//Refresca el formulario (Llama a la función que permite pintar la página)

                    txtClave.Clear();//Limpia las cajas de texto
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Solo se permiten valores entre 1 y 99...", "ERROR.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Clear();
                }
            }
            else
            {
                //Si hay al menos una caja de texto que no tiene datos, muestra un mensaje de error
                MessageBox.Show("Por favor llene todos los campos del formulario...", "ERROR.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtClave.Focus();
        }

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            guardar = true;
            Refresh();
            txtClave.Focus();
        }

        
    }
}