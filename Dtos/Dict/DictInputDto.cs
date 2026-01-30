namespace PMCSystem_Backend.Dtos.Dict
{
    // 继承 Dictionary，因为前端提交的字段是不确定的（动态的）
    public class DictInputDto : Dictionary<string, object>
    {
        // 这是一个空类，它本身就是个字典
        // 用来接收 { "Name": "张三", "Age": 18, ... } 这样的动态 JSON
    }
}