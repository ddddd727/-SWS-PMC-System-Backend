using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Services.Impletation
{
    public class ExampleService : IExampleService
    {
        public ExampleDto GetExampleData()
        {
            return new ExampleDto()
            {
                Message = "Hello from the service layer!",
                Timestamp = DateTime.UtcNow,
            };
        }
    }
}
