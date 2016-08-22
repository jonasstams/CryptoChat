using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encryptor.Models;

namespace Encryptor.Data
{
    public class MessageDAO
    {
        private readonly dbConnection conn;

        public MessageDAO()
        {
            conn = new dbConnection();
        }

        public DataTable GetAll()
        {
            var query = "SELECT * FROM Messages";
            return conn.executeSelectQuery(query);
        }
        /// <method>
        ///     Get User Email By Firstname or Lastname and return DataTable
        /// </method>
        public DataTable SearchIncomeMessages(int _id)
        {
            var query = "SELECT * FROM Messages WHERE ToUser = @Id ORDER BY CreateDate DESC";
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@Id", SqlDbType.Int);
            sqlParameters[0].Value = _id;
            return conn.executeSelectQuery(query, sqlParameters);
        }
        /// <method>
        ///     Get User Email By Firstname or Lastname and return DataTable
        /// </method>
        public DataTable SearchSentMessages(int _id)
        {
            var query = "SELECT * FROM Messages WHERE FromUser = @Id ORDER BY CreateDate DESC";
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@Id", SqlDbType.Int);
            sqlParameters[0].Value = Convert.ToString(_id);
            return conn.executeSelectQuery(query, sqlParameters);
        }


        /// <method>
        ///     Get User Email By Id and return DataTable
        /// </method>
        public DataTable SearchById(int _id)
        {
            var query = "SELECT * FROM Message where Id = @Id";
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@Id", SqlDbType.Int);
            sqlParameters[0].Value = _id;
            return conn.executeSelectQuery(query, sqlParameters);
        }

        public bool Create(Message _msg)
        {
            var query =
                "INSERT INTO Messages (FromUser, ToUser, CreateDate, File1, File2, File3, IV) VALUES (@FromUser, @ToUser, @CreateDate, @File1, @File2, @File3, @IV)";
            var sqlParameters = new SqlParameter[7];
            sqlParameters[0] = new SqlParameter("@FromUser", SqlDbType.Int);
            sqlParameters[0].Value = _msg.From.Id;
            sqlParameters[1] = new SqlParameter("@ToUser", SqlDbType.Int);
            sqlParameters[1].Value = _msg.To.Id;
            sqlParameters[2] = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            sqlParameters[2].Value = _msg.When;
            sqlParameters[3] = new SqlParameter("@File1", SqlDbType.VarBinary);
            sqlParameters[3].Value = _msg.File1;
            sqlParameters[4] = new SqlParameter("@File2", SqlDbType.VarBinary);
            sqlParameters[4].Value = _msg.File2;
            sqlParameters[5] = new SqlParameter("@File3", SqlDbType.VarBinary);
            sqlParameters[5].Value = _msg.File3;
            sqlParameters[6] = new SqlParameter("@IV", SqlDbType.VarBinary);
            sqlParameters[6].Value = _msg.IV;

            return conn.executeInsertQuery(query, sqlParameters);
        }
    }
}
