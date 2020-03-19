using MySql.Data.MySqlClient;
using ProyectoFinal_DI_AlexisSantana.model;
using System;
using System.Data;

namespace ProyectoFinal_DI_AlexisSantana.data.DB
{
    public sealed class DBConnection
    {
        private readonly static DBConnection _instance = new DBConnection();

        // Atributo de tipo dataset donde se almacena resultado de queries
        public DataSet DsResult { get; set; }
        private DataTable dt;
        private int intAffected;
        private string query;
        public Producto[] itemsInventario;
        public Cita[] itemsCitas;

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;
        private string connectionString;

        private DBConnection() { }

        /*
         * Crear instancia de la conexión con el patrón Singleton.
         */
        public static DBConnection Instance
        {
            get
            {
                return _instance;
            }
        }

        /*
         *  Crear la conexión con la BD.
         */
        public void ConnectDB(string option)
        {
            try
            {
                LoadConnectionData();
                if (OpenConnection())
                {
                    switch (option)
                    {
                        case "inventario":
                            ReadInventario();
                            break;
                        case "citas":
                            ReadCitas();
                            break;
                    }
                }
                CloseConnection();
            }
            catch (Exception)
            {
                UIGlobal.MainWindow.Close();
            }
        }

        /*
         * Cargar los datos de la conexión.
         */
        private void LoadConnectionData()
        {
            server = "localhost";
            database = "vrworld";
            uid = "vrworld";
            password = "vrworld";
            port = "3311";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" +
                                "PORT=" + port + ";" + "UID=" + uid + ";" + "PWD=" + password +
                                ";Convert Zero Datetime=true;CHARSET=utf8";
            connection = new MySqlConnection(connectionString);
        }

        /*
         * Abrir la conexión con la BD.
         */
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                    case 1042:
                        UIGlobal.MainWindow.ShowMessage("No se puede conectar a la BD", "error");
                        throw new Exception("Cannot connect to server.  Contact administrator");

                    case 1045:
                        UIGlobal.MainWindow.ShowMessage("Usuario/Contraseña erroneos", "error");
                        throw new Exception("Invalid username/password, please try again");
                }
                return false;
            }
        }

        /*
         * Cerrar conexión con la BD.
         */
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Métodos Inventario
        public void ReadInventario()
        {
            try
            {
                query = "SELECT * FROM inventario";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                int i = 0;

                dt = new DataTable();
                adapter.Fill(dt);
                intAffected = dt.Rows.Count;

                if (intAffected > 0)
                    itemsInventario = new Producto[intAffected];
                else
                    itemsInventario = new Producto[0];

                foreach (DataRow row in dt.Rows)
                {
                    itemsInventario[i] = new Producto(Convert.ToInt32(row["id"]), Convert.ToString(row["nombre_prod"]),
                        Convert.ToString(row["descripcion"]), Convert.ToInt32(row["cantidad"]), Convert.ToDouble(row["precio"]));
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ErrorLeerInventario" + ex.Message);
            }
        }

        public bool InsertInv(Producto i)
        { 
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "INSERT INTO inventario VALUES(?id, ?nombre_prod, ?descripcion, ?cantidad, ?precio)";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", i.Id);
                    comando.Parameters.AddWithValue("?nombre_prod", i.Nombre);
                    comando.Parameters.AddWithValue("?descripcion", i.Descripcion);
                    comando.Parameters.AddWithValue("?cantidad", i.Cantidad);
                    comando.Parameters.AddWithValue("?precio", i.Precio);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    UIGlobal.MainWindow.ShowMessage("Ya existe un producto con ese ID", "error");
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool EditInv(Producto i)
        { 
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "UPDATE inventario SET nombre_prod = ?nombre_prod, descripcion = ?descripcion, cantidad = ?cantidad, precio = ?precio WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", i.Id);
                    comando.Parameters.AddWithValue("?nombre_prod", i.Nombre);
                    comando.Parameters.AddWithValue("?descripcion", i.Descripcion);
                    comando.Parameters.AddWithValue("?cantidad", i.Cantidad);
                    comando.Parameters.AddWithValue("?precio", i.Precio);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool DeleteInv(Producto i)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "DELETE FROM inventario WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", i.Id);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool SearchInv(int? id)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "SELECT * FROM inventario WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", id);
                    if (comando.ExecuteReader().HasRows)
                    {
                        CloseConnection();
                        return true;
                    }
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
            CloseConnection();
            return false;
        }
        #endregion 

        #region Métodos Citas
        public void ReadCitas()
        {
            try
            {
                query = "SELECT * FROM citas_demos";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                int i = 0;

                dt = new DataTable();
                adapter.Fill(dt);
                intAffected = dt.Rows.Count;

                if (intAffected > 0)
                    itemsCitas = new Cita[intAffected];
                else
                    itemsCitas = new Cita[0];

                foreach (DataRow row in dt.Rows)
                {
                    itemsCitas[i] = new Cita(Convert.ToInt32(row["id"]), Convert.ToString(row["nombre_cliente"]),
                        Convert.ToInt32(row["producto"]), Convert.ToDateTime(row["fecha"]));
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ErrorLeerCitasDemos" + ex.Message);
            }
        }

        public bool InsertCita(Cita c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "INSERT INTO citas_demos VALUES(?id, ?nombre_cliente, ?fecha, ?prod_id)";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?nombre_cliente", c.NombreCliente);
                    comando.Parameters.AddWithValue("?fecha", Convert.ToDateTime(c.Fecha));
                    comando.Parameters.AddWithValue("?prod_id", c.Producto);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    UIGlobal.MainWindow.ShowMessage("Ya existe una cita con ese ID", "error");
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool EditCita(Cita c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "UPDATE citas_demos SET nombre_cliente = ?nombre_cliente, fecha = ?fecha, producto = ?prod_id WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?nombre_cliente", c.NombreCliente);
                    comando.Parameters.AddWithValue("?fecha", Convert.ToDateTime(c.Fecha));
                    comando.Parameters.AddWithValue("?prod_id", c.Producto);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool DeleteCita(Cita c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "DELETE FROM citas_demos WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    return false;
                }
            }

            CloseConnection();
            return true;
        }
        #endregion
    }
}
