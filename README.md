# Convert between Binary and  Bean

+ add a `PersonBean` class

```csharp
[BBConvertClass]
public class PersonBean
{
    [BBConvertPrimitive(Order = 0, Len = 1)]
    public PersonRole Role { get; set; }

    [BBConvertPrimitive(Order = 1, Len = 2)]
    public int Id { get; set; }

    [BBConvertPrimitive(Order = 3, Len = 1)]
    public bool Male { get; set; }

    [BBConvertPrimitive(Order = 4, Len = 2, Rate = 0.1)]
    public double Score { get; set; }
}
```
+ Convert Bean to Binary

```csharp
var person = new PersonBean()
 {
     Role = PersonRole.Teacher,
     Id = 100,
     Male = true,
     Score = 99.9,
 };
 var binary = BBConvertFactory.CreateBinary(person);
 ```

+ Convert Binary to Bean

```csharp
var person2 = BBConvertFactory.CreateBean(typeof(PersonBean), binary, 0, out _);
```