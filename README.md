# Yawlib
Yet Another Wmi Library.

A simple C# lib for doing Windows Wmi .net 4.5 style.
(I know, not the most creative name...)

### Features
	* Async/Await WMI functions.
	* Generic type conversions between dto and wmi classes.
	* Simple remote wmi calls.
	* User parsing of wmi objects thru callbacks.
	* ...

### TODO
	* Add more Cimv2 wmi classes.
	* Add more StandardCimv2 wmi classes.
	* Add LinqLight Where functions and other filter functions.
	* Add InteNCS2 wmi classes for talking directly with intel driver.
	* Add HP wmi classes for talking directly with HP driver.

### Example code

First create example dto mapping class.
Note the use of attribute 'WmiClassName' to use friendly .net naming for class, while using another WMI class name for querying.

```csharp
namespace Example
{
	[WmiClassName("Win32_PhysicalMemory")]
	public class PhysicalMemory
	{
		public string Manufacturer {get;set;}
		public string PartNumber {get;set;}
		public UInt64 Capacity {get;set;}		
		public UInt32 Speed {get;set;}
	}
}
```

Then to retrive all instances of PhysicalMemory from wmi use:
```csharp

namespace Example
{
	public class Example1
	{
		public static void Test1()
		{
			//connect to local machine.
			var conn = new WmiConnection();
			var physicalmemory = conn.Get<PhysicalMemory>();
		}
	}
}

```

Or you can run q wql query instead.
```csharp

namespace Example
{
	public class Example2
	{
		public static void Test2()
		{
			//connect to local machine.
			var conn = new WmiConnection();
			var query = new SelectQuery("SELECT * FROM Win32_PhysicalMemory where Manufacturer='Corsair'");
			var corsairmemory = conn.Query<PhysicalMemory>(query);			
		}
	}
}

```


## License

See the [LICENSE](LICENSE.md) file for license rights and limitations (MIT)
