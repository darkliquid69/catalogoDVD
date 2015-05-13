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
		private delegate void Mensajero ( string texto );
		private event Mensajero EnviarLog;
		private enum Accion
		{
			CONECTAR		= 0,
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
			this.EnviarLog += c_Enviado;
		}

		private void OnEnviarLog(string texto)
		{
			if ( this.EnviarLog != null )
				this.EnviarLog ( texto );
		}

		private void Gui2_Load ( object sender, EventArgs e )
		{
			Inicializador ( );

		}

		private void Inicializador ( )
		{
			this.groupBox_Op.Text		= "Listado";
			this.groupBox_Listado.Text	= "Operaciones";
			this.groupBox_Visor.Text	= "Visor de Eventos";

			string[] nombres = new string [ ]{
				"Conectar",
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
			this.button1.Tag	= 0;
			this.button2.Tag	= 1;
			this.button3.Tag	= 2;
			this.button4.Tag	= 3;
			this.button5.Tag	= 4;
			this.button6.Tag	= 5;
			this.button7.Tag	= 6;
			this.button8.Tag	= 7;
			this.button9.Tag    = 8;

			//rtb
			this.richTextBox1.ScrollBars		= RichTextBoxScrollBars.Vertical;
			this.richTextBox1.TextChanged	   += richTextBox1_TextChanged;
		}

		void richTextBox1_TextChanged ( object sender, EventArgs e )
		{
			RichTextBox rtb = sender as RichTextBox;
			if ( rtb != null )
				rtb.ScrollToCaret ( );
		}

		void c_Enviado ( string texto )
		{
			this.richTextBox1.AppendText ( "["+DateTime.Now.ToShortTimeString ( )+"]"+ " "+
                texto  + "\r\n" );
		}


		///METODOS DE ANTIGUA UI:
		///
		private void Conectar ( )
		{
			this.c.Conectar ( );
		}

		private void VolcarAFichero ( )
		{
			c.XmlAFichero ( );
		}

		private void Borrar ( )
		{
			if ( this.listBox1.SelectedItem != null )
			{
				this.c.BorrarDVD ( ( ( dvd ) this.listBox1.SelectedItem ).Codigo.ToString ( ) );
				if ( MessageBox.Show ( this, "Seguro que quiere borrar el dvd :" + ( ( dvd ) this.listBox1.SelectedItem ).Titulo + "?", "Aviso", MessageBoxButtons.YesNo ) == System.Windows.Forms.DialogResult.Yes )
					this.OnEnviarLog ( string.Format ( "Se ha borrado el dvd {0}", ( ( dvd ) this.listBox1.SelectedItem ).Titulo ) );
			}
		}

		private void Anadir ( )
		{
			int res = -1;
			if ( this.listBox1.Items == null ||
				this.listBox1.Items.Count < 1 )
				return;

			IEnumerable<object> obj = this.listBox1.Items.Cast<object> ( ).OrderBy ( x => ( ( dvd ) x ).Codigo )
				.Reverse ( ).Take ( 1 );

			foreach ( object item in obj )			
				res = c.AddEntrada ( ( ( ( dvd ) item ).Codigo + 1 ).ToString ( ) );
			
			if ( res == 1 )
			{
				this.listBox1.Items.Clear ( );
				this.LeerTodos ( );
				this.OnEnviarLog ( "Se ha añadido un nuevo DVD" );
			}
		}

		private void XmlString ( )
		{
			MessageBox.Show ( this, c.Xml );
		}

		private void ListarPorPais ( )
		{
			this.OnEnviarLog ( "Esta función todavía no está implementada" );
		}

		private void LeerTodos ( )
		{
			this.listBox1.Items.Clear ( );
			this.c.LeerDVD ( );
			foreach ( var item in this.c.ToOBjectItr ( ) )
				this.listBox1.Items.Add ( item as dvd );
			this.OnEnviarLog ( "Se han cargado todos los DvD" );
		}

		private void Botones_Click ( object sender, EventArgs e )
		{
			Button boton = sender as Button;
			if ( boton != null )
			{
				try
				{
					switch ( ( Accion ) boton.Tag )
					{
						case Accion.CONECTAR: this.Conectar ( );
							break;
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
				catch ( Exception ex )
				{
					this.OnEnviarLog ( "ERROR: " + ex.Message );
				}
			}
		}
	}
}
