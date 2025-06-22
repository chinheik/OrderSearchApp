using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace OrderSearchApp
{
    public static class Database
    {
        private const string ConnStr =
            "server=localhost;user id=root;password=;database=company_accounts;port=3306;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnStr);
        }

        public static DataTable SearchOrder(string orderID, string customerName)
        {
            var dt = new DataTable();
            string sql = @"
                SELECT o.oid AS 'Order ID',
                       o.customer_name AS 'Customer Name',
                       o.product AS 'Product',
                       o.qty AS 'Quantity',
                       o.unit AS 'Unit',
                       o.order_type AS 'Order Type',
                       o.total AS 'Total',
                       o.due_date AS 'Due Date'
                FROM orders o
                WHERE ((@oid = '' AND @custname <> '' AND o.customer_name LIKE @custname)
                       OR (@oid <> '' AND @custname <> '' AND o.oid = @oid AND o.customer_name LIKE @custname))";
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@oid", orderID);
            cmd.Parameters.AddWithValue("@custname", "%" + customerName + "%");
            using var adp = new MySqlDataAdapter(cmd);
            adp.Fill(dt);
            return dt;
        }
    }
}
