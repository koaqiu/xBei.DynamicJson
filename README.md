# ��̬����JSON

��ʱ���ر���д�͵�����ͨ�ŵĽӿڻ���������ʱ�������������ݵ�JSON�ļ���ʽ���ȶ������⵱ǰ����ֻ��Ҫ�����ֶΣ���ʱ��ʹ��ʵ�������࣬������岻������Ȼ����з�ϵ�л���ϵ�л��Ȳ����Ժ����ݾͻᶪʧ��
��ʱ�����Ҫ��̬����JSON�ˡ�

ʾ��JSON

```json
{
  "Name": "WMB",
  "age": 42,
  "CreateTime": "2023-10-13T21:26:22+08:00"
}
```
ʹ��ʾ����
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
    car.Name = "����˹��Focus��"
    Console.WriteLine(car.ToJson(true));
```
��`Car`��Ȼû�ж���`age`�ֶΣ������ڷ����л�ʱ�����Զ����Ե����������л�ʱ�����Զ������ȥ��

��������
```json
{
  "Name": "����˹��Focus��",
  "age": 42,
  "CreateTime": "2023-10-13T21:26:22+08:00"
}
```