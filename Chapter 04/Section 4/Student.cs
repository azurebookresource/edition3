using Newtonsoft.Json;
    
public class Student
{
	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }
	public string LastName { get; set; }
	public Address Address { get; set; }
	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}

public class Address
{
	public string State { get; set; }
	public string County { get; set; }
	public string City { get; set; }
}