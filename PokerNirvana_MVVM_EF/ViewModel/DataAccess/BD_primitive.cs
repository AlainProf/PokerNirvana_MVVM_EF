using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerNirvana_MVVM_EF.ViewModel.DataAccess
{
    class BD_primitive
    {
        private readonly string CONNEXION_PARAM;
        private MySqlConnection connexion;
        //private MySqlTransaction transaction;

        public BD_primitive()
        {
            CONNEXION_PARAM = ConfigurationManager.ConnectionStrings["NirvanaConnexion_MySQL"].ConnectionString;

            try
            {
                connexion = new MySqlConnection(CONNEXION_PARAM);
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        private bool Ouvrir()
        {
            try
            {
                if (connexion.State == ConnectionState.Open)
                    return true;

                connexion.Open();
                return true;
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        private bool Fermer()
        {
            try
            {
                connexion.Close();
                return true;
            }
            catch (MySqlException)
            {
                throw;
            }
        }
        public int Commande(string query)
        {
            //MessageBox.Show(query);
            int nbResultat = 0;
            try
            {
                if (Ouvrir() || connexion.State == ConnectionState.Open)
                {
                    MySqlCommand command = new MySqlCommand(query, connexion);
                    nbResultat = command.ExecuteNonQuery();
                    if (nbResultat == 1)
                        nbResultat = (int)command.LastInsertedId;
                }
                return nbResultat;
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                // if (transaction == null)
                Fermer();
            }
        }


        public DataSet Recuperer(string req)
        {

            DataSet dataset = new DataSet();

            try
            {
                if (Ouvrir()) //|| transaction != null)
                {
                    MySqlDataAdapter adapteur = new MySqlDataAdapter();
                    adapteur.SelectCommand = new MySqlCommand(req, connexion);
                    adapteur.Fill(dataset);
                }
                return dataset;

            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                Fermer();
            }
        }

        //public DataSet Recuperer()
        //{

        //    DataSet dataset = new DataSet();

        //    try
        //    {
        //        if (Ouvrir()) //|| transaction != null)
        //        {
        //            MySqlDataAdapter adapteur = new MySqlDataAdapter();
        //            adapteur.SelectCommand = new MySqlCommand("select * from produits", connexion);
        //            adapteur.Fill(dataset);
        //        }
        //        return dataset;

        //    }
        //    catch (MySqlException)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        Fermer();
        //    }

        //}

        /* public BD_primitives()
         {

             CONNECTION_STRING = SessionHelper.StringToSessions(Properties.Settings.Default.ActiveSession).First().ToConnexionString();

             ConfigurationConnection(CONNECTION_STRING);
         }
        private void ConfigurationConnection(string connectionString)
        {
            try
            {
                connection = new MySqlConnection(connectionString);
            }
            catch (MySqlException)
            {
                throw;
            }
        }          

         public void OpenWithTransaction()
          {
              try
              {
                  if (Open())
                  {
                      transaction = connection.BeginTransaction();
                  }
              }
              catch (MySqlException)
              {
                  throw;
              }
          }



          public void Commit()
          {
              try
              {
                  transaction.Commit();
                  transaction = null;
                  connection.Close();
              }
              catch (MySqlException)
              {
                  throw;
              }
          }

          public void Rollback()
          {
              try
              {
                  transaction.Rollback();
                  transaction = null;
                  connection.Close();
              }
              catch (MySqlException)
              {
                  throw;
              }
          }


       public DataSet StoredProcedure(string query, IList<MySqlParameter> parameters = null)
       {
           DataSet dataset = new DataSet();

           try
           {
               if (Open() || transaction != null)
               {

                   MySqlCommand commande = new MySqlCommand(query, connection);
                   commande.CommandType = CommandType.StoredProcedure;

                   if (parameters != null)
                   {
                       commande.Parameters.AddRange(parameters.ToArray());
                   }
                   MySqlDataAdapter adapter = new MySqlDataAdapter();
                   adapter.SelectCommand = commande;
                   adapter.Fill(dataset);


               }
               return dataset;

           }
           catch (MySqlException)
           {
               throw;
           }
           finally
           {
               if (transaction == null)
                   Close();
           }
       }

       public void Dispose()
       {
           Dispose(true);
           GC.SuppressFinalize(this);
       }

       protected virtual void Dispose(bool disposing)
       {
           if (disposing)
           {
               if (connection != null)
                   connection.Dispose();
               if (transaction != null)
                   transaction.Dispose();
           }
       }*/
    }
}
