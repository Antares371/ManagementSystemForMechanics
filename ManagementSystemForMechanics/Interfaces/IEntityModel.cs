namespace ManagementSystemForMechanics.Interfaces
{
    public interface IEntityModel : ICreated, IModyfied
    {
        int Id { get; set; }
    }
}
