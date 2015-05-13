using System;
using System.Collections.Generic;
using System.IO;

namespace App_CatalogoCD
{
	/// <summary>
	/// Contiene una coleccion de objetos de DVD
	/// </summary>
	class Catalogo
	{
		public delegate void Mensajero ( string texto );
		public event Mensajero Enviado;
		private string _estadoInicial;
		private bool _conectado;
		static public ushort contadorParaCodigo = 100;

		List<dvd> _catalogoDVD = new List<dvd> ( );
		//DAOdvd dao = new DAOdvd();
		DAOdvdSQLite dao = new DAOdvdSQLite ( );

		public void OnEnviado ( string texto )
		{
			if ( this.Enviado != null )
				this.Enviado ( texto );
		}

		/// <summary>
		/// Inicializa una instancia de catálogo
		/// </summary>
		/// <param name="tipo"></param>
		public Catalogo ( )
		{
			_conectado = false;
		}

		public void Conectar ( )
		{
			try
			{
				if ( _conectado )
				{
					_estadoInicial = "Ya existe una conexión abierta a la Base de datos!";
					return;
				}
				if ( dao.Conectar ( ) )
				{
					this._estadoInicial = "Conexión con éxito a la BD";
					_conectado = true;
				}
				else
				{
					this._estadoInicial = "No se puede conectar a la BD";
					return;
				}
				this.LeerDVD ( );
			}
			catch ( Exception e )
			{
				this._estadoInicial =  "ERROR: " + e.Message;
			}
			finally
			{
				this.OnEnviado ( this._estadoInicial );
			}
		}

		public Catalogo ( int n )
		{
			// Crea n objetos de tipo DVD
			AddEntradas ( n );
		}

		/// <summary>
		/// Crea n objetos de tipo DVD para pruebas
		/// </summary>
		/// <param name="n">Cambidad de objetos a crear y añadir a la colección</param>
		void AddEntradas ( int n )
		{
			Random rnd = new Random ( );
			for ( int i = 0; i < n; i++ )
			{
				dvd unDVD = new dvd ( contadorParaCodigo++,
					"Titulo_" + i.ToString ( ),
					"Artista_" +i.ToString ( ),
					null,
					"Compañia_" + i.ToString ( ),
					( ( decimal ) rnd.Next ( 5, 20 ) / 100 ) * 100,
					( ushort ) rnd.Next ( 1900, 2016 ) );

				_catalogoDVD.Add ( unDVD );
			}
		}

		public int AddEntrada ( string i )
		{
			Random rnd = new Random ( );
			dvd unDVD = new dvd ( ushort.Parse ( i ),
					"Titulo_" + i.ToString ( ),
					"Artista_" + i.ToString ( ),
					null,
					"Compañia_" + i.ToString ( ),
					( ( decimal ) rnd.Next ( 5, 20 ) / 100 ) * 100,
					( ushort ) rnd.Next ( 1900, 2016 ) );

			_catalogoDVD.Add ( unDVD );
			int res = -1;
			try
			{
				res = dao.Insertar ( unDVD );
			}
			catch ( Exception ex )
			{
				OnEnviado ( "Resultado de la inserción: " + ex.Data );
			}
			return res;
		}

		public void LeerDVD ( )
		{
			if ( _conectado )
				_catalogoDVD = dao.Seleccionar ( null );
			else this.OnEnviado ( "ERROR: No hay conexión a la Base de Datos!" );
		}
		/// <summary>
		/// Obtiene de la tabla el DVD con el codigo dado
		/// </summary>
		/// <returns>The DV.</returns>
		/// <param name="codigo">Codigo.</param>
		public dvd LeerDVD ( string codigo )
		{
			List<dvd> resultado = dao.Seleccionar ( codigo );
			if ( resultado.Count > 0 )
				return resultado [ 0 ];
			else
				return null;
		}

		public List<dvd> CatalogoDVD
		{
			get { return _catalogoDVD; }
		}

		public int BorrarDVD ( string codigo )
		{
			return dao.Borrar ( codigo );
		}
		public int ActualizarDVD ( dvd unDVD )
		{
			return dao.Actualizar ( unDVD );
		}

		public string Xml
		{
			// Crea un string en formato XML con todos los CDROM
			get
			{
				string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
				foreach ( dvd tmp in _catalogoDVD )
				{
					xml += tmp.LeerXML ( );
				}
				return xml;
			}
		}

		public void XmlAFichero ( )
		{
			// Manda a fichero (c:/salida.xml), la lista de los CDROM en formato XML
			// Creo un flujo hacia el fichero
			String ruta = "../../Dependencias/salida.xml";
			FileStream fs1 = new FileStream ( @ruta, FileMode.Create );
			// Guardo el dispositivo de salida (pantalla) en tmp
			TextWriter tmp = Console.Out;
			// Fichero de salida
			StreamWriter sw1 = new StreamWriter ( fs1 );
			Console.SetOut ( sw1 );
			// Esto se escribirá en el fichero
			Console.WriteLine ( this.Xml );
			Console.SetOut ( tmp );    // Reestablezco la salida estandar
			OnEnviado ( @"Se ha creado el fichero: " + Path.GetFullPath ( ruta ) );
			sw1.Close ( );
		}

		public void FiltrarPorPais ( )
		{
			//_catalogoDVD = dao.SeleccionarPorPais("US");
		}

		public override string ToString ( )
		{
			String resultado = "\n";
			foreach ( var item in _catalogoDVD )
			{
				resultado = resultado + item.ToString ( ) + "\n";
			}
			return resultado;
		}

		public IEnumerable<dvd> ToOBjectItr ( )
		{
			foreach ( dvd item in this._catalogoDVD )
				yield return item;
		}
	}
}
