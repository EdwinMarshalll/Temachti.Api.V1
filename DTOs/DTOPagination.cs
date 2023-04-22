namespace Temachti.Api.DTOs;

public class DTOPagination
{
    public int Page { get; set; } = 1;
    public int _recordsPerPage = 10;
    public readonly int maxQuantityPerPage = 50;
    public int RecordsPerPage
    {
        get 
        {
            return _recordsPerPage;
        }
        set
        {
            _recordsPerPage = (value > maxQuantityPerPage ? maxQuantityPerPage : value);
        }
    }
}