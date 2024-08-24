namespace Api.Shared.Models;

public class BasePageableModel<T>
{
    public int Index { get; set; }
    public int Size { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
    public IList<T>? Items { get; set; }
}
