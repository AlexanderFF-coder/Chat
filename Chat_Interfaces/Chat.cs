using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Chat_Interfaces
{
    public partial class Chat : Form
    {
        //Variables para manejar base de datos
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataReader leer;
        public Chat()
        {
            InitializeComponent();
            conexion = new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=Alex;Pwd=12345");
            conexion.Open();
            //LLena el listbox con los grupos que alla en la base de datos (probado y funcional)
            comando = new MySqlCommand("SELECT Nombre_grupo FROM  grupo", conexion);
            leer = comando.ExecuteReader();
            while (leer.Read())
            {
                listBox1.Items.Add(leer["Nombre_grupo"].ToString());
                //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                listBox1.Items.Add("--------------------------------------------------");
            }
        }

        private void Chat_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Buscar chat en listbox y base de datos y selecionar el que mas se parezca y oculta lo demas (no funciona) 
            string chec =textBox1.Text;
            for (int i=0;i<listBox1.Items.Count;i++)
            {
                if (listBox1.Items[i].ToString().Contains(chec))
                {
                    listBox1.SetSelected(i,true);
                }
                else
                {
                    listBox1.SetSelected(i,false);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Crea_grupo crea= new Crea_grupo();
            conexion.Close();
            crea.Show();
            this.Hide();
        }

        private void Chat_VisibleChanged(object sender, EventArgs e)
        {
            conexion = new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=Alex;Pwd=12345");
            conexion.Open();
            comando = new MySqlCommand("SELECT Nombre_grupo FROM  grupo", conexion);
            leer = comando.ExecuteReader();
            while (leer.Read())
            {
                listBox1.Items.Add(leer["Nombre_grupo"].ToString());
                //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                listBox1.Items.Add("--------------------------------------------------");
            }
        }
    }
}
