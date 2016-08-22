using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Encryptor.Data;
using Encryptor.Models;

namespace Encryptor.Business
{
    public class MessageBUS
    {
        private MessageDAO _msgDAO;
        private UserBUS _userBus;

        /// <constructor>
        /// Constructor UserBUS
        /// </constructor>
        public MessageBUS()
        {
            _msgDAO = new MessageDAO();
            _userBus= new UserBUS();
            
        }


        /// <method>
        /// Get sent messages by user id
        /// </method>
        public List<Message> GetSentMessages(int _id)
        {
            var messageList = new List<Message>();
            Message message;
            DataTable dataTable = _msgDAO.SearchSentMessages(_id);


            foreach (DataRow dr in dataTable.Rows)
            {
                message = new Message();
                message.Id = Int32.Parse(dr["Id"].ToString());
                message.From = _userBus.getUserById(Int32.Parse(dr["FromUser"].ToString()));
                message.To = _userBus.getUserById(Int32.Parse(dr["ToUser"].ToString()));
                message.When = DateTime.Parse(dr["When"].ToString());
                message.File1 = (Byte[]) dr["File1"];
                message.File2 = (Byte[]) dr["File2"];
                message.File3 = (Byte[]) dr["File3"];
                messageList.Add(message);
            }
            return messageList;
        }


        /// <method>
        /// Get received messages by user id
        /// </method>
        public List<Message> GetIncomeMessages(int _id)
        {
            var messageList = new List<Message>();
            Message message;
            DataTable dataTable = _msgDAO.SearchIncomeMessages(_id);


            foreach (DataRow dr in dataTable.Rows)
            {
                message = new Message();
                message.Id = Int32.Parse(dr["Id"].ToString());
                message.From = _userBus.getUserById(Int32.Parse(dr["FromUser"].ToString()));
                message.To = _userBus.getUserById(Int32.Parse(dr["ToUser"].ToString()));
                message.When = DateTime.Parse(dr["CreateDate"].ToString());
                message.File1 = (byte[]) dr["File1"];
                message.File2 = (byte[]) dr["File2"];
                message.File3 = (byte[]) dr["File3"];
                message.IV = (byte[]) dr["IV"];
                messageList.Add(message);
            }
            return messageList;
        }

        /// <method>
        /// Get Message By Id 
        /// </method>
        public Message GetMessageById(int _id)
        {
            Message message = new Message();
            DataTable dataTable = new DataTable();
            dataTable = _msgDAO.SearchById(_id);

            foreach (DataRow dr in dataTable.Rows)
            {
                message.Id = Int32.Parse(dr["Id"].ToString());
                message.From = _userBus.getUserById(Int32.Parse(dr["FromUser"].ToString()));
                message.To = _userBus.getUserById(Int32.Parse(dr["ToUser"].ToString()));
                message.When = DateTime.Parse(dr["CreateDate"].ToString());
                message.File1 = (byte[]) dr["File1"];
                message.File2 = (byte[]) dr["File2"];
                message.File3 = (byte[]) dr["File3"];
                message.IV = (byte[]) dr["IV"];
            }
            return message;
        }

        public bool Create(Message _message)
        {
            _message.When = DateTime.Now;
            if (_msgDAO.Create(_message))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
