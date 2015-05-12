using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;

namespace App_CatalogoCD
{
	public class DAOdvdSQLite
	{

		static string BD = "catalogo.db";

		public SqliteConnection conexion;

		public bool Conectar()
		{
			string cadenaConexion = "Data Source=" + BD + ";Version=3;" + "FailIfMissing=true;";
			try
			{
					conexion = new SqliteConnection(cadenaConexion);
					conexion.Open();
					return true;
			}
			catch (SqliteException ex)
			{
				throw new Exception("Error de conexión: " + ex.Data);
			}
		}

		public void Desconectar()
		{
			try
			{
				conexion.Close();
			}
			catch (SqliteException)
			{
				throw;
			}
		}

		public int Insertar(dvd unDVD)
		{
			string sql;

			if (unDVD != null)
			{
				sql = "insert into dvd (codigo, titulo, artista, pais, compania, precio, anio) " +
					" values (" + unDVD.Codigo + ",'" + unDVD.Titulo + "','" + unDVD.Artista + "','" +
					unDVD.Pais + "','" + unDVD.Compania + "'," +
					unDVD.Precio.ToString().Replace(',','.') + "," 
					+ unDVD.Anio + ")";
				//Console.WriteLine(sql);
				SqliteCommand cmd = new SqliteCommand(sql, conexion);
				return cmd.ExecuteNonQuery();
			}
			else
				return 0;
		}
		/// <summary>
		/// Realiza búsquedas de DVD en la tabla según la clave
		/// </summary>
		/// <param name="codigo">si es null, lee la tabla entero, de lo contrario solo el codigo indicado</param>
		/// <returns></returns>
		public List<dvd> Seleccionar(string codigo)
		{
			List<dvd> resultado = new List<dvd>();
			string sql;
			if (codigo == null)
				sql = "select codigo,titulo,artista,pais,compania,precio,anio from dvd";
			else
				sql = "select codigo,titulo,artista,pais,compania,precio,anio from dvd where codigo = " + codigo;

			SqliteCommand cmd = new SqliteCommand(sql, conexion);
			// construimos un datareader y ejecutamos el comando sql

			SqliteDataReader lector = cmd.ExecuteReader();
			//recuperamos los datos y volcamos en el resultado a devolver

			while (lector.Read())
			{
				dvd unDVD = new dvd();
				unDVD.Codigo = ushort.Parse(lector["codigo"].ToString());
				unDVD.Titulo = lector["titulo"].ToString();
				unDVD.Artista = lector["artista"].ToString();
				unDVD.Pais = lector["pais"].ToString();
				unDVD.Compania = lector["compania"].ToString();
				unDVD.Precio = decimal.Parse(lector["precio"].ToString());
				unDVD.Anio = ushort.Parse(lector["anio"].ToString());

				resultado.Add(unDVD);
			}
			lector.Close();
			return resultado;
		}
		/// <summary>
		/// Elimina de la tabla el código indicado
		/// </summary>
		/// <param name="codigo">Si es null, elimina la tabla completa</param>
		public int Borrar(string codigo)
		{
			string sql;
			if (codigo != null)
			{
				sql = "delete from dvd where codigo = " + codigo;
				SqliteCommand cmd = new SqliteCommand(sql, conexion);
				return cmd.ExecuteNonQuery();
			}
			else
				return 0;
		}

		public int Actualizar(dvd unDVD)
		{
			string sql;
			if (unDVD != null)
			{
				sql = "update dvd " + 
					"set titulo = '" + unDVD.Titulo + "'," +
					"artista='" + unDVD.Artista + "'," +
					"pais='" + unDVD.Pais + "'," +
					"compania='" + unDVD.Compania + "'," +
					"precio='" + unDVD.Precio + "'," +
					"anio='" + unDVD.Anio + "' " +
					"where codigo = " + unDVD.Codigo;
				//Console.WriteLine (sql);
				SqliteCommand cmd = new SqliteCommand(sql, conexion);
				try {
					cmd.ExecuteNonQuery();
					return 1;
				} catch (SqliteException) {
					return 0;
				}
			}
			else
				return 0;
		}
	}
}