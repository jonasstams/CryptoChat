using System;
using System.Data;
using System.Data.SqlClient;
using Encryptor.Models;

namespace Encryptor.Data
{
    public class UserDAO
    {
        private readonly dbConnection conn;

        /// <constructor>
        ///     Constructor UserDAO
        /// </constructor>
        public UserDAO()
        {
            conn = new dbConnection();
        }

        public DataTable GetAll()
        {
            var query = "SELECT * FROM Users";
            return conn.executeSelectQuery(query);
        }
        /// <method>
        ///     Get User Email By Firstname or Lastname and return DataTable
        /// </method>
        public DataTable SearchByName(string _username)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username";
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@Username", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(_username);
            return conn.executeSelectQuery(query, sqlParameters);
        }

        /// <method>
        ///     Get User Email By Id and return DataTable
        /// </method>
        public DataTable SearchById(int _id)
        {
            var query = "SELECT * FROM Users where Id = @Id";
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@Id", SqlDbType.Int);
            sqlParameters[0].Value = _id;
            return conn.executeSelectQuery(query, sqlParameters);
        }

        public bool Create(User _user)
        {
            var query =
                "INSERT INTO Users (Username, Password, Role, PvK, PuK) VALUES (@Username, @Password, @Role, @PvK, @PuK)";
            var sqlParameters = new SqlParameter[5];
            sqlParameters[0] = new SqlParameter("@Username", SqlDbType.VarChar);
            sqlParameters[0].Value = _user.Username;
            sqlParameters[1] = new SqlParameter("@Password", SqlDbType.VarChar);
            sqlParameters[1].Value = _user.Password;
            sqlParameters[2] = new SqlParameter("@Role", SqlDbType.VarChar);
            sqlParameters[2].Value = _user.Role;
            sqlParameters[3] = new SqlParameter("@PvK", SqlDbType.VarBinary);
            sqlParameters[3].Value = _user.PvK;
            sqlParameters[4] = new SqlParameter("@PuK", SqlDbType.VarBinary);
            sqlParameters[4].Value = _user.PuK;

            return conn.executeInsertQuery(query, sqlParameters);
        }

    }
}
