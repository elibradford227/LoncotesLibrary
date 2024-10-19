namespace LoncotesLibrary.models;
public class MaterialDTO
{
  public int Id { get; set; }
  public string MaterialName { get; set; }
  public string MaterialTypeId { get; set; }
  public MaterialTypeDTO MaterialType { get; set; }
  public string GenreId { get; set; }
  public GenreDTO Genre { get; set; }
  public DateTime OutOfCirculationSince { get; set; }
}