
using System.Text.Json;
using System.Text.Json.Serialization;
using net.xBei.DynamicJson.Helper;

namespace xBei.DynamicJson.ConsoleDemo {
    internal class Program {
        static void Main(string[] args) {
            var json = @"{
""name"": ""John Doe"",
""age"": 42,
  ""CreateTime"": ""2023-10-13T21:26:22+08:00""
}";

            Console.WriteLine(json);
            Console.WriteLine("-----------------------------------");
            var car = json.TryDeserialize<Car>() ?? throw new Exception("");
            Console.WriteLine($"name={car.Name}");
            //Console.WriteLine($"age={car.age}");
            Console.WriteLine($"CreateTime={car.CreateTime}");
            car.Name = "福克斯（Focus）";
            //car.CreateTime = DateTime.UtcNow;
            Console.WriteLine(car.ToJson(true));
            Console.WriteLine("-----------------------------------"); 
            Console.WriteLine(JsonSerializer.Serialize(car));
            Console.WriteLine("-----------------------------------");
            var car1 = JsonSerializer.Deserialize<Car>(json) ?? throw new Exception("");
            Console.WriteLine(JsonSerializer.Serialize(car1));
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(JsonSerializer.Serialize(new Apple { CreateTime = DateTime.Now }));
        }
    }
    class Apple {
        public DateTime? CreateTime { get; set; }
    }
    //[JsonConverter(typeof(net.xBei.DynamicJson.Converters.DynamicJsonConverter<Car>))]
    class Car : net.xBei.DynamicJson.DynamicJson {
        public Car() : base(default) { }
        public string Name { get => GetString(nameof(Name)) ?? ""; set => SetString(nameof(Name), value); }
        //public int age { get => GetInt(nameof(age)) ?? 0; set => SetInt(nameof(age), value); }
        public DateTime? CreateTime {
            get => GetDateTime(nameof(CreateTime));
            set => SetDateTime(nameof(CreateTime), value);
        }
    }
}