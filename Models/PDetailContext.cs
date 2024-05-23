using Modul_7.Helpers;
using Npgsql;

namespace Modul_7.Models
{
    public class PDetailContext : IDisposable
    {
        private readonly string _constr;
        private string _errorMsg;
        private readonly SqlDBHelpers _db;

        public PDetailContext(string constr)
        {
            _constr = constr;
            _db = new SqlDBHelpers(_constr);
        }

        public void InsertPersonDetails(List<PersonDetailApi> personDetails)
        {
            string query = @"INSERT INTO person_detail (id, name, saldo, hutang) 
                     VALUES (@id, @name, @saldo, @hutang) 
                     ON CONFLICT (id) DO UPDATE SET 
                         name = EXCLUDED.name,
                         saldo = EXCLUDED.saldo,
                         hutang = EXCLUDED.hutang;";

            try
            {
                using (var connection = new NpgsqlConnection(_constr))
                {
                    connection.Open();
                    foreach (var person in personDetails)
                    {
                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("id", person.id);
                            cmd.Parameters.AddWithValue("name", person.name);
                            cmd.Parameters.AddWithValue("saldo", person.saldo);
                            cmd.Parameters.AddWithValue("hutang", person.hutang);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                throw;
            }
        }


        public PersonDetail getPersonDetail(int id)
        {
            string query = @"SELECT p.id, p.name, p.age, pd.saldo, pd.hutang FROM persons p 
                             JOIN person_detail pd ON p.id = pd.id WHERE p.id = @id;";

            PersonDetail personDetail = null;

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_constr))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("id", id);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                personDetail = new PersonDetail
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    age = reader.GetInt32(2),
                                    detail = new Detail
                                    {
                                        saldo = reader.GetInt32(3),
                                        hutang = reader.GetInt32(4)
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }

            return personDetail;
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}

