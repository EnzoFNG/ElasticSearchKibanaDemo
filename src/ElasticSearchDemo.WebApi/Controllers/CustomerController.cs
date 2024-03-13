using ElasticSearchDemo.WebApi.Data;
using ElasticSearchDemo.WebApi.Models;
using ElasticSearchDemo.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchDemo.WebApi.Controllers;

[ApiController]
[Route("api/v1/customers")]
public sealed class CustomerController(ILogger<CustomerController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCustomers()
    {
        logger.LogInformation("Executing GetAllCustomers method.");

        return Ok(InMemoryData.Customers);
    }

    [HttpGet("{id}")]
    public IActionResult GetCustomerById(Guid id)
    {
        logger.LogInformation("Executing GetAllCustomers method.");

        if (id == Guid.Empty)
        {
            logger.LogInformation($"Error while attempting to set ID {id}.");

            return BadRequest("Id is invalid");
        }

        var customer = InMemoryData.Customers.FirstOrDefault(x => x.Id == id);

        if (customer is null)
        {
            logger.LogInformation($"Error while finding the Customer with ID {id}.");

            return NotFound();
        }

        logger.LogInformation($"Success finding the Customer with the ID {id}.");

        return Ok(customer);
    }

    [HttpPost]
    public IActionResult AddCustomer([FromBody] CustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            logger.LogInformation($"Error while validating the Customer.");

            return BadRequest(ModelState);
        }

        var existentCustomer = InMemoryData.Customers.FirstOrDefault(x => x.Name == request.Name);

        if (existentCustomer is not null)
        {
            logger.LogInformation($"Error while adding the Customer with the Name {request.Name}. This customer already exists.");

            return BadRequest("This customer already exists.");
        }

        var newCustomer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Birthdate = request.Birthdate
        };

        try
        {
            ValidateCustomer(newCustomer);
            InMemoryData.Customers.Add(newCustomer);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, ex.Message, ex.ParamName);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }

        logger.LogInformation("Success adding the Customer.");

        return Ok("Customer added successfully!");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            logger.LogInformation($"Error while validating the Customer.");

            return BadRequest(ModelState);
        }

        var existentCustomerIndex = InMemoryData.Customers.FindIndex(x => x.Id == id);

        if (existentCustomerIndex == -1)
        {
            logger.LogInformation($"Error while finding the Customer with ID {id}.");

            return NotFound();
        }

        var customerUpdated = new Customer
        {
            Id = id,
            Name = request.Name,
            Birthdate = request.Birthdate
        };

        try
        {
            ValidateCustomer(customerUpdated);
            InMemoryData.Customers[existentCustomerIndex] = customerUpdated;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, ex.Message, ex.ParamName);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }

        logger.LogInformation($"Success updating the Customer with ID {id}.");

        return Ok(customerUpdated);
    }

    [HttpDelete("{id}")]
    public IActionResult RemoveCustomer(Guid id)
    {
        var existentCustomer = InMemoryData.Customers.FirstOrDefault(x => x.Id == id);

        if (existentCustomer is null)
        {
            logger.LogInformation($"Error while finding the Customer with ID {id}.");

            return NotFound();
        }

        InMemoryData.Customers.Remove(existentCustomer);

        logger.LogInformation($"Success removing the Customer with ID {id}.");

        return Ok("Customer removed successfully!");
    }

    private void ValidateCustomer(Customer customer)
    {
        if (customer.Id == Guid.Empty)
        {
            throw new ArgumentNullException("Customer.Id", "Id is invalid.");
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            throw new ArgumentNullException("Customer.Name", "The name cannot be empty.");
        }

        if (customer.Age < 16)
        {
            throw new ArgumentNullException("Customer.Birthdate", "The customer should be greater than or equal to 16 years old.");
        }
    }
}