using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKadr.mvvm.model
{
    public class PostRepository
    {
        private PostRepository()
        {

        }
        static PostRepository instance;
        public static PostRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new PostRepository();
                return instance;
            }
        }

        internal List<Post> GetPosts()
        {
            List<Post> result = new List<Post>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;

            string sql = "SELECT * FROM Post";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                while (reader.Read())
                {
                    var post = new Post
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Zarplata = reader.GetInt32("zarplata"),
                        DateGet = reader.GetDateOnly("dateGet"),
                        DateEnd = reader.GetDateOnly("dateEnd")
                    };
                    result.Add(post);
                }
            }
            return result;
        }
    }
}
