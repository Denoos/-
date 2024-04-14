using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKadr.mvvm.model
{
    public class Post
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public int Zarplata { get; set; }
        public DateOnly DateGet { get; set; }
        public DateOnly DateEnd { get; set; }
    }
}
