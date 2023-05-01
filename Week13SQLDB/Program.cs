

using System.Data.SQLite;

ReadData(CreateConnection());
//InsertCustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
FindCustomer(CreateConnection());


static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source = mydb.db; Version = 3; New = True; Compress = True");

    try
    {
        connection.Open();
        Console.WriteLine("DB Found");
    }
    catch
    {
        Console.WriteLine("DB not found.");
    }

    return connection;
}


static void ReadData (SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, * FROM customer";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowID = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);  
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowID}. Full name: {readerStringFirstName} {readerStringLastName}; DoB: {readerStringDoB}");
    }

    myConnection.Close();
}

static void InsertCustomer (SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName, DoB;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter DoB:");
    DoB = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer (firstName, lastName, dateOfBirth) " +
        $"VALUES ('{fName}', '{lName}', '{DoB}')";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");

    
    ReadData (myConnection);
}

static void RemoveCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Enter a customer ID to delete:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed form the table customer.");

    ReadData (myConnection);

}

static void FindCustomer(SQLiteConnection myConnection) 
{ 
    SQLiteDataReader reader; 
    SQLiteCommand command; 
    
    string searchName; 
    Console.WriteLine("Enter the first name to display customer data:"); 
    searchName = Console.ReadLine(); 
    command = myConnection.CreateCommand();
    command.CommandText = $"select customer.firstname, customer.lastname, status.statustype from customerStatus" +
        $"JOIN customer ON customer.rowid = customerStatus.customerid " +
        $"JOIN status ON status.rowid = customerStatus.statusid " +
        $"WHERE firstname LIKE {searchName}";
    
    reader = command.ExecuteReader(); 
    
    
    while (reader.Read()) 
    { 
        string readerRowid = reader["rowid"].ToString(); 
        string readerStringFirstName = reader.GetString(1); 
        string readerStringLastName = reader.GetString(2); 
        string readerStringStatus = reader.GetString(3); 
        Console.WriteLine($"Search result: ID: {readerRowid}. {readerStringFirstName}{readerStringLastName}. Status: {readerStringStatus}"); 
    } 
    
    myConnection.Close(); 
}