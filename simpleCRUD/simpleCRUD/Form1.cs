using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;


namespace simpleCRUD
{
    public partial class Form1 : Form
    {

        //CONEX�O COM O BANCO DE DADOS
        private string data_source = "datasource=localhost;username=root;password=12345;database=crud";
        private MySqlConnection conn;

        public Form1()
        {
            InitializeComponent();

            listView.View = View.Details;
            listView.LabelEdit = true;
            listView.AllowColumnReorder = true;
            listView.FullRowSelect = true;
            listView.GridLines = true;

            listView.Columns.Add("Id", 50, HorizontalAlignment.Left);
            listView.Columns.Add("Nome", 180, HorizontalAlignment.Left);
            listView.Columns.Add("Sobrenome", 180, HorizontalAlignment.Left);
            listView.Columns.Add("E-mail", 180, HorizontalAlignment.Left);
            listView.Columns.Add("Telefone", 155, HorizontalAlignment.Left);
        }



        //Criar Cadastro
        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            try
            {
                conn = new MySqlConnection(data_source);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = "INSERT INTO cadastro (nome,sobrenome,email,telefone) " +
                                  "VALUES (@nome,@sobrenome,@email,@telefone)";
                
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@sobrenome", txtSobrenome.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                MessageBox.Show("O cadastro de " + txtNome.Text + " foi salvo com sucesso!",
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        //Buscar na DB
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "'%" + txtBuscar.Text + "%'";

                conn = new MySqlConnection(data_source);

                string sql = "SELECT * FROM cadastro " +
                            "WHERE nome LIKE " + query + "OR email LIKE" + query + "OR id LIKE" + query;

                conn.Open();

                MySqlCommand comando = new MySqlCommand(sql, conn);

                MySqlDataReader reader = comando.ExecuteReader();

                listView.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)
                    };

                    var l_listview = new ListViewItem(row);

                    listView.Items.Add(l_listview); 
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}