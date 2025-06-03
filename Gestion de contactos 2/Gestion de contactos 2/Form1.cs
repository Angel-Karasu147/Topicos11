using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gestion_de_contactos_2
{
    public partial class Form1 : Form
    {
        private Label lblTitulo;
        private TextBox txtNombre, txtTelefono, txtCorreo;
        private Button btnAgregar, btnEliminar, btnLimpiar;
        private ListBox lstContactos;
        private MenuStrip menuStrip;
        private ToolStripMenuItem menuArchivo, menuSalir, menuAcercaDe;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Gestión de Contactos";
            this.Size = new System.Drawing.Size(500, 500);

            menuStrip = new MenuStrip();
            menuArchivo = new ToolStripMenuItem("Archivo");
            menuSalir = new ToolStripMenuItem("Salir", null, MenuSalir_Click);
            menuAcercaDe = new ToolStripMenuItem("Acerca de", null, MenuAcercaDe_Click);
            menuArchivo.DropDownItems.Add(menuSalir);
            menuStrip.Items.Add(menuArchivo);
            menuStrip.Items.Add(menuAcercaDe);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            lblTitulo = new Label { Text = "Gestión de Contactos", AutoSize = true, Font = new System.Drawing.Font("Arial", 10), Top = 30, Left = 100 };
            this.Controls.Add(lblTitulo);

            txtNombre = new TextBox { Top = 70, Left = 50, Width = 300 };
            txtTelefono = new TextBox { Top = 100, Left = 50, Width = 300 };
            txtCorreo = new TextBox { Top = 130, Left = 50, Width = 300 };
            this.Controls.Add(txtNombre);
            this.Controls.Add(txtTelefono);
            this.Controls.Add(txtCorreo);

            SetCueBanner(txtNombre, "Nombre");
            SetCueBanner(txtTelefono, "Teléfono");
            SetCueBanner(txtCorreo, "Correo");

            btnAgregar = new Button { Text = "Agregar Contacto", Top = 170, Left = 50, Width = 140 };
            btnEliminar = new Button { Text = "Eliminar Contacto", Top = 170, Left = 210, Width = 140 };
            btnLimpiar = new Button { Text = "Limpiar Campos", Top = 210, Left = 130, Width = 140 };
            this.Controls.Add(btnAgregar);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(btnLimpiar);

            lstContactos = new ListBox { Top = 250, Left = 50, Width = 300, Height = 100 };
            this.Controls.Add(lstContactos);

            btnAgregar.Click += BtnAgregar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            lstContactos.Items.Add($"{txtNombre.Text} - {txtTelefono.Text} - {txtCorreo.Text}");
            MessageBox.Show("Contacto agregado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BtnLimpiar_Click(sender, e);
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (lstContactos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un contacto para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            lstContactos.Items.Remove(lstContactos.SelectedItem);
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
        }

        private void MenuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuAcercaDe_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gestión de Contactos \n Desarrolo de practica 1 hola profe ", "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);

        private void SetCueBanner(TextBox textBox, string cueText)
        {
            const uint EM_SETCUEBANNER = 0x1501;
            SendMessage(textBox.Handle, EM_SETCUEBANNER, (IntPtr)1, cueText);
        }
    }
}
