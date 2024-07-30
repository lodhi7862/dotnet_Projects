using System.ComponentModel.DataAnnotations;

namespace task.Models;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [DataType(DataType.Date)]
    public DateTime DOB { get; set; }
    public string? Cource { get; set; }
    public string? ImageFilename { get; set; }
}