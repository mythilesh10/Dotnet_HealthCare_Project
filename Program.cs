using MySqlConnector;

namespace healthcaredemo
{
	internal class Program
	{


		private static void DisplayPatients()
		{
			string strcon = "Server=localhost;uid=root;pwd=root;database=healthcare";
			using(MySqlConnection con = new MySqlConnection(strcon))
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = con;
				cmd.CommandText = "select *from patient";
				cmd.CommandType = System.Data.CommandType.Text;

				con.Open();
				MySqlDataReader dr = cmd.ExecuteReader();

				int pid;
				string fname, lname, address, contact;
				int doctor_id;
				Console.WriteLine("Pid | Fname | Lname | address | contact | doctor_id");
				while (dr.Read())
				{
					pid = (int)dr[0];
					fname = (string)dr[1];
					lname = (string)dr[2];
					address = (string)dr[3];
					contact = (string)dr[4];
					doctor_id = (int)dr[5];
					
                    Console.WriteLine($"{pid} | {fname} | {lname} | {address} | {contact} |  {doctor_id}");
				}

				dr.Close();
				con.Close();
			}
		}

		private static void DisplayDoctors()
		{
			string strcon = "Server=localhost;uid=root;pwd=root;database=healthcare";
			using (MySqlConnection con = new MySqlConnection(strcon))
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = con;
				cmd.CommandText = "select *from doctor";
				cmd.CommandType = System.Data.CommandType.Text;

				con.Open();
				MySqlDataReader dr = cmd.ExecuteReader();

				int doctor_id;
				string name, degree, specialization;
				int experience;
				Console.WriteLine("Doctor_id  Name  Degree  Specialization  Experience");
				while (dr.Read())
				{
					doctor_id = (int)dr[0];
					name = (string)dr[1];
					degree = (string)dr[2];
					specialization = (string)dr[3];
					experience = (int)dr[4];

					Console.WriteLine($"{doctor_id}  {name}  {degree}  {specialization}  {experience}");
				}

				dr.Close();
				con.Close();
			}
		}

		private static void InsertPatient(int pid, string fname, string lname, string address, string contact, int doctor_id)
		{
			string strcon = "server=localhost;uid=root;pwd=root;database=healthcare";
			using (MySqlConnection con = new MySqlConnection(strcon))
			{
				MySqlTransaction? transaction = null;
				try
				{
					MySqlCommand cmd = new MySqlCommand();
					cmd.Connection = con;
					cmd.CommandText = "Insert into patient(pid,fname,lname,address,contact,doctor_id)" + "values(@pid,@fname,@lname,@address,@contact,@doctor_id)";
					cmd.CommandType = System.Data.CommandType.Text;

					cmd.Parameters.AddWithValue("@pid", pid);
					cmd.Parameters.AddWithValue("@fname", fname);
					cmd.Parameters.AddWithValue("@lname", lname);
					cmd.Parameters.AddWithValue("@address", address);
					cmd.Parameters.AddWithValue("@contact", contact);
					cmd.Parameters.AddWithValue("@doctor_id", doctor_id);

					con.Open();
					transaction = con.BeginTransaction();
					cmd.Transaction = transaction;
					int res = cmd.ExecuteNonQuery();
					transaction.Commit();
					con.Close();
					if (res == 0)
					{
						Console.WriteLine("Error");
					}
					else
					{
						Console.WriteLine("Record inserted successfully");
					}
				}
				catch (Exception e)
				{
					transaction.Rollback();
					if (con != null && con.State == System.Data.ConnectionState.Open)
					{
						con.Close();
					}
					throw;
				}
			}
		}

		private static void UpdatePatient(int pid, string fname, string lname, string address, string contact, int doctor_id)
		{
			string strcon = "server=localhost; uid=root;pwd=root;database=healthcare";
			using (MySqlConnection con = new MySqlConnection(strcon))
			{
				MySqlTransaction? transaction = null;

				try
				{
					MySqlCommand cmd = new MySqlCommand();
					cmd.Connection = con;
					cmd.CommandText = "UPDATE patient SET fname=@fname, lname=@lname, address=@address, contact=@contact, doctor_id=@doctor_id WHERE pid=@pid";

					cmd.CommandType = System.Data.CommandType.Text;


					cmd.Parameters.AddWithValue("@pid", pid);
					cmd.Parameters.AddWithValue("@fname", fname);
					cmd.Parameters.AddWithValue("@lname", lname);
					cmd.Parameters.AddWithValue("@address", address);
					cmd.Parameters.AddWithValue("@contact", contact);
					cmd.Parameters.AddWithValue("@doctor_id", doctor_id);


					con.Open();
					transaction = con.BeginTransaction();
					cmd.Transaction = transaction;

					int res = cmd.ExecuteNonQuery();
					transaction.Commit();
					con.Close();

					if (res == 0)
					{
						Console.WriteLine("Error");
					}
					else
					{
						Console.WriteLine("Record updated successfully");
					}
				}
				catch (Exception e)
				{
					transaction?.Rollback();
					if (con.State == System.Data.ConnectionState.Open)
					{
						con.Close();
					}
                    Console.WriteLine($"Error: {e.Message}");
					throw;
				}
			}
		}

		private static void DeletePatient(int pid)
		{
			string strcon = "Server=localhost;uid=root;pwd=root;database=healthcare";
			using (MySqlConnection con = new MySqlConnection(strcon))
			{
				MySqlTransaction? transaction = null;
				try
				{
					MySqlCommand cmd = new MySqlCommand();
					cmd.Connection = con;
					cmd.CommandText = "DELETE from patient WHERE pid = @pid";
					cmd.CommandType = System.Data.CommandType.Text;
					cmd.Parameters.AddWithValue("@pid", pid);

					con.Open();
					transaction = con.BeginTransaction();
					cmd.Transaction = transaction;

					int res = cmd.ExecuteNonQuery();
					transaction.Commit();
					con.Close();
					if (res == 0)
					{
						Console.WriteLine("Error");
					}
					else
					{
						Console.WriteLine($"{pid} Deleted record successfully");
					}
				}
				catch (Exception e)
				{
					transaction?.Rollback();
					if (con.State == System.Data.ConnectionState.Open)
					{
						con.Close();
					}
					else
					{
						Console.WriteLine($"Error:{e.Message}");
					}
				}
			}
		}

		static void Main(string[] args)
		{
			DisplayPatients();
			Console.WriteLine("-----------------");

			DisplayDoctors();
			Console.WriteLine("------------------");

			InsertPatient(6, "Ayusha", "Salunkhe", "Mumbai", "8723557832", 2);
			Console.WriteLine("-------------------");
			Console.WriteLine("After update");
			DisplayPatients();
			DeletePatient(3);
			Console.WriteLine("After delete");


			UpdatePatient(2, "Mohit", "Chinni", "Nashik", "95466548230", 3);
			Console.WriteLine("--------------------------------------------");
			DisplayPatients();


		}
	}
}
