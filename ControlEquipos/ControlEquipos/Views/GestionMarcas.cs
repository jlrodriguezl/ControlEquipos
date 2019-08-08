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

namespace ControlEquipos.Views
{
    public partial class GestionMarcas : Form
    {
        ControlEquipos.Models.marcas oMarca = null;
        private int? idMarca;
        public GestionMarcas(int? idMarca = null)
        {
            //Inicializar el formulario (Dibujarlo)
            InitializeComponent();
            this.idMarca = idMarca;

            //Si idMarca no es nulo, es modo Edición
            if(idMarca != null)
            {
                //Cargar datos
                using (controlequiposEntities db = new controlequiposEntities())
                {
                    oMarca = db.marcas.Find(idMarca);
                    this.txtNombre.Text = oMarca.nom_marca;
                }
            }            
        }

        private void GestionMarcas_Load(object sender, EventArgs e)
        {
            this.txtNombre.Select();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validar que los datos obligatorios se hayan diligenciado
            if (string.IsNullOrEmpty(this.txtNombre.Text))
            {
                MessageBox.Show("Los campos marcados con (*) son obligatorios");
            }
            else
            {
                using (controlequiposEntities db = new controlequiposEntities())
                {
                    if(idMarca == null)
                    {
                        oMarca = new marcas();
                    }
                    oMarca.nom_marca = this.txtNombre.Text;

                    if(idMarca == null)
                    {
                        db.marcas.Add(oMarca);
                    }
                    else
                    {
                        db.Entry(oMarca).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    this.Close();
                }
            }
        }
    }
}
