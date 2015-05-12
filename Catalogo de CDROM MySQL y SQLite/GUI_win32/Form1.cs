using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_win32
{
    public partial class Gui2 : Form
    {
        public Gui2()
        {
            InitializeComponent();
        }

        private void Gui2_Load(object sender, EventArgs e)
        {
            this.groupBox_Op.Text = "Operaciones";
            this.groupBox_Listado.Text = "Listado";
            string[] nombres = new string[]{
                "Leer todos los Dvd's",
                "Volcar todos los Dvd's en XML",
                "Añadir un dvd con datos al azar",
                "Eliminar un dvd",
                "Modificar un dvd",
                "Volcar xml a fichero",
                "Listar Dvd's por país",
                "(q) fin"
            };

            int i = 0;
            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                if (item is Button)
                    ((Button)item).Text = nombres[i++];
            }
        }
    }
}
