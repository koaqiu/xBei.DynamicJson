# 动态解析JSON

**开发测试中**

## 说明

有时候（特别是写和第三方通信的接口或者相关组件时）用来传输数据的JSON文件格式不稳定，另外当前程序只需要部分字段，这时候使用实体数据类，如果定义不完整，然后进行反系列化在系列化等操作以后，数据就会丢失。
这时候就需要动态解析JSON了。

示例JSON

```json
{
  "Name": "WMB",
  "age": 42,
  "CreateTime": "2023-10-13T21:26:22+08:00"
}
```
使用示例：
```c#
    [JsonConverter(typeof(net.xBei.DynamicJson.Converters.DynamicJsonConverter<Car>))]
    class Car : net.xBei.DynamicJson.DynamicJson {
        public Car() : base(default) { }
        public string Name { get => GetString(nameof(Name)) ?? ""; set => SetString(nameof(Name), value); }
        public DateTime? CreateTime {
            get => GetDateTime(nameof(CreateTime));
            set => SetDateTime(nameof(CreateTime), value);
        }
    }

    var car = json.TryDeserialize<Car>() ?? throw new Exception("");
    car.Name = "福克斯（Focus）"
    Console.WriteLine(car.ToJson(true));
```
类`Car`虽然没有定义`age`字段，但是在反序列化时，会自动忽略掉，而在序列化时，会自动添加上去。

输出结果：
```json
{
  "Name": "福克斯（Focus）",
  "age": 42,
  "CreateTime": "2023-10-13T21:26:22+08:00"
}
```

## 注意

1. **开发测试中**
1. 还不支持复杂数据类型
