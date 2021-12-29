using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaymentGateway.Checkout;

public class PaymentViewModelDropDownItemsProvider
{
    private static readonly SelectListItem[] _months = 
    {   new("01", "01"), 
        new("02", "02"),
        new("03", "03"),
        new("04", "04"),
        new("05", "05"),
        new("06", "06"),
        new("07", "07"),
        new("08", "08"),
        new("09", "09"),
        new("10", "10"),
        new("11", "11"),
        new("12", "12")
    };

    private const int YearItemsCount = 10;
    private static int _currentYear = DateTime.Now.Year;
    private static SelectListItem[] _years = GenerateYearItems(_currentYear, count: YearItemsCount);

    public IReadOnlyCollection<SelectListItem> AvailableMonths()
    {
        return Array.AsReadOnly(_months);
    }

    public IReadOnlyCollection<SelectListItem> AvailableYears()
    {
        if (_currentYear != DateTime.Now.Year)
        {
            _currentYear = DateTime.Now.Year;
            _years = GenerateYearItems(_currentYear, count: YearItemsCount);
        }

        return Array.AsReadOnly(_years);
    }

    private static SelectListItem[] GenerateYearItems(int startYear, int count)
    {
        var year = startYear;
        var items = new SelectListItem[count];
        for (var i = 0; i < count; i++)
        {
            var yearLastTwoDigits = (year - 2000).ToString();
            items[i] = new SelectListItem(yearLastTwoDigits, yearLastTwoDigits);
            year++;
        }
        return items;
    }
}