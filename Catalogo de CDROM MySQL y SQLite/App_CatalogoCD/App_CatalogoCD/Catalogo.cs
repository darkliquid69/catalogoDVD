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
		static public ushort contadorParaCodigo = 100;

        List<dvd> _catalogoDVD = new List<dvd>();
        DAOdvd dao = new DAOdvd();
		//DAOdvdSQLite dao = new DAOdvdSQLite();

        /// <summary>
        /// Constructor con acceso a la BD o en modo pruebas con datos ficticios
        /// </summary>
        /// <param name="tipo"></param>
        public Catalogo()
        {
            try
                {
                    if (dao.Conectar())
                        Console.WriteLine("Conexión con éxito a la BD");
					else 
						Console.WriteLine("No se puede conectar a la BD");
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
				this.LeerDVD ();
        }

        public Catalogo(int n)
        {
            // Crea n objetos de tipo DVD
            AddEntradas(n);
        }

        /// <summary>
        /// Crea n objetos de tipo DVD para pruebas
        /// </summary>
        /// <param name="n">Cambidad de objetos a crear y añadir a la colección</param>
        void AddEntradas(int n)
        {
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                dvd unDVD = new dvd(contadorParaCodigo++, 
                    "Titulo_" + i.ToString(), 
                    "Artista_" +i.ToString(),
                    null,
                    "Compañia_" + i.ToString(), 
                    ((decimal) rnd.Next(5,20) / 100) * 100,
                    (ushort)rnd.Next(1900, 2016));
                
                _catalogoDVD.Add(unDVD);
            }
        }

        public void AddEntrada(string i)
		{
            Random rnd = new Random();
            dvd unDVD = new dvd(ushort.Parse(i),
                    "Titulo_" + i.ToString(),
                    "Artista_" + i.ToString(),
                    null,
                    "Compañia_" + i.ToString(),
                    ((decimal)rnd.Next(5, 20) / 100) * 100,
                    (ushort)rnd.Next(1900, 2016));

            _catalogoDVD.Add(unDVD);
            Console.WriteLine("Resultado de la inserción: " + dao.Insertar(unDVD));
        }

        public void LeerDVD()
        {
            _catalogoDVD = dao.Seleccionar(null);
        }
		/// <summary>
		/// Obtiene de la tabla el DVD con el codigo dado
		/// </summary>
		/// <returns>The DV.</returns>
		/// <param name="codigo">Codigo.</param>
		public dvd LeerDVD(string codigo)
		{
			List<dvd> resultado = dao.Seleccionar (codigo);
			if (resultado.Count > 0)
				return resultado [0];
			else
				return null;
		}

        public List<dvd> CatalogoDVD
        {
            get { return _catalogoDVD; }
        }

        public int BorrarDVD(string codigo) {
            return dao.Borrar(codigo);
        }
        public int ActualizarDVD(dvd unDVD)
		{
			return dao.Actualizar (unDVD);
		}

        public string Xml
        {
            // Crea un string en formato XML con todos los CDROM
            get
            {
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
                foreach (dvd tmp in _catalogoDVD)
                {
                    xml += tmp.LeerXML();
                }
                return xml;
            }
        }

        public void XmlAFichero()
        {
            // Manda a fichero (c:/salida.xml), la lista de los CDROM en formato XML
            // Creo un flujo hacia el fichero
			String ruta = "c:\\basura\\salida.xml";
            FileStream fs1 = new FileStream(@ruta, FileMode.Create);
            // Guardo el dispositivo de salida (pantalla) en tmp
            TextWriter tmp = Console.Out;
            // Fichero de salida
            StreamWriter sw1 = new StreamWriter(fs1);
            Console.SetOut(sw1);
            // Esto se escribirá en el fichero
            Console.WriteLine(this.Xml);
            Console.SetOut(tmp);    // Reestablezco la salida estandar
            Console.WriteLine(@"Se ha creado el fichero: " + ruta);
            sw1.Close();
        }

		public void FiltrarPorPais() 
		{
			_catalogoDVD = dao.SeleccionarPorPais("US");
		}

		public override string ToString ()
		{
			String resultado = "\n";
			foreach (var item in _catalogoDVD) {
				resultado = resultado + item.ToString () + "\n";
			}
			return resultado;
		}
    }
}
