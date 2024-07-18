namespace Cambios.Serviços
{
    using Cambios.Modelos;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;

    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogueService dialogueService;

        public DataService()
        {
            dialogueService = new DialogueService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Rates.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists rates(RateId int, Code varchar(5), TaxRate real, Name varchar(250))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();

            }
            catch (Exception e)
            { 
                dialogueService.ShowMessage("Erro",e.Message); 
            }
        }

        public void SaveData(List<Rates> Rate)
        {
            try
            {
                foreach (var rate in Rate)
                {
                    string sql = string.Format("insert into Rates (RateId, Code, TaxRate, Name) values({0}, '{1}', {2}, '{3}')",
                        rate.RateId, rate.Code, rate.TaxRate.ToString(CultureInfo.GetCultureInfo("en-GB")), rate.Name);

                    command = new SQLiteCommand(sql, connection);

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                dialogueService.ShowMessage("Erro", e.Message);
            }
        }

        public List<Rates> GetData()
        {
            List<Rates> rates = new List<Rates>();

            try
            {
                string sql = "select RateId, Code, TaxRate, Name from Rates";

                command = new SQLiteCommand(sql, connection);

                //Lê cada registo
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rates.Add(new Rates
                    {
                        RateId = (int) reader["RateId"],
                        Code = (string) reader["Code"],
                        TaxRate = (double)reader["TaxRate"],
                        Name = (string) reader["Name"],
                        
                    });
                }
                connection.Close();

                return rates;
            }
            catch (Exception e)
            {
                dialogueService.ShowMessage("Erro",e.Message);
                return null;
            }
        }

        public void DeleteData()
        {
            try
            {
                string sql = "delete from Rates";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogueService.ShowMessage("Erro", e.Message);
            }
        }
    }
}
