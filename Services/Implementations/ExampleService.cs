using AutoMapper;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Services.Impletation
{
    public class ExampleService : IExampleService
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public ExampleService(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ExampleDto GetExampleData()
        {
            return new ExampleDto()
            {
                Message = "Hello from the service layer!",
                Timestamp = DateTime.Now,
            };
        }

        public ExampleDto GetDbData()
        {
            // 示例： 从数据库读取第一条记录
            var entity= _context.Examples.FirstOrDefault();
            // return entity != null ? new ExampleDto() { Message=entity.Message, Timestamp = DateTime.UtcNow } : new ExampleDto { Message = "No data" };
            // 使用 AutoMapper 映射
            return _mapper.Map<ExampleDto>(entity ?? new ExampleEntity { Message = "No data" });
        }

        public void SaveExample(ExampleDto dto)
        {
            // var entity = new ExampleEntity { Message = dto.Message, TimeStamp = dto.Timestamp };
            var entity = _mapper.Map<ExampleEntity>(dto);
            entity.TimeStamp = DateTime.Now;        // 可覆盖特定字段
            _context.Examples.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateExample(int id,ExampleDto dto)
        {
            var entity = _context.Examples.Find(id);
            if (entity != null)
            {
                _mapper.Map(dto, entity);       // 将 DTO 映射到现有 Entity (源 -> 目标)
                _context.SaveChanges();
            }
        }
    }
}
