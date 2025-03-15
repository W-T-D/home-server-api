namespace HomeServer.Models.Files;

public class SearchFileRequestDto
{
    public string Filter { get; set; }
    
    public int Take { get; set; }

    public int Skip { get; set; }
}