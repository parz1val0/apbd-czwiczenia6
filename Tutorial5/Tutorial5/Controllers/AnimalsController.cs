using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;
using Tutorial5.Repositories;

namespace Tutorial5.Controllers;

[ApiController]
// [Route("/api/animals")]
[Route("/api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;

    public AnimalsController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy = null)
    {
        var animals = _animalRepository.GetAnimals(orderBy);

        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal animal)
    {
        _animalRepository.AddAnimal(animal);
        
        // 200
        return Created("/api/animals", null);
    }
    [HttpPut("{id}")]
    public IActionResult PutAnimal(AddAnimal animal,int id)
    {
        _animalRepository.PutAnimal(animal,id);
        
        // 200
        return Created("/api/animals/"+id, null);
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        _animalRepository.DeleteAnimal(id);
        
        // 200
        return Created("/api/animals/"+id, null);
    }
}