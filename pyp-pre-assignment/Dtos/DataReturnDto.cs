namespace pyp_pre_assignment.Dtos
{
    public class DataReturnDto
    {
        public string? FilterName { get; set; }
        public int TotalCount { get; set; }
        public double Sale { get; set; }
        public double Discount { get; set; }
        public double Profit { get; set; }

        public int SaleCount { get; set; }
        public double SalesPrice { get; set; }
        public double DiscountSalesPrice { get; set; }
        public double COGS { get; set; }
        public int ProductCount { get; set; }
        public double DiscountPercentage { get; set; }

        public double Percentage(double salePrice,double discountPrice)
        {
            return salePrice / discountPrice;
        }
    }
}
