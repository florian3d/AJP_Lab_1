using Microsoft.AspNetCore.Mvc;
namespace LAB1.Controllers;

[ApiController]
// [Route("[controller]")]

public class MyClassController : ControllerBase
{
    private readonly ILogger<MyClassController> _logger;

    public MyClassController(ILogger<MyClassController> logger)
    {
        _logger = logger;
    }

    // http://localhost:5000
    [HttpGet("/")]
    public string Root()
    {
        return "Hello World!";
    }

    // http://localhost:5000/api/all
    [HttpGet("api/all")]
    public List<Item> GetAll()
    {
        return DataContainer.DataList.Where(d=> 1==1).ToList<Item>();
    }

    // Wyszukiwanie wszystkich wpisów dla konkretnego segmentu
    // http://localhost:5000/api/segment/Midmarket
    [HttpGet("api/segment/{segment}")]
    public List<Item> GetSegments(string segment)
    {
        return DataContainer.DataList.Where(d=>d.Segment.ToLower() == segment.ToLower()).ToList<Item>();
    }

    // Wyszukiwanie wszystkich wpisów dla konkretnego kraju
    // http://localhost:5000/api/country/France
    [HttpGet("api/country/{country}")]
    public List<Item> GetCountries(string country)
    {
        return DataContainer.DataList.Where(d=>d.Country.ToLower() == country.ToLower()).ToList<Item>();
    }

    // Wyszukiwanie wszystkich wpisów dla konkretnego produktu
    // http://localhost:5000/api/product/Paseo
    [HttpGet("api/product/{product}")]
    public List<Item> GetProducts(string product)
    {
        return DataContainer.DataList.Where(d=>d.Product.ToLower() == product.ToLower()).ToList<Item>();
    }

    // Metodę wyszukującą wybrany wpis z tabeli ???
    // http://localhost:5000/api/filter?country=Polska&segment=Zachód&product=Python&unitsold=1000000
    [HttpGet("api/filter")]
    public List<Item> GetFiltered(string segment, string country, string product)
    {
        return DataContainer.DataList.Where(d=>d.Segment.ToLower() == segment.ToLower() && d.Country.ToLower() == country.ToLower() && d.Product.ToLower() == product.ToLower()).ToList<Item>();
    }

    // Metodę, która pozwala usunąć wpis z tabeli po indeksie
    // http://localhost:5000/api/delete/guid/7c9e6679-7425-40de-944b-e07fc1f90ae7
    [HttpGet("api/delete/guid/{guid}")]
    public Boolean RemoveIndex(Guid guid)
    {
        Item? item = DataContainer.DataList.Find(d=>d.guid == guid);
        if(item != null){
            return DataContainer.DataList.Remove(item);
        } else {
            return false;
        }
    }

    // Metodę, która pozwala usunąć wpis z tabeli po obiekcie
    // http://localhost:5000/api/delete/items?country=Polska&segment=Zachód&product=Python&unitsold=1000000
    [HttpGet("api/delete/items")]
    public int RemoveItem(string segment, string country, string product, decimal unitSold)
    {
        return DataContainer.DataList.RemoveAll(new Predicate<Item>(i => i.Segment == segment && i.Country == country && i.Product == product && i.UnitSold == unitSold));
    }

    // Metodę, która pozwala dodać nowy wpis do tabeli
    // http://localhost:5000/api/add?country=Polska&segment=Zachód&product=Python&unitsold=1000000
    [HttpGet("api/add")]
    public Boolean AddItem(string segment, string country, string product, decimal unitSold)
    {
        if(segment != null && country != null && product != null){
            DataContainer.DataList.Add(new Item(segment, country, product, unitSold));
            return true;
        } else {
            return false;
        }
    }

    // Raport, który przedstawia zsumowane wartości Units Sold dla segmentu i kraju
    // http://localhost:5000/api/report?segment=Midmarket&country=France
    [HttpGet("api/report")]
    public Dictionary<string, object> Report(string segment, string country)
    {
        decimal sum = 0;
        List<Item> data = DataContainer.DataList.Where(d => d.Segment.ToLower() == segment.ToLower() && d.Country.ToLower() == country.ToLower()).ToList();
        sum = data.Sum(d => d.UnitSold);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("country", country);
        dict.Add("segment", segment);
        dict.Add("items", data);
        dict.Add("unitSoldSum", sum);
        return dict;
    }
}
