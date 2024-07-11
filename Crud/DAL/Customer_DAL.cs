using Crud.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Crud.DAL
{
	public class Customer_DAL
	{
		SqlConnection _connection = null;

		SqlCommand _command = null;

        public IConfiguration Configuration { get; set; }
    
		private string GetConnectionString()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			Configuration = builder.Build();

			return Configuration.GetConnectionString("DefaultConnection");
		}

		public List<Customer> GetAll()
		{
			List<Customer> customersList = new List<Customer>();

			using (_connection = new SqlConnection(GetConnectionString()))
			{
				_command = _connection.CreateCommand();
				_command.CommandType = CommandType.Text;
				_command.CommandText = @"
									SELECT [CustomerID]
										  ,[NameStyle]
										  ,[Title]
										  ,[FirstName]
										  ,[MiddleName]
										  ,[LastName]
										  ,[Suffix]
										  ,[CompanyName]
										  ,[SalesPerson]
										  ,[EmailAddress]
										  ,[Phone]
										  ,[PasswordHash]
										  ,[PasswordSalt]
										  ,[rowguid]
										  ,[ModifiedDate]
									FROM [SalesLT].[Customer]";

				_connection.Open(); 
				SqlDataReader reader = _command.ExecuteReader();

				while (reader.Read())
				{
					Customer customer = new Customer
					{
						CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
						NameStyle = reader.GetBoolean(reader.GetOrdinal("NameStyle")),
						Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
						FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
						MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
						LastName = reader.GetString(reader.GetOrdinal("LastName")),
						Suffix = reader.IsDBNull(reader.GetOrdinal("Suffix")) ? null : reader.GetString(reader.GetOrdinal("Suffix")),
						CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
						SalesPerson = reader.IsDBNull(reader.GetOrdinal("SalesPerson")) ? null : reader.GetString(reader.GetOrdinal("SalesPerson")),
						EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
						Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
						PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
						PasswordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt")),
						RowGuid = reader.GetGuid(reader.GetOrdinal("rowguid")),
						ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
					};

					customersList.Add(customer);

				}
				reader.Close();
				_connection.Close();

				return customersList;

			}
		}

		public void Add(Customer customer)
		{
			using (_connection = new SqlConnection(GetConnectionString()))
			{
				_command = _connection.CreateCommand();
				_command.CommandType = CommandType.Text;

				_command.CommandText = @"
                    INSERT INTO [SalesLT].[Customer]
                               ([NameStyle]
                               ,[Title]
                               ,[FirstName]
                               ,[MiddleName]
                               ,[LastName]
                               ,[Suffix]
                               ,[CompanyName]
                               ,[SalesPerson]
                               ,[EmailAddress]
                               ,[Phone]
                               ,[PasswordHash]
                               ,[PasswordSalt]
                               ,[rowguid]
                               ,[ModifiedDate])
                    VALUES
                               (@NameStyle
                               ,@Title
                               ,@FirstName
                               ,@MiddleName
                               ,@LastName
                               ,@Suffix
                               ,@CompanyName
                               ,@SalesPerson
                               ,@EmailAddress
                               ,@Phone
                               ,@PasswordHash
                               ,@PasswordSalt
                               ,@RowGuid
                               ,@ModifiedDate)";

				_command.Parameters.AddWithValue("@NameStyle", customer.NameStyle);
				_command.Parameters.AddWithValue("@Title", (object)customer.Title ?? DBNull.Value);
				_command.Parameters.AddWithValue("@FirstName", customer.FirstName);
				_command.Parameters.AddWithValue("@MiddleName", (object)customer.MiddleName ?? DBNull.Value);
				_command.Parameters.AddWithValue("@LastName", customer.LastName);
				_command.Parameters.AddWithValue("@Suffix", (object)customer.Suffix ?? DBNull.Value);
				_command.Parameters.AddWithValue("@CompanyName", (object)customer.CompanyName ?? DBNull.Value);
				_command.Parameters.AddWithValue("@SalesPerson", (object)customer.SalesPerson ?? DBNull.Value);
				_command.Parameters.AddWithValue("@EmailAddress", (object)customer.EmailAddress ?? DBNull.Value);
				_command.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);
				_command.Parameters.AddWithValue("@PasswordHash", customer.PasswordHash);
				_command.Parameters.AddWithValue("@PasswordSalt", customer.PasswordSalt);
				_command.Parameters.AddWithValue("@RowGuid", customer.RowGuid);
				_command.Parameters.AddWithValue("@ModifiedDate", customer.ModifiedDate);

				_connection.Open();
				_command.ExecuteNonQuery();
				_connection.Close();
			}
		}

		public void Update(Customer customer)
		{
			using (_connection = new SqlConnection(GetConnectionString()))
			{
				_command = _connection.CreateCommand();
				_command.CommandType = CommandType.Text;
				_command.CommandText = @"
                    UPDATE [SalesLT].[Customer]
                    SET [NameStyle] = @NameStyle
                       ,[Title] = @Title
                       ,[FirstName] = @FirstName
                       ,[MiddleName] = @MiddleName
                       ,[LastName] = @LastName
                       ,[Suffix] = @Suffix
                       ,[CompanyName] = @CompanyName
                       ,[SalesPerson] = @SalesPerson
                       ,[EmailAddress] = @EmailAddress
                       ,[Phone] = @Phone
                       ,[PasswordHash] = @PasswordHash
                       ,[PasswordSalt] = @PasswordSalt
                       ,[ModifiedDate] = @ModifiedDate
                    WHERE [CustomerID] = @CustomerID";

				_command.Parameters.AddWithValue("@NameStyle", customer.NameStyle);
				_command.Parameters.AddWithValue("@Title", (object)customer.Title ?? DBNull.Value);
				_command.Parameters.AddWithValue("@FirstName", customer.FirstName);
				_command.Parameters.AddWithValue("@MiddleName", (object)customer.MiddleName ?? DBNull.Value);
				_command.Parameters.AddWithValue("@LastName", customer.LastName);
				_command.Parameters.AddWithValue("@Suffix", (object)customer.Suffix ?? DBNull.Value);
				_command.Parameters.AddWithValue("@CompanyName", (object)customer.CompanyName ?? DBNull.Value);
				_command.Parameters.AddWithValue("@SalesPerson", (object)customer.SalesPerson ?? DBNull.Value);
				_command.Parameters.AddWithValue("@EmailAddress", (object)customer.EmailAddress ?? DBNull.Value);
				_command.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);
				_command.Parameters.AddWithValue("@PasswordHash", customer.PasswordHash);
				_command.Parameters.AddWithValue("@PasswordSalt", customer.PasswordSalt);
				_command.Parameters.AddWithValue("@ModifiedDate", customer.ModifiedDate);
				_command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);

				_connection.Open();
				_command.ExecuteNonQuery();
				_connection.Close();
			}
		}

		public void Delete(int customerId)
		{
			using (_connection = new SqlConnection(GetConnectionString()))
			{
				_command = _connection.CreateCommand();
				_command.CommandType = CommandType.Text;
				_command.CommandText = @"
                    DELETE FROM [SalesLT].[Customer]
                    WHERE [CustomerID] = @CustomerID";

				_command.Parameters.AddWithValue("@CustomerID", customerId);

				_connection.Open();
				_command.ExecuteNonQuery();
				_connection.Close();
			}
		}

		public Customer GetCustomerById(int customerId)
		{
			Customer customer = null;

			using (_connection = new SqlConnection(GetConnectionString()))
			{
				_command = _connection.CreateCommand();
				_command.CommandType = CommandType.Text;
				_command.CommandText = @"
                    SELECT [CustomerID]
                          ,[NameStyle]
                          ,[Title]
                          ,[FirstName]
                          ,[MiddleName]
                          ,[LastName]
                          ,[Suffix]
                          ,[CompanyName]
                          ,[SalesPerson]
                          ,[EmailAddress]
                          ,[Phone]
                          ,[PasswordHash]
                          ,[PasswordSalt]
                          ,[rowguid]
                          ,[ModifiedDate]
                    FROM [SalesLT].[Customer]
                    WHERE [CustomerID] = @CustomerID";

				_command.Parameters.AddWithValue("@CustomerID", customerId);

				_connection.Open();
				SqlDataReader reader = _command.ExecuteReader();

				if (reader.Read())
				{
					customer = new Customer
					{
						CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
						NameStyle = reader.GetBoolean(reader.GetOrdinal("NameStyle")),
						Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
						FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
						MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
						LastName = reader.GetString(reader.GetOrdinal("LastName")),
						Suffix = reader.IsDBNull(reader.GetOrdinal("Suffix")) ? null : reader.GetString(reader.GetOrdinal("Suffix")),
						CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
						SalesPerson = reader.IsDBNull(reader.GetOrdinal("SalesPerson")) ? null : reader.GetString(reader.GetOrdinal("SalesPerson")),
						EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
						Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
						PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
						PasswordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt")),
						RowGuid = reader.GetGuid(reader.GetOrdinal("rowguid")),
						ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
					};
				}

				reader.Close();
				_connection.Close();
			}

			return customer;
		}
	}
}
