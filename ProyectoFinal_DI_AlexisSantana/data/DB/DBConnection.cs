using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
        public Cliente[] itemsClientes;
        public Compra[] itemsCompras;
        public CompraLinea[] itemsComprasLineas;

        private MySqlConnection connection;
        private string server, database, uid, password, port;
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
                        case "clientes":
                            ReadClientes();
                            break;
                        case "compras":
                            ReadCompras();
                            break;
                        case "compras_lineas":
                            ReadComprasLineas();
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
            ConData conData = JsonConvert.DeserializeObject<ConData>(System.IO.File.ReadAllText(@"dbconfig.json"));

            server = conData.server;
            database = conData.database;
            uid = conData.uid;
            password = conData.password;
            port = conData.port;
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

        //Comprueba si existe un producto cuyo id sea el pasado por parámetro
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

        //Similar al método anterior, pero este devuelve el objeto del producto encontrado
        public Producto GetProd(int? id)
        {
            LoadConnectionData();
            Producto p = null;
            if (OpenConnection())
            {
                try
                {
                    query = "SELECT * FROM inventario WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", id);
                    var reader = comando.ExecuteReader();
                    
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Producto(Convert.ToInt32(reader["id"]), Convert.ToString(reader["nombre_prod"]), Convert.ToString(reader["descripcion"]), 
                            Convert.ToInt32(reader["cantidad"]), Convert.ToDouble(reader["precio"]));
                        reader.Close();
                        CloseConnection();
                        return p;
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("ErrorLeerInventario" + ex.Message);
                }
            }
            CloseConnection();
            return p;
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

        public bool FilterCita(string param, string col)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    switch (col)
                    {
                        case "id":
                            query = "SELECT * FROM citas_demos WHERE id = ?id";
                            break;

                        default:
                            query = "SELECT * FROM citas_demos";
                            break;
                    }

                    
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", param);
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

        #region Métodos Clientes
        public void ReadClientes()
        {
            try
            {
                query = "SELECT * FROM clientes";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                int i = 0;

                dt = new DataTable();
                adapter.Fill(dt);
                intAffected = dt.Rows.Count;

                if (intAffected > 0)
                    itemsClientes = new Cliente[intAffected];
                else
                    itemsClientes = new Cliente[0];

                foreach (DataRow row in dt.Rows)
                {
                    itemsClientes[i] = new Cliente(Convert.ToInt32(row["id"]), Convert.ToString(row["nombre"]),
                        Convert.ToString(row["direccion"]), Convert.ToString(row["telefono"]));
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ErrorLeerClientes" + ex.Message);
            }
        }

        public bool InsertCliente(Cliente c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "INSERT INTO clientes VALUES(?id, ?nombre, ?direccion, ?telefono)";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?nombre", c.NombreCliente);
                    comando.Parameters.AddWithValue("?direccion", c.Direccion);
                    comando.Parameters.AddWithValue("?telefono", c.Telefono);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    UIGlobal.MainWindow.ShowMessage("Ya existe un cliente con ese ID", "error");
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool EditCliente(Cliente c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "UPDATE clientes SET nombre = ?nombre, direccion = ?direccion, telefono = ?telefono WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?nombre", c.NombreCliente);
                    comando.Parameters.AddWithValue("?direccion", c.Direccion);
                    comando.Parameters.AddWithValue("?telefono", c.Telefono);
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

        //Comprueba si existe un producto cuyo id sea el pasado por parámetro
        public bool SearchCli(int? id)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "SELECT * FROM clientes WHERE id = ?id";
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

        public bool DeleteCliente(Cliente c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "DELETE FROM clientes WHERE id = ?id";
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

        #region Métodos Compras
        public void ReadCompras()
        {
            try
            {
                query = "SELECT * FROM compras";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                int i = 0;

                dt = new DataTable();
                adapter.Fill(dt);
                intAffected = dt.Rows.Count;

                if (intAffected > 0)
                    itemsCompras = new Compra[intAffected];
                else
                    itemsCompras = new Compra[0];

                foreach (DataRow row in dt.Rows)
                {
                    itemsCompras[i] = new Compra(Convert.ToInt32(row["id"]), Convert.ToInt32(row["cliente"]), Convert.ToInt32(row["productos"]),
                        Convert.ToDateTime(row["fecha"]), (float)Convert.ToDouble(row["total"]));
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ErrorLeerCompras" + ex.Message);
            }
        }

        public bool InsertCompra(Compra c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "INSERT INTO compras VALUES(?id, ?cliente, null, ?fecha, null)";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?cliente", c.Cliente);
                    comando.Parameters.AddWithValue("?fecha", Convert.ToDateTime(c.Fecha));
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    UIGlobal.MainWindow.ShowMessage("Ya existe una compra con ese ID", "error");
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool EditCompra(Compra c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "UPDATE compras SET cliente = ?cliente, fecha = ?fecha WHERE id = ?id";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?cliente", c.Cliente);
                    comando.Parameters.AddWithValue("?fecha", Convert.ToDateTime(c.Fecha));
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

        public bool DeleteCompra(Compra c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "DELETE FROM compras WHERE id = ?id";
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

        #region Métodos Compras Lineas
        public void ReadComprasLineas()
        {
            try
            {
                query = "SELECT * FROM compras_lineas";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                int i = 0;

                dt = new DataTable();
                adapter.Fill(dt);
                intAffected = dt.Rows.Count;

                if (intAffected > 0)
                    itemsComprasLineas = new CompraLinea[intAffected];
                else
                    itemsComprasLineas = new CompraLinea[0];

                foreach (DataRow row in dt.Rows)
                {
                    itemsComprasLineas[i] = new CompraLinea(Convert.ToInt32(row["compra"]), Convert.ToInt32(row["producto"]),
                        Convert.ToInt32(row["cantidad"]), (float) Convert.ToDouble(row["precio"]));
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ErrorLeerComprasLineas" + ex.Message);
            }
        }

        public bool InsertCompraLinea(CompraLinea c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "INSERT INTO compras_lineas VALUES(?compra, ?producto, ?cantidad, null)";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?compra", c.Compra);
                    comando.Parameters.AddWithValue("?producto", c.Producto);
                    comando.Parameters.AddWithValue("?cantidad", c.Cantidad);
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    UIGlobal.MainWindow.ShowMessage("Ya existe una linea en esta Compra de ese producto, edita la existente.", "error");
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        public bool EditCompraLinea(CompraLinea c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "UPDATE compras_lineas SET producto = ?producto, cantidad = ?cantidad WHERE compra = ?compra AND producto = ?producto";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?producto", c.Producto);
                    comando.Parameters.AddWithValue("?cantidad", c.Cantidad);
                    comando.Parameters.AddWithValue("?compra", c.Compra);
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

        public bool DeleteCompraLinea(CompraLinea c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "DELETE FROM compras_lineas WHERE compra = ?compra AND producto = ?producto";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?compra", c.Compra);
                    comando.Parameters.AddWithValue("?producto", c.Producto);
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

        public bool SearchCompraLinea(CompraLinea c)
        {
            LoadConnectionData();
            if (OpenConnection())
            {
                try
                {
                    query = "SELECT * FROM compras_lineas WHERE compra = ?compra AND producto = ?producto";
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?compra", c.Compra);
                    comando.Parameters.AddWithValue("?producto", c.Producto);
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
    }
}

class ConData
{
    public string server, database, uid, password, port;

    public ConData(string server, string database, string uid, string password, string port)
    {
        this.server = server;
        this.database = database;
        this.uid = uid;
        this.password = password;
        this.port = port;
    }
}
