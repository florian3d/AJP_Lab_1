using OfficeOpenXml;
public class Item{

    public string Segment {get;set;} = "";
    public string Country {get;set;} = "";
    public string Product {get;set;} = "";
    public decimal UnitSold {get;set;} = 0;
    public Guid guid{get;set;}
    public Item(string Segment, string Country, string Product, decimal UnitSold){

        this.Segment = Segment;
        this.Country = Country;
        this.Product = Product;
        this.UnitSold = UnitSold;
        this.guid = Guid.NewGuid();
    }
}

public sealed class DataContainer{

    public static List<Item> DataList {get;set;} = _read();
    private static readonly object padlock = new object();

    private static DataContainer? _instance = null;
    public static DataContainer Instance{
        get{
            lock(padlock){
                if (_instance == null)
            {
                _instance = new();
            }
            return _instance;
            }
        }
    }

    private static List<Item> _read(){
        List<Item> _dataList = new List<Item>();
        using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo("sample-xlsx-file-for-testing.xlsx")))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
            int rows = worksheet.Dimension.End.Row;
            for (int row = 2; row <= rows; row++)
            {
                string? segment = worksheet.Cells[row, 1].Value.ToString();
                string? country = worksheet.Cells[row, 2].Value.ToString();
                string? product = worksheet.Cells[row, 3].Value.ToString();
                decimal unitSold = Convert.ToDecimal(worksheet.Cells[row, 5].Value);

                if(segment != null && country != null && product != null){
                    _dataList.Add(new Item(segment, country, product, (decimal) unitSold));
                }                
            }
        }
        return _dataList;
    }
}
