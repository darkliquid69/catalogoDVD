using System;
using System.Threading;

namespace App_CatalogoCD
{
	/// <summary>
	/// Información sobre los datos de un CD-Rom
	/// </summary>
	public class dvd
	{
		#region Constructores
		public dvd ( )
		{

		}
		public dvd ( ushort contador, string titulo, string artista, string pais, string compania, decimal precio, ushort anio )
		{
			this.Codigo = contador;
			this.Titulo = titulo;
			this.Artista = artista;
			if ( pais == null )
				this._pais = PaisAlAzar ( );
			else
				this.Pais = pais;
			this.Compania = compania;
			this.Precio = precio;
			this.Anio = anio;
		}
		#endregion

		#region Campos/Datos internos
		string[] paisesPermitidos = { "ES", "UK", "IT", "FR", "DE", "US", "NO" };

		int maxLongTit = 40;            // Máxima longitud para el título
		int maxLongArt = 30;            // Máxima longitud para el nombre del artista
		int maxLongCom = 40;            // Máxima longitud para el nombre de la compañia

		#endregion

		#region Campos para almacenar los datos
		int _codigo;         // Código del CD-ROM, entero, autoincremental
		string _titulo;      // Titulo del volumen. Max. 40 caracteres
		string _artista;     // Nombre del artista. Max. 30 caracteres
		string _pais;        // Solo admite las siglas de un pais permitido, en otro caso ?? etc
		string _compania;    // Nombre de la compañia. Máx. 40 carácteres
		decimal _precio;     // Precio del disco
		ushort _anio;        // Año de la publicación. Tiene que estar entre 1900 y 9999 incluidos
		// Si no se cumple se lanza una excepción
		#endregion

		#region Propiedades
		/// <summary>
		/// Es de solo lectura, porque el código no se puede modificar
		/// </summary>
		public int Codigo
		{
			get { return _codigo; }
			set { _codigo = value; }
		}
		public string Titulo
		{
			get { return _titulo; }
			set
			{
				if ( value.Length > maxLongTit )
					_titulo = value.Substring ( 0, maxLongTit - 1 );
				else
					_titulo = value;
			}
		}
		public string Artista
		{
			get { return _artista; }
			set
			{
				if ( value.Length > maxLongArt )
					_artista = value.Substring ( 0, maxLongArt - 1 );
				else
					_artista = value;
			}
		}
		public string Pais
		{
			get { return _pais; }
			set
			{
				if ( EsValidoPais ( value ) )
					_pais = value;
				else
					_pais = "??";
			}
		}
		public string Compania
		{
			get { return _compania; }
			set
			{
				if ( value.Length > maxLongCom )
					_compania = value.Substring ( 0, maxLongCom - 1 );
				else
					_compania = value;
			}
		}
		public decimal Precio
		{
			get { return _precio; }
			set { _precio = value; }
		}
		public ushort Anio
		{
			get { return _anio; }
			set
			{
				if ( value >= 1900 && value <= 9999 )
					_anio = value;
				else
					throw new Exception ( "El valor del año no es correcto. Tiene que ser desde 1900-9999" );
			}
		}
		#endregion

		#region Métodos públicos


		/// <summary>
		/// Crea un string con formato XAML y los datos del objeto
		/// </summary>
		/// <returns></returns>
		public string LeerXML ( )
		{
			string xml = string.Empty;
			string nl = "\r\n";
			string tab = "\t";
			xml += "<cd codigo=" + "'" + _codigo + "'>"+nl;
			xml += tab + "<titulo>"+_titulo+"</titulo>"+nl;
			xml += tab + "<artista>" + _artista + "</artista>" + nl;
			xml += tab + "<pais>" + _pais + "</pais>" + nl;
			xml += tab + "<compania>" + _compania + "</compania>" + nl;
			xml += tab + "<precio>" + _precio + "</precio>" + nl;
			xml += tab + "<anio>" + _anio + "</anio>" + nl;
			xml += "<cd>"+nl;
			return xml;
		}
		public string PaisAlAzar ( )
		{
			Thread.Sleep ( 30 ); //para conseguir un mayor grado de aleatoriedad, sino se repiten los mismos
			return paisesPermitidos [ new Random ( ).Next ( paisesPermitidos.Length ) ];
		}
		#endregion

		#region Métodos sobreescritos
		public override string ToString ( )
		{
			return _codigo.ToString ( ) + ", " + _titulo + ", " + _artista + ", " 
                + _pais + ", " + _compania + ", " + _precio + ", " + _anio;
		}
		#endregion

		#region Métodos Privados
		/// <summary>
		/// Comprueba si el pais del campo _pais está en la lista de los permitidos
		/// </summary>
		/// <param name="pais">Siglas del pais a comprobar</param>
		/// <returns>true, si el pais está permitido o false en otro caso</returns>
		private bool EsValidoPais ( string pais )
		{
			foreach ( string tmp in paisesPermitidos )
				if ( tmp == pais )
					return true;
			return false;
		}
		#endregion
	}
}
