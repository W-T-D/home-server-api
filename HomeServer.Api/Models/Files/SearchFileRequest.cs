using HomeServer.Models.Files;

namespace HomeServer.Api.Models.Files;

public class SearchFileRequest
{
    public string Filter { get; set; }
    
    public int Take { get; set; }

    public int Skip { get; set; }
    
    public static SearchFileRequestDto ToDto(SearchFileRequest request)
    {
        return new SearchFileRequestDto
        {
            Filter = request.Filter,
            Take = request.Take,
            Skip = request.Skip
        };
    }
}