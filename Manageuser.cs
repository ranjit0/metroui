using MetroFramework;
using MetroFramework.Forms;
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

namespace metroui
{
    public partial class Manageuser : MetroForm
    {
        public Manageuser()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DataTable dt = CheckUser(txtUsername.Text);
            if (dt.Rows.Count > 0)
            {
                MetroMessageBox.Show(this, "user already exist");
            }
            else
            {
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
                Integrated Security=true; Database=MetroDB");
                SqlCommand cmd = new SqlCommand("insert into tbluser values(@a,@b,@c,@d)", con);
                cmd.Parameters.AddWithValue("@a", txtUsername.Text);
                cmd.Parameters.AddWithValue("@b", txtPassword.Text);
                cmd.Parameters.AddWithValue("@c", cboUsertype.Text);
                cmd.Parameters.AddWithValue("@d", txtFullname.Text);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    LoadGrid();

                    MetroMessageBox.Show(this, "User Created");
                }
            }
        }

        private void Manageuser_Load(object sender, EventArgs e)
        {
            LoadGrid();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void LoadGrid()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
            Integrated Security=true; Database=MetroDB");
            SqlCommand cmd = new SqlCommand("select * from tbluser", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroGrid1.DataSource = dt;
        }

        int userid = 0;
        private void metroGrid1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            userid = Convert.ToInt32(metroGrid1.CurrentRow.Cells[0].Value.ToString());
            txtUsername.Text = metroGrid1.CurrentRow.Cells[1].Value.ToString();
            txtPassword.Text = metroGrid1.CurrentRow.Cells[2].Value.ToString();
            cboUsertype.Text = metroGrid1.CurrentRow.Cells[3].Value.ToString();
            txtFullname.Text = metroGrid1.CurrentRow.Cells[4].Value.ToString();
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnCreate.Enabled = false;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=true; Database=MetroDB");
            SqlCommand cmd = new SqlCommand("update tbluser set Username=@a,Password=@b,Usertype=@c,Fullname=@d where userid=@e", con);
            cmd.Parameters.AddWithValue("@a", txtUsername.Text);
            cmd.Parameters.AddWithValue("@b", txtPassword.Text);
            cmd.Parameters.AddWithValue("@c", cboUsertype.Text);
            cmd.Parameters.AddWithValue("@d", txtFullname.Text);
            cmd.Parameters.AddWithValue("@e", userid);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                LoadGrid();

                ClearControls();

                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnCreate.Enabled = true;
                MetroMessageBox.Show(this, "User Updated");
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnCreate.Enabled = true;

            ClearControls();

        }

        private void ClearControls()
        {
            txtUsername.Text = "";
            txtFullname.Text = string.Empty;
            txtPassword.Clear();
            cboUsertype.SelectedIndex = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this, "Are you sure to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
            Integrated Security=true; Database=MetroDB");
                SqlCommand cmd = new SqlCommand("delete from tbluser where userid=@e", con);
                cmd.Parameters.AddWithValue("@e", userid);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    LoadGrid();

                    MetroMessageBox.Show(this, "User Deleted");
                }
            }
            else { 
            }
            ClearControls();

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnCreate.Enabled = true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
            Integrated Security=true; Database=MetroDB");
            SqlCommand cmd = new SqlCommand("select * from tbluser where Username like @a", con);
            cmd.Parameters.AddWithValue("@a",txtSearch.Text+"%");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroGrid1.DataSource = dt;
        }

        public DataTable CheckUser(string Username)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;
            Integrated Security=true; Database=MetroDB");
            SqlCommand cmd = new SqlCommand("select * from tbluser where Username = @a", con);
            cmd.Parameters.AddWithValue("@a", txtUsername.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return (dt);
        }
        
    }
}
 