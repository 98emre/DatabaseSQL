using Microsoft.Data.SqlClient;

namespace DatabaseSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "source",
                UserID = "user name",
                Password = "password",
                InitialCatalog = "database"
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    // CRUD Operations on Customer
                    int customerId = PostCustomer(connection, "Test", "Test", "Mr.", "Test Company", "Test.Test@example.com", "123-456-7890", "ABCD1234", "ABCD5678");
                    GetCustomer(connection, customerId);
                    EditCustomer(connection, customerId, "Test2", "Test2", "Mr.", "Updated Company", "Test2.Test2@example.com", "423-222-3210");
                    GetCustomer(connection, customerId);

                    // CRUD Operations on Address
                    Console.WriteLine("\n");
                    int addressId = PostAddress(connection, "Test City", null, "Test City", "Test State", "Test Country", "12345");
                    GetAddress(connection, addressId);
                    EditAddress(connection, addressId, "Test City 2", "Apt 10", "Updated City", "Updated State", "Updated Country", "67890");
                    GetAddress(connection, addressId);

                    // CRUD Operations on CustomerAddress
                    Console.WriteLine("\n");
                    PostCustomerAddress(connection, customerId, addressId, "Home");
                    GetCustomerAddress(connection, customerId, addressId);

                    // Clean up
                    DeleteCustomerAddress(connection, customerId, addressId);
                    DeleteAddress(connection, addressId);
                    DeleteCustomer(connection, customerId);
                    GetCustomer(connection, customerId);
                    GetAddress(connection, addressId);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Done");
        }

        // Customer Methods
        static int PostCustomer(SqlConnection connection, string firstName, string lastName, string title, string companyName, string emailAddress, string phone, string passwordHash, string passwordSalt)
        {
            string sql = "INSERT INTO SalesLT.Customer (FirstName, LastName, Title, CompanyName, EmailAddress, Phone, PasswordHash, PasswordSalt, rowguid, ModifiedDate) " +
                         "OUTPUT INSERTED.CustomerID VALUES (@FirstName, @LastName, @Title, @CompanyName, @EmailAddress, @Phone, @PasswordHash, @PasswordSalt, @rowguid, @ModifiedDate)";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@CompanyName", companyName);
                command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                command.Parameters.AddWithValue("@rowguid", Guid.NewGuid());
                command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                int customerId = (int)command.ExecuteScalar();
                Console.WriteLine("Inserted Customer ID: {0}", customerId);
                return customerId;
            }
        }

        static void GetCustomer(SqlConnection connection, int customerId)
        {
            string sql = "SELECT FirstName, LastName, Title, CompanyName, EmailAddress, Phone FROM SalesLT.Customer WHERE CustomerID = @CustomerID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Customer: {0} {1}, Title: {2}, Company: {3}, Email: {4}, Phone: {5}",
                            reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                    }
                    else
                    {
                        Console.WriteLine("Customer ID {0} not found.", customerId);
                    }
                }
            }
        }

        static void EditCustomer(SqlConnection connection, int customerId, string firstName, string lastName, string title, string companyName, string emailAddress, string phone)
        {
            string sql = "UPDATE SalesLT.Customer SET FirstName = @FirstName, LastName = @LastName, Title = @Title, CompanyName = @CompanyName, EmailAddress = @EmailAddress, Phone = @Phone WHERE CustomerID = @CustomerID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@CompanyName", companyName);
                command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                command.Parameters.AddWithValue("@Phone", phone);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Rows updated: {0}", rowsAffected);
            }
        }

        static void DeleteCustomer(SqlConnection connection, int customerId)
        {
            string sql = "DELETE FROM SalesLT.Customer WHERE CustomerID = @CustomerID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Rows deleted: {0}", rowsAffected);
            }
        }

        // Address Methods
        static int PostAddress(SqlConnection connection, string addressLine1, string addressLine2, string city, string stateProvince, string countryRegion, string postalCode)
        {
            string sql = "INSERT INTO SalesLT.Address (AddressLine1, AddressLine2, City, StateProvince, CountryRegion, PostalCode, rowguid, ModifiedDate) " +
                         "OUTPUT INSERTED.AddressID VALUES (@AddressLine1, @AddressLine2, @City, @StateProvince, @CountryRegion, @PostalCode, @rowguid, @ModifiedDate)";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                command.Parameters.AddWithValue("@AddressLine2", addressLine2 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@StateProvince", stateProvince);
                command.Parameters.AddWithValue("@CountryRegion", countryRegion);
                command.Parameters.AddWithValue("@PostalCode", postalCode);
                command.Parameters.AddWithValue("@rowguid", Guid.NewGuid());
                command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                int addressId = (int)command.ExecuteScalar();
                Console.WriteLine("Inserted Address ID: {0}", addressId);
                return addressId;
            }
        }

        static void GetAddress(SqlConnection connection, int addressId)
        {
            string sql = "SELECT AddressLine1, AddressLine2, City, StateProvince, CountryRegion, PostalCode FROM SalesLT.Address WHERE AddressID = @AddressID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AddressID", addressId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Address: {0} {1}, City: {2}, State: {3}, Country: {4}, PostalCode: {5}",
                            reader.GetString(0), reader.IsDBNull(1) ? "N/A" : reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                    }
                    else
                    {
                        Console.WriteLine("Address ID {0} not found.", addressId);
                    }
                }
            }
        }

        static void EditAddress(SqlConnection connection, int addressId, string addressLine1, string addressLine2, string city, string stateProvince, string countryRegion, string postalCode)
        {
            string sql = "UPDATE SalesLT.Address SET AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, City = @City, StateProvince = @StateProvince, CountryRegion = @CountryRegion, PostalCode = @PostalCode WHERE AddressID = @AddressID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AddressID", addressId);
                command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                command.Parameters.AddWithValue("@AddressLine2", addressLine2 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@StateProvince", stateProvince);
                command.Parameters.AddWithValue("@CountryRegion", countryRegion);
                command.Parameters.AddWithValue("@PostalCode", postalCode);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Rows updated: {0}", rowsAffected);
            }
        }

        static void DeleteAddress(SqlConnection connection, int addressId)
        {
            string sql = "DELETE FROM SalesLT.Address WHERE AddressID = @AddressID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AddressID", addressId);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Rows deleted: {0}", rowsAffected);
            }
        }

        // CustomerAddress Methods
        static void PostCustomerAddress(SqlConnection connection, int customerId, int addressId, string addressType)
        {
            string sql = "INSERT INTO SalesLT.CustomerAddress (CustomerID, AddressID, AddressType, rowguid, ModifiedDate) " +
                         "VALUES (@CustomerID, @AddressID, @AddressType, @rowguid, @ModifiedDate)";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@AddressID", addressId);
                command.Parameters.AddWithValue("@AddressType", addressType);
                command.Parameters.AddWithValue("@rowguid", Guid.NewGuid());
                command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                command.ExecuteNonQuery();
                Console.WriteLine("Inserted CustomerAddress with CustomerID: {0} and AddressID: {1}", customerId, addressId);
            }
        }

        static void GetCustomerAddress(SqlConnection connection, int customerId, int addressId)
        {
            string sql = "SELECT CustomerID, AddressID, AddressType FROM SalesLT.CustomerAddress WHERE CustomerID = @CustomerID AND AddressID = @AddressID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@AddressID", addressId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("CustomerAddress: CustomerID: {0}, AddressID: {1}, AddressType: {2}",
                            reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
                    }
                    else
                    {
                        Console.WriteLine("CustomerAddress with CustomerID {0} and AddressID {1} not found.", customerId, addressId);
                    }
                }
            }
        }

        static void DeleteCustomerAddress(SqlConnection connection, int customerId, int addressId)
        {
            string sql = "DELETE FROM SalesLT.CustomerAddress WHERE CustomerID = @CustomerID AND AddressID = @AddressID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@AddressID", addressId);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("CustomerAddress rows deleted: {0}", rowsAffected);
            }
        }
    }
}
