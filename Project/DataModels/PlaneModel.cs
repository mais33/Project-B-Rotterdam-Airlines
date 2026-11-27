public class PlaneModel
{
    public long Id { get; set; }

    public string Name { get; set; } = "";

    public string Model { get; set; } = "";

    public PlaneModel(long id, string name, string model)
    {
        Id = id;
        Name = name;
        Model = model;
    }
}