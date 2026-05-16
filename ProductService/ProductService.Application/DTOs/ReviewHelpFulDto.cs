namespace ProductService.Application.DTOs;

public class ReviewHelpFulDto
{
    public long ReviewHelpId { get; set; }
    public int Helpful { get; set; }
    public int NotHelpFul { get; set; }
    public int Reported { get; set; }
}
