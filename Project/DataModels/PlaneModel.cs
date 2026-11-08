public class PlaneModel
{
    public Int64 Id { get; set; }
    public string Name { get; set; } = "";
    public string Model { get; set; } = "";
    public int Capacity { get; set; }

    public PlaneModel()
    {
    }

    public PlaneModel(Int64 id, string name, string model, int capacity)
    {
        Id = id;
        Name = name;
        Model = model;
        Capacity = capacity;
    }
}
