using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AccesoBaseDatos1
{
    public partial class Form1 : Form
    {
        private string Servidor = "KARASU\\SQLEXPRESS";
        private string Basedatos = "ESCOLAR";

        public Form1()
        {
            InitializeComponent();
        }

        private string ObtenerCadenaConexion()
        {
            return $"Server={Servidor};Database={Basedatos};Trusted_Connection=True;";
        }

        private void llenarGrid()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ObtenerCadenaConexion()))
                {
                    conn.Open();
                    string sql = "SELECT * FROM Alumnos";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvAlumnos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoControl.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtCarrera.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios");
                return;
            }

            try
            {
                string sql = "INSERT INTO Alumnos VALUES (@NoControl, @Nombre, @Carrera)";

                using (SqlConnection conn = new SqlConnection(ObtenerCadenaConexion()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@NoControl", txtNoControl.Text);
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Carrera", txtCarrera.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registro insertado correctamente");
                llenarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar: " + ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (dgvAlumnos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para borrar");
                return;
            }

            string noControl = dgvAlumnos.SelectedRows[0].Cells["NoControl"].Value.ToString();

            try
            {
                string sql = "DELETE FROM Alumnos WHERE NoControl = @NoControl";

                using (SqlConnection conn = new SqlConnection(ObtenerCadenaConexion()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@NoControl", noControl);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registro borrado correctamente");
                llenarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al borrar: " + ex.Message);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void dgvAlumnos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAlumnos.Rows[e.RowIndex];
                txtNoControl.Text = row.Cells["NoControl"].Value.ToString();
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtCarrera.Text = row.Cells["carrera"].Value.ToString();
            }
        }
    }
}