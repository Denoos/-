using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKadr.mvvm.model
{
    public class Kadr
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Otchestvo { get; set; }
        public List<Post> Posts { get; set; }
    }
}
