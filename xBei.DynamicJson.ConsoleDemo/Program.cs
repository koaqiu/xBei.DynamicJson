
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using net.xBei.DynamicJson.Helper;

namespace xBei.DynamicJson.ConsoleDemo {
    internal class Program {
        static void Main(string[] args) {
            var json = @"{
""name"": ""John Doe"",
""age"": 42,
  ""CreateTime"": ""2023-10-13T21:26:22+08:00"",
""Part"":[1,2,3,4],
""Apple"":{
""Name"":""Japan"",
""age"": 42
}
}";

            Console.WriteLine(json);
            var car = json.TryDeserialize<Car>() ?? throw new Exception("TryDeserialize");
            Console.WriteLine($"name={car.Name}");
            //Console.WriteLine($"age={car.age}");
            Console.WriteLine($"CreateTime={car.CreateTime}");
            car.Name = "福克斯（Focus）";
            car.Child = new[] { "2012" };
            if (car.Apple != null) {
                car.Apple.Name = "xxx";
            }
            car.BadApple ??= new Apple();
            car.BadApple.Name = "Bad Apple";
            //car.CreateTime = DateTime.UtcNow;
            Console.WriteLine("car from json: car.ToJson()                    -----------------------------------");
            Console.WriteLine(car.ToJson(true));
            Console.WriteLine("car from json: JsonSerializer.Serialize(car)   -----------------------------------");
            Console.WriteLine(JsonSerializer.Serialize(car));
            var car1 = JsonSerializer.Deserialize<Car>(json) ?? throw new Exception("Deserialize");
            car1.Part = new int[] { 5, 8 };
            car1.UpdateData(car1.Apple, apple => apple.CreateTime = DateTime.Now);
            Console.WriteLine("Car1 from json: JsonSerializer.Serialize(car1) -----------------------------------");
            Console.WriteLine(JsonSerializer.Serialize(car1));
            Console.WriteLine("Apple: JsonSerializer.Serialize()              -----------------------------------");
            Console.WriteLine(JsonSerializer.Serialize(new Apple { CreateTime = DateTime.Now }));
        }
    }
    [JsonConverter(typeof(net.xBei.DynamicJson.Converters.DynamicJsonConverter<Apple>))]
    class Apple : net.xBei.DynamicJson.DynamicJson {
        public Apple() : base(default) {
        }

        public string? Name {
            get => GetString(nameof(Name));
            set => SetString(nameof(Name), value); }
        public DateTime? CreateTime { get; set; }
    }
    [JsonConverter(typeof(net.xBei.DynamicJson.Converters.DynamicJsonConverter<Car>))]
    class Car : net.xBei.DynamicJson.DynamicJson {
        public Car() : base(default) { }
        public string Name { get => GetString(nameof(Name)) ?? ""; set => SetString(nameof(Name), value); }
        public int[]? Part {
            get => GetList<int>(nameof(Part))?.ToArray();
            set => SetList(nameof(Part), value);
        }
        public string[]? Child {
            get => GetList<string>(nameof(Child))?.ToArray();
            set => SetList(nameof(Child), value);
        }
        public Apple? Apple {
            get => GetObject<Apple>(nameof(Apple));
            set => SetObject(nameof(Apple), value);
        }
        public Apple? BadApple {
            get => GetObject<Apple>(nameof(BadApple));
            set => SetObject(nameof(BadApple), value);
        }
        //public int age { get => GetInt(nameof(age)) ?? 0; set => SetInt(nameof(age), value); }
        public DateTime? CreateTime {
            get => GetDateTime(nameof(CreateTime));
            set => SetDateTime(nameof(CreateTime), value);
        }
    }
}