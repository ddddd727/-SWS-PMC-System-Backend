using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations.DictStrategies
{
    public class FittingDictStrategy : IDictStrategy
    {
        public Task<int> AddAsync(string type, DictItemConfig config, DictInputDto data) => throw new NotImplementedException("管子连接件复杂逻辑开发中");
        public Task<int> UpdateAsync(string type, int id, DictItemConfig config, DictInputDto data) => throw new NotImplementedException();
        public Task<int> DeleteAsync(string type, int id, DictItemConfig config) => throw new NotImplementedException();
    }
}