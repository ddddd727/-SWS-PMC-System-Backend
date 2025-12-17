using PMCSystem_Backend.Models;

namespace PMCSystem_Backend.Services.Interface
{
    public interface IExampleService
    {
        ExampleDto GetExampleData();

        ExampleDto GetDbData();

        public void SaveExample(ExampleDto dto);
    }
}
