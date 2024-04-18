using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;

namespace Tutorial5.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;

    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IEnumerable<Animal> GetAnimals(string orderby)
    {
        // Otwieramy połaczenie do bazy danych
        using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        connection.Open();
        // Definiujemy query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        switch(orderby) 
        {
            case "Description":
                command.CommandText = "SELECT * FROM Animal order by Description;";
                break;
            case "Category":
                command.CommandText = "SELECT * FROM Animal order by Category;";
                break;
            case "Area":
                command.CommandText = "SELECT * FROM Animal order by Area;";
                break;
            default:
                command.CommandText = "SELECT * FROM Animal;";
                break;
        }
       

        // Wykonanie commanda
        var reader = command.ExecuteReader();
        
        var animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int DescriptionOrdinal = reader.GetOrdinal("Description");
        int CategoryOrdinal = reader.GetOrdinal("Category");
        int AreaOrdinal = reader.GetOrdinal("Area");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(DescriptionOrdinal),
                Category = reader.GetString(CategoryOrdinal),
                Area = reader.GetString(AreaOrdinal)
            });
        }

        return animals;
    }

   

   

    public void AddAnimal(AddAnimal animal)
    {
        using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        connection.Open();

       
        string getMaxIdQuery = "SELECT MAX(idanimal) FROM Animal;";
        using SqlCommand getMaxIdCommand = new SqlCommand(getMaxIdQuery, connection);

       
        int maxId = Convert.ToInt32(getMaxIdCommand.ExecuteScalar());
        int newId = maxId + 1;

        
        string insertQuery = "INSERT INTO Animal VALUES (@idAnimal, @animalName, @description, @category, @area);";
        using SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
        insertCommand.Parameters.AddWithValue("idAnimal", newId);
        insertCommand.Parameters.AddWithValue("animalName", animal.Name);
        insertCommand.Parameters.AddWithValue("description", animal.Description);
        insertCommand.Parameters.AddWithValue("category", animal.Category);
        insertCommand.Parameters.AddWithValue("area", animal.Area);

        insertCommand.ExecuteNonQuery();
    }

    public void PutAnimal(AddAnimal animal,int id)
    {
        // Otwieramy połaczenie do bazy danych
        using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        connection.Open();
        // Definiujemy query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "UPDATE Animal set Name=@animalName, Description=@description, Category=@category, Area=@area where IdAnimal=@id;";
        command.Parameters.AddWithValue("animalName", animal.Name);
        command.Parameters.AddWithValue("description", animal.Description);
        command.Parameters.AddWithValue("category", animal.Category);
        command.Parameters.AddWithValue("area", animal.Area);
        command.Parameters.AddWithValue("id", id);

        command.ExecuteNonQuery();
    }
    public void DeleteAnimal(int id)
    {
        // Otwieramy połaczenie do bazy danych
        using SqlConnection connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True");
        connection.Open();
        // Definiujemy query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "DELETE from Animal where IdAnimal=@id";
        command.Parameters.AddWithValue("id", id);

        command.ExecuteNonQuery();
    }
}