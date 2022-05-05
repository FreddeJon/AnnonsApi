namespace Api.Application.Models;

public class AdvertisementModel
{
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(30)]
    public string Title { get; set; } = null!;
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(30)]
    public string Text { get; set; } = null!;
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(30)]
    public string Seller { get; set; } = null!;
    [Required(ErrorMessage = "{0} is required")]
    [Range(1, 10000000, ErrorMessage = "{0} must be between {1} and {2}")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    public DateTime PublishDate { get; set; }
}