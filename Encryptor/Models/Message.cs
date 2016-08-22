using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User From { get; set; }
        public User To { get; set; }
        public DateTime When { get; set; }
        public byte[] File1 { get; set; }
        public byte[] File2 { get; set; }
        public byte[] File3 { get; set; }
        public byte[] IV { get; set; }
    }
}
