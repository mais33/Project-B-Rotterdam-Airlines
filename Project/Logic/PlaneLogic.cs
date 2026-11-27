public class PlaneLogic
{
    private PlaneAccess planeAccess = new();
    public List<PlaneModel> GetAllPlanes()
    {
        return planeAccess.GetAllPlanes();
    }

    public PlaneModel GetPlaneById(long id)
    {
        return planeAccess.GetById(id); 
    }

    public void UpdatePlaneName(PlaneModel plane)
    {
        planeAccess.UpdatePlaneName(plane);
    }

    public void UpdatePlaneModel(PlaneModel plane)
    {
        planeAccess.UpdatePlaneModel(plane);
    }

    
}