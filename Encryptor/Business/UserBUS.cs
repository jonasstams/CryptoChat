using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Encryptor.Data;
using Encryptor.Models;

namespace Encryptor.Business
{
    public class UserBUS
    {
        private UserDAO _userDAO;

        /// <constructor>
        /// Constructor UserBUS
        /// </constructor>
        public UserBUS()
        {
            _userDAO = new UserDAO();
        }

        public List<string> GetUsernameList()
        {
            List<string> userList = new List<string>();
            DataTable dataTable = _userDAO.GetAll();
            foreach (DataRow dr in dataTable.Rows)
            {
                userList.Add(dr["Username"].ToString());
            }
            return userList;
        }
        /// <method>
        /// Get User by Username
        /// </method>
        public User getUserByUserName(string _username)
        {
            User User = new User();
            DataTable dataTable = new DataTable();

            dataTable = _userDAO.SearchByName(_username);

            foreach (DataRow dr in dataTable.Rows)
            {
                User.Id = Int32.Parse(dr["Id"].ToString());
                User.Username = dr["Username"].ToString();
                User.Password = dr["Password"].ToString();
                User.PuK = (byte[])dr["PuK"];
                User.PvK = (byte[])dr["PvK"];
                User.Role = dr["Role"].ToString();
            }
            return User;
        }

        /// <method>
        /// Get User By Id 
        /// </method>
        public User getUserById(int _id)
        {
            User User = new User();
            DataTable dataTable = new DataTable();
            dataTable = _userDAO.SearchById(_id);

            foreach (DataRow dr in dataTable.Rows)
            {
                User.Id = Int32.Parse(dr["Id"].ToString());
                User.Username = dr["Username"].ToString();
                User.Password = dr["Password"].ToString();
                User.PuK = (byte[]) dr["PuK"];
                User.PvK = (byte[]) dr["PvK"];
                User.Role = dr["Role"].ToString();
            }
            return User;
        }

        public bool Create(User _user)
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            _user.PuK = rsa.ExportCspBlob(false);
            _user.PvK = rsa.ExportCspBlob(true);
            if (_userDAO.Create(_user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UsernameExist(string _username)
        {
            DataTable dataTable = _userDAO.SearchByName(_username);
            if (dataTable.Rows.Count > 0)
            {
                return true;
            }
            {
                return false;
            }
        }
    }
}
