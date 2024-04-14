using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKadr.mvvm.model
{
    public class KadrRepository
    {
        private KadrRepository()
        {

        }

        static KadrRepository instance;
        public static KadrRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new KadrRepository();
                return instance;
            }
        }

        internal IEnumerable<Kadr> GetAllKadrs(string sql)
        {
            var result = new List<Kadr>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Kadr kadr = new Kadr();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (kadr.Id != id)
                    {
                        kadr = new Kadr();
                        result.Add(kadr);
                        kadr.Id = id;
                        kadr.Name = reader.GetString("name");
                        kadr.Surname = reader.GetString("surname");
                        kadr.Otchestvo = reader.GetString("otchestvo");
                    }
                    Post post = new Post
                    {
                        Id = reader.GetInt32("postId"),
                        Name = reader.GetString("postName"),
                        Zarplata = reader.GetInt32("postZarplata"),
                        DateGet = reader.GetDateOnly("postDateGet"),
                        DateEnd = reader.GetDateOnly("postDateEnd")
                    };
                    kadr.Posts.Add(post);
                }
            }

            return result;
        }

        internal void AddKadr(Kadr kadr)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            int id = MySqlDB.Instance.GetAutoID("Kadr");

            string sql = "INSERT INTO Kadr VALUES (0, @name, @surname, @otchestvo)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("name", kadr.Name));
                mc.Parameters.Add(new MySqlParameter("surname", kadr.Surname));
                mc.Parameters.Add(new MySqlParameter("otchestvo", kadr.Otchestvo));
                if (mc.ExecuteNonQuery() > 0)
                {
                    sql = "";
                    foreach (var post in kadr.Posts)
                        sql += "INSERT INTO CrossKadrPost VALUES (" + id + "," + post.Id + ");";
                    using (var mcCross = new MySqlCommand(sql, connect))
                        mcCross.ExecuteNonQuery();
                }
            }
        }

        internal void Remove(Kadr kadr)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM CrossKadrPost WHERE idKadr = '" + kadr.Id + "';";
            sql += "DELETE FROM Kadr WHERE id = '" + kadr.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal IEnumerable<Kadr> Search(string searchText, Post selectedPost)
        {
            string sql = "SELECT k.id, k.name, k.surname, k.otchestvo, p.id AS postId, p.Name AS tagTitle FROM CrossKadrPost ckp, Kadr k, Post p WHERE ckp.id_Kadr = k.id AND ckp.id_Post = p.id";
            sql += " AND (k.Name LIKE '%" + searchText + "%'";
            sql += " OR k.Surname LIKE '%" + searchText + "%')";
            sql += " OR k.Otchestvo LIKE '%" + searchText + "%')";

            if (selectedPost.Id != 0)
            {
                var result = GetAllKadrs(sql).Where(s => s.Posts.FirstOrDefault(s => s.Id == selectedPost.Id) != null);
                return result;
            }
            return GetAllKadrs(sql);
        }

        internal void UpdateKadr(Kadr kadr)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM CrossKadrPost WHERE id_Kadr = '" + kadr.Id + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

            sql = "";
            foreach (var post in kadr.Posts)
                sql += "INSERT INTO CrossKadrPost VALUES (" + kadr.Id + "," + post.Id + ");";
            using (var mcCross = new MySqlCommand(sql, connect))
                mcCross.ExecuteNonQuery();

            sql = "UPDATE Drink SET Title = @title, Capacity = @capacity, Price = @price, Description = @description WHERE Id = " + drink.ID;
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("name", kadr.Name));
                mc.Parameters.Add(new MySqlParameter("surname", kadr.Surname));
                mc.Parameters.Add(new MySqlParameter("otchestvo", kadr.Otchestvo));
                mc.ExecuteNonQuery();
            }
        }
    }
}
