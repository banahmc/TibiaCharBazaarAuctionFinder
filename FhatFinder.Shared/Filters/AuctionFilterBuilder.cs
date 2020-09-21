using FhatFinder.Shared.Extensions;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FhatFinder.Shared.Filters
{
    public static class AuctionFilterBuilder
    {
        public static string Build(IAuctionFilter auctionFilter)
        {
            var filterProperties = auctionFilter.GetType().GetPublicProperties();

            var filterQueryBuilder = new StringBuilder();

            foreach (var filterProp in filterProperties)
            {
                var displayAttribute = filterProp.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
                if (displayAttribute == null)
                {
                    continue;
                }

                var filterName = (displayAttribute as DisplayNameAttribute).DisplayName;
                var filterValue = string.Empty;

                var filterPropValue = filterProp.GetValue(auctionFilter, null);
                if (filterPropValue != null)
                {
                    if (filterProp.PropertyType.IsEnum)
                    {
                        filterValue = ((int)filterPropValue).ToString();
                    }
                    else
                    {
                        filterValue = filterPropValue.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(filterName))
                {
                    filterQueryBuilder.Append($"{filterName}={filterValue}&");
                }
            }

            return filterQueryBuilder.ToString().TrimEnd('&');
        }
    }
}
