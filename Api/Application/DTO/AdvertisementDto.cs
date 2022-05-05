﻿namespace Api.Application.DTO;

public class AdvertisementDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string Seller { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTime PublishDate { get; set; }
}