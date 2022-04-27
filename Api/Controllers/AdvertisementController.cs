namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AdvertisementController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AdvertisementController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdvertisementDto>>> GetAdvertisements()
    {
        return Ok(_mapper.Map<AdvertisementDto>(await _context.Advertisements.ToListAsync()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdvertisementDto>> GetAdvertisement(int id)
    {
        var advertisement = _mapper.Map<AdvertisementDto>(await _context.Advertisements.FindAsync(id));

        if (advertisement == null) return NotFound();

        return Ok(advertisement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdvertisement(int id, AdvertisementDto advertisement)
    {
        if (id != advertisement.Id) return BadRequest();


        _context.Entry(advertisement).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AdExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }



    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchAdvertisement(int id, [FromBody] JsonPatchDocument<Advertisement> patchEntity)
    {
        var entity = await _context.Advertisements.FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null) return NotFound();


        patchEntity.ApplyTo(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }


  
    [HttpPost]
    public async Task<ActionResult<Advertisement>> PostAdvertisement(AdvertisementModel advertisement)
    {
        var newAd = new Advertisement() { Text = advertisement.Text, Title = advertisement.Title };
        _context.Advertisements.Add(newAd);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAdvertisement), new { id = newAd.Id }, advertisement);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdvertisement(int id)
    {

        var ad = await _context.Advertisements.FindAsync(id);

        if (ad == null) return NotFound();

        _context.Advertisements.Remove(ad);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AdExists(int id)
    {
        return _context.Advertisements.Any(e => e.Id == id);
    }
}