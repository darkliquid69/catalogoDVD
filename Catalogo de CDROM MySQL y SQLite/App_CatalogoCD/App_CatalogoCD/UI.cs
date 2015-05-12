using System;

namespace App_CatalogoCD
{
	public class UI
	{
		static Catalogo c;

		public UI ()
		{
			c = new Catalogo ();
			PedirOpcion ();
		}
		static void Menu()
		{
			Console.WriteLine ("CATALOGO DE DVDs - Menú de opciones");
			Console.WriteLine ("===================================");
			Console.WriteLine ("(1) Leer todos los DVDs");
			Console.WriteLine ("(2) Leer todos los DVDs en XML");
			Console.WriteLine ("(3) Añadir un DVD con datos al azar");
			Console.WriteLine ("(4) Eliminar un DVD");
			Console.WriteLine ("(5) Modificar un DVD");
			Console.WriteLine ("(6) Volcar XML a fichero");
			Console.WriteLine ("(7) Listar DVD por pais");
			Console.WriteLine ("(q) Fin");
			Console.WriteLine ();
			Console.Write ("Opcion? "); 
		}
		static void BorrarDVD() 
		{
			Console.WriteLine ();
			Console.WriteLine("Código de DVD para eliminar?");
			string codigo = Console.ReadLine();
			Console.WriteLine("Borrando el DVD con código: " + codigo);
			if (c.BorrarDVD(codigo) != 0)
				Console.WriteLine("Operación realizada");
			else
				Console.WriteLine("Registro no encontrado");
		}
		static void AnadirDVD()
		{
			Console.WriteLine ();
			Console.WriteLine("Código de DVD para añadir?");
			string codigo = Console.ReadLine();
			Console.WriteLine("Añadiendo el DVD con código: " + codigo);
			c.AddEntrada (codigo);
		}

		static void PedirCampos (dvd registro) 
		{
			string entrada;

			Console.Write("Titulo: [" + registro.Titulo + "]: ");
			entrada = Console.ReadLine();
			if (entrada != "")
				registro.Titulo = entrada;
			Console.Write("Artista: [" + registro.Artista + "]: ");
			entrada = Console.ReadLine();
			if (entrada != "")
				registro.Artista = entrada;
			Console.Write("Pais: [" + registro.Pais + "]: ");
			entrada = Console.ReadLine();
			if (entrada != "")
				registro.Pais = entrada;
			Console.Write("Compañía: [" + registro.Compania + "]: ");
			entrada = Console.ReadLine();
			if (entrada != "")
				registro.Compania = entrada;
			Console.Write("Precio: [" + registro.Precio + "]: ");
			entrada = Console.ReadLine();
			if (entrada != "")
				registro.Precio = Decimal.Parse(entrada);
			Console.Write("Año: [" + registro.Anio + "]: ");
			entrada = Console.ReadLine();
			if (entrada != "")
				registro.Anio = ushort.Parse(entrada);
		}

		static void ActualizarDVD()
		{
			dvd registro;

			Console.WriteLine("\nCódigo de DVD para modificar?");
			string codigo = Console.ReadLine();
		
			registro = c.LeerDVD (codigo);
			if (registro != null) {
				Console.WriteLine (registro.ToString ());
			
				PedirCampos (registro);

				Console.WriteLine (registro.ToString());
				Console.Write ("Es correcto? [s/n] ");
				if (Console.ReadKey().Key.Equals(ConsoleKey.S)) {
					if (c.ActualizarDVD(registro) == 1)
						Console.WriteLine ("\nRegistro actualizado correctamente");
					else
						Console.WriteLine ("Error al actualizar el registro");
				}
			} else
				Console.WriteLine("Disco no encontrado");
		}
		static void VolcarAFichero()
		{
			c.XmlAFichero();
		}

		static void PedirOpcion()
		{
			ConsoleKeyInfo opcion;
			do {
				Console.WriteLine();
				Menu();
				opcion = Console.ReadKey();
				switch ((char) opcion.Key) {
				case '1':// Muestra, en pantalla, la lista de los CDROM en formato XML
					c.LeerDVD ();
					Console.WriteLine (c.ToString());
					break;
				case '2':// Muestra, en pantalla, la lista de los CDROM en formato XML
					c.LeerDVD ();
					Console.WriteLine (c.Xml);
					break;
				case '3': //Añadir
					AnadirDVD ();
					break;
				case '4': //Eliminar
					BorrarDVD ();
					break;
				case '5': //Modificar
					ActualizarDVD ();
					break;
				case '6': //Volcar a fichero
					VolcarAFichero();
					break;
				case '7': //Filtrar por pais
					c.FiltrarPorPais();
					Console.WriteLine (c.ToString());
					break;

				default:
						break;
				}
			} while  (opcion.Key != ConsoleKey.Q);
		}
	}
}

