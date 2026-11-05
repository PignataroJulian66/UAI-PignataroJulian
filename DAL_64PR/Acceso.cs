using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_64PR
{
    /*
    Esto fue sacado de un proyecto desarrollado en desarrollo y arquitectura
    de software, junto a jeremias gomez (comision de los miercoles a la mañana)
    */
    public class Acceso
    {
        private static Acceso _instancia;
        protected SqlConnection conexion = null;

        ///[INSTALADOR v2] Cadenas fijas originales: se usan como respaldo si el config no trae la entrada
        ///(o si todavia tiene el marcador #SQLINSTANCE# sin reemplazar), asi el comportamiento previo no cambia.
        private const string CadenaFijaBD = @"Data Source=.;Initial Catalog=BD_64PR;Integrated Security=True;TrustServerCertificate=True";
        private const string CadenaFijaMaster = @"Data Source=.;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
        private const string MarcadorSinConfigurar = "#SQLINSTANCE#";
        SqlCommand comando = new SqlCommand();
        private Acceso()
        {
            conexion = new SqlConnection();
        }

        public static Acceso Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Acceso();
                }
                return _instancia;
            }
        }

        public void conectar()
        {
            //DESKTOP-1R961GN
            //JULIÁN
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    //conexion.ConnectionString = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;
                    // [INSTALADOR v2] código original:
                    // conexion.ConnectionString = @"Data Source=.;Initial Catalog=BD_64PR;Integrated Security=True;TrustServerCertificate=True";
                    conexion.ConnectionString = ObtenerCadena("ConexionBD", CadenaFijaBD);
                    conexion.Open();
                    Console.WriteLine("Conexión exitosa");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de conexión" + ex.Message);
            }
        }

        public void desconectar()
        {
            try
            {
                conexion.Close();
                Console.WriteLine("Desconexión exitosa.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al desconectar: " + ex.Message);
            }
        }

        public SqlTransaction IniciarTransaccion()
        {
            conectar();
            return conexion.BeginTransaction();
        }

        public void ConfirmarTransaccion(SqlTransaction tx)
        {
            try
            {
                if (tx != null)
                {
                    tx.Commit();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al confirmar la transacción: " + ex.Message);
            }
            finally
            {
                desconectar();
            }
        }

        public void CancelarTransaccion(SqlTransaction tx)
        {
            try
            {
                if (tx != null)
                {
                    tx.Rollback();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al cancelar la transacción: " + ex.Message);
            }
            finally
            {
                //desconectar();
            }
        }

        public int escribirQuery(string query, SqlParameter[] parametro, CommandType tipoComando = CommandType.Text)
        {
            SqlTransaction tx = null;
            int filasAfectadas = 0;
            try
            {
                comando.Parameters.Clear();
                tx = IniciarTransaccion();
                comando.Connection = tx.Connection; // Asignar la conexión de la transacción al comando
                comando.Transaction = tx; // Asignar la transacción al comando
                comando.CommandType = tipoComando;
                comando.CommandText = query;
                if (parametro != null)
                {
                    foreach (SqlParameter param in parametro)
                    {
                        comando.Parameters.AddWithValue(param.ParameterName, param.Value);
                    }
                    //comando.Parameters.AddRange(parametro);   (asi lo haciamos antes, que funciona igual, solo que los mandaba todos de una)
                }
                filasAfectadas = comando.ExecuteNonQuery();
                ConfirmarTransaccion(tx);
                return filasAfectadas;
            }
            catch
            {
                CancelarTransaccion(tx);
                throw;
            }
        }

        public DataTable leerQuery(string query, SqlParameter[] parametro, CommandType tipoComando = CommandType.Text)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adaptador = new SqlDataAdapter();
            conectar();
            comando.Connection = conexion;
            comando.CommandType = tipoComando;
            comando.CommandText = query;
            comando.Parameters.Clear();

            if (parametro != null)
            {
                foreach (SqlParameter param in parametro)
                {
                    comando.Parameters.AddWithValue(param.ParameterName, param.Value);
                }
                //comando.Parameters.AddRange(parametro);
            }
            adaptador.SelectCommand = comando;
            adaptador.Fill(dt);
            desconectar();
            return dt;
        }

        public object leerEscalar(string query, SqlParameter[] parametro, CommandType tipoComando = CommandType.Text)
        {
            object resultado = null;
            try
            {
                conectar();
                comando.Connection = conexion;
                comando.CommandType = tipoComando;
                comando.CommandText = query;

                if (parametro != null)
                {
                    comando.Parameters.Clear();
                    foreach (SqlParameter param in parametro)
                    {
                        comando.Parameters.AddWithValue(param.ParameterName, param.Value);
                    }
                    //comando.Parameters.AddRange(parametro);
                }
                resultado = comando.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al ejecutar ExecuteScalar: " + ex.Message);
            }
            finally
            {
                comando.Parameters.Clear();
                desconectar();
            }
            return resultado;
        }
        public int EjecutarComandoSinTransaccion(string query, SqlParameter[] parametros, int timeoutSegundos = 300)
        {
            ///este metodo es necesario para el backup, ya que no pueden ejecutarse en una TX de usuario
            conectar();
            int filasAfectadas = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.CommandTimeout = timeoutSegundos; /// el backup puede tardar mas que el default (30s)
                    if (parametros != null)
                    {
                        foreach (SqlParameter p in parametros)
                            cmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                    }
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                desconectar();
            }
            return filasAfectadas;
        }
        public int EjecutarComandoMaster(string query, SqlParameter[] parametros, int timeoutSegundos = 300)
        {
            ///Conexión independiente contra 'master'
            //string connStringMaster = ConfigurationManager.ConnectionStrings["ConexionMaster"].ConnectionString;
            // [INSTALADOR v2] código original:
            // string connStringMaster = @"Data Source=.;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
            string connStringMaster = ObtenerCadena("ConexionMaster", CadenaFijaMaster);
            using (SqlConnection conexionMaster = new SqlConnection(connStringMaster))
            {
                conexionMaster.Open();
                using (SqlCommand cmd = new SqlCommand(query, conexionMaster))
                {
                    cmd.CommandTimeout = timeoutSegundos;
                    if (parametros != null)
                    {
                        foreach (SqlParameter p in parametros)
                            cmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        ///[INSTALADOR v2] Igual que EjecutarComandoMaster pero devuelve un valor (ej: SELECT DB_ID(@nombre)).
        ///A diferencia de leerEscalar, NO se traga la excepcion: quien llama necesita saber si fallo la conexion.
        public object leerEscalarMaster(string query, SqlParameter[] parametros, int timeoutSegundos = 30)
        {
            string connStringMaster = ObtenerCadena("ConexionMaster", CadenaFijaMaster);
            using (SqlConnection conexionMaster = new SqlConnection(connStringMaster))
            {
                conexionMaster.Open();
                using (SqlCommand cmd = new SqlCommand(query, conexionMaster))
                {
                    cmd.CommandTimeout = timeoutSegundos;
                    if (parametros != null)
                    {
                        foreach (SqlParameter p in parametros)
                            cmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        ///[INSTALADOR v2] Nombre de la BD de trabajo (Initial Catalog de ConexionBD), para no hardcodear "BD_64PR"
        public string ObtenerNombreBaseDatos()
        {
            return new SqlConnectionStringBuilder(ObtenerCadena("ConexionBD", CadenaFijaBD)).InitialCatalog;
        }

        ///[INSTALADOR v2] Lee la cadena del config; si falta, esta vacia o sin configurar, usa la cadena fija original
        private static string ObtenerCadena(string nombre, string cadenaRespaldo)
        {
            ConnectionStringSettings entrada = ConfigurationManager.ConnectionStrings[nombre];
            if (entrada == null || string.IsNullOrWhiteSpace(entrada.ConnectionString)
                || entrada.ConnectionString.Contains(MarcadorSinConfigurar))
                return cadenaRespaldo;

            return entrada.ConnectionString;
        }
    }
}
