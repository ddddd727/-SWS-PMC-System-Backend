using Microsoft.Extensions.DependencyInjection;
using PMCSystem_Backend.Services.Interfaces;
using System;

namespace PMCSystem_Backend.Services.Implementations.DictStrategies
{
    public class DictStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public DictStrategyFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public IDictStrategy GetStrategy(string? handlerType)
        {
            return handlerType switch
            {
                "Attribute" => _serviceProvider.GetRequiredService<AttributeDictStrategy>(),
                "Fitting" => _serviceProvider.GetRequiredService<FittingDictStrategy>(),
                "Flange" => _serviceProvider.GetRequiredService<FlangeDictStrategy>(),
                // 默认兜底走业务属性策略
                _ => _serviceProvider.GetRequiredService<AttributeDictStrategy>()
            };
        }
    }
}