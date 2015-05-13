using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App_CatalogoCD
{
	public partial class Gui2 : Form
	{
		private enum Accion
		{
			LEER_TODOS		= 1,
			XML_STRING		= 2,
			ANADIR_1		= 3,
			BORRAR			= 4,
			MOD	            = 5,
			VOLCAR_XML		= 6,
			POR_PAIS		= 7,
			SALIR			= 8
		};

		private Catalogo c;

		public Gui2 ( )
		{
			InitializeComponent ( );
			this.c = new Catalogo ( );
			this.c.Enviado += c_Enviado;
		}

		private void Gui2_Load ( object sender, EventArgs e )
		{
			Inicializador ( );

		}

		private void Inicializador ( )
		{
			this.groupBox_Op.Text = "Listado";
			this.groupBox_Listado.Text = "Operaciones";
            this.groupBox_Visor.Text = "Visor de Eventos";

			string[] nombres = new string [ ]{
                "Leer todos los Dvd's",
                "Volcar todos los Dvd's en XML",
                "Añadir un dvd con datos al azar",
                "Eliminar un dvd",
                "Modificar un dvd",
                "Volcar xml a fichero",
                "Listar Dvd's por país",
                "Salir"
            };

			int i = 0;
			foreach ( var item in this.flowLayoutPanel1.Controls )
			{
				if ( item is Button )
				{
					( ( Button ) item ).Text = nombres [ i++ ];
					( ( Button ) item ).Click += new EventHandler ( Botones_Click );
				}
			}

			//botones tags
			this.button1.Tag	= 1;
			this.button2.Tag	= 2;
			this.button3.Tag	= 3;
			this.button4.Tag	= 4;
			this.button5.Tag	= 5;
			this.button6.Tag	= 6;
			this.button7.Tag	= 7;
			this.button8.Tag	= 8;            
		}

        void c_Enviado(string texto)
        {
            this.richTextBox1.AppendText("["+DateTime.Now.ToShortTimeString()+"]"+
                texto);
        }


		///METODOS DE ANTIGUA UI:
		private void VolcarAFichero ( )
		{
			c.XmlAFichero ( );
		}

		private void Borrar ( )
		{
			if ( this.listBox1.SelectedItem != null )
			{
				this.c.BorrarDVD ( ( ( dvd ) this.listBox1.SelectedItem ).Codigo.ToString ( ) );
				//
			}
		}

		private void Anadir ( )
		{
			if ( this.listBox1.Items == null ||
				this.listBox1.Items.Count < 1 )
				return;

			dvd d = ( dvd ) this.listBox1.Items.Cast<object> ( ).OrderBy ( x => ( ( dvd ) x ).Codigo )
				.Reverse ( ).Take ( 1 );

			c.AddEntrada ( ( d.Codigo + 1 ).ToString ( ) );

		}

		private void XmlString ( )
		{
			MessageBox.Show ( this, c.Xml );
		}

		private void ListarPorPais ( )
		{
			this.c.FiltrarPorPais ( );
		}

		private void LeerTodos ( )
		{
			this.listBox1.Items.Clear ( );
			this.c.LeerDVD ( );
			foreach ( var item in this.c.ToOBjectItr ( ) )
				this.listBox1.Items.Add ( item as dvd );
		}

		private void Botones_Click ( object sender, EventArgs e )
		{
			Button boton = sender as Button;
			if ( boton != null )
			{
				switch ( ( Accion ) boton.Tag )
				{
					case Accion.LEER_TODOS: this.LeerTodos ( );
						break;
					case Accion.XML_STRING: this.XmlString ( );
						break;
					case Accion.ANADIR_1: this.Anadir ( );
						break;
					case Accion.BORRAR: this.Borrar ( );
						break;
					case Accion.MOD:
						break;
					case Accion.VOLCAR_XML: this.VolcarAFichero ( );
						break;
					case Accion.POR_PAIS: this.ListarPorPais ( );
						break;
					case Accion.SALIR: this.Close ( );
						break;
					default:
						break;
				}
			}
		}
	}
}
