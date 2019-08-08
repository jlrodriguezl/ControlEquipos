using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlEquipos.Models;
using ControlEquipos.Views;

namespace ControlEquipos.Views
{
    public partial class Marcas : Form
    {
        public Marcas()
        {
            InitializeComponent();
        }
        #region
        public void CargarDatos()
        {
            using (controlequiposEntities db = new controlequiposEntities())
            {
                //select * from marcas m;
                //Utilizar LINQ para escribir consulta a la BD
                var LstMarcas = from m in db.marcas
                                select new
                                {
                                    id_marca = m.id_marca,
                                    nom_marca = m.nom_marca
                                };
                this.grdDatos.DataSource = LstMarcas.ToList();
            }
        }
        public marcas getSelectedItem()
        {
            marcas m = new marcas();
            try
            {
                m.id_marca = 
                    int.Parse(grdDatos.Rows[grdDatos.CurrentRow.Index].
                            Cells[0].Value.ToString());
                return m;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        private void Marcas_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (controlequiposEntities db = new controlequiposEntities())
            {
                //Consultar todas las marcas
                var lstMarcas = from m in db.marcas
                                select new
                                {
                                    id_marca = m.id_marca,
                                    nom_marca = m.nom_marca
                                };
                //Aplicar los filtros 
                if (!string.IsNullOrEmpty(this.txtNombre.Text))
                {
                    lstMarcas = lstMarcas.Where(m =>
                                    m.nom_marca.Contains(this.txtNombre.Text));
                }
                //Mapear la lista con los filtros al Datagrid
                this.grdDatos.DataSource = lstMarcas.ToList();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            GestionMarcas gestionMarcas = new GestionMarcas(null);
            gestionMarcas.ShowDialog();
            //Actualizar DataGrid
            this.CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            //Obtener los datos que se seleccionaron en el DataGrid
            marcas m = getSelectedItem();
            //Hubo selección?
            if(m != null)
            {
                //Inicializar formulario de edición
                GestionMarcas gestionMarcas = new GestionMarcas(m.id_marca);
                //Abrir el formulario
                gestionMarcas.ShowDialog();
                this.CargarDatos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //Obtener el registro que se va a eliminar
            marcas m = getSelectedItem();
            //Hubo selección?
            if (m != null)
            {
                //Solicitar confirmación para eliminar
                if(MessageBox.Show("Está seguro de eliminar este registro?",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    using (controlequiposEntities db = new controlequiposEntities())
                    {
                        marcas marcaEliminar = db.marcas.Find(m.id_marca);
                        db.marcas.Remove(marcaEliminar);
                        db.SaveChanges();
                    }
                }
                this.CargarDatos();
            }
        }
    }
}
