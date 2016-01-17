using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarGo.Model.Helpers
{
    public class CommonDataHelper
    {
        public CommonDataHelper(CarGoDbDataContext carGoDbDataContext)
        {
            this.Db = carGoDbDataContext;
        }

        #region Properties & Vars

        public static CommonDataHelper GlobalInstance { get;  private set; }

        public CarGoDbDataContext Db { get; set; }

        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<CarBrand> CarBrands { get; set; }
        public IEnumerable<CarLoadingType> CarLoadingTypes { get; set; }
        public IEnumerable<CarModel> CarModels { get; set; }
        public IEnumerable<CarType> CarTypes { get; set; }
        public IEnumerable<FreightType> FreightTypes { get; set; }
        public IEnumerable<GeoRegion> GeoRegions { get; set; }
        public IEnumerable<GeoCity> GeoCities { get; set; }

        #endregion //Properties & Vars


        public static void SetGlobalInstance(CarGoDbDataContext carGoDbDataContext)
        {
            GlobalInstance = new CommonDataHelper(carGoDbDataContext);
        }


        #region Locating

        /// <summary>
        /// Поиск регионов
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<GeoRegion> SearchRegions(string query, int rowsCount)
        {
            var splitter = query.Split(' ');
            List<GeoRegion> regions = Db.GeoRegion.Where(r=>r.Region.Contains(query)).ToList();
            return ReversSearchResults(query, regions, rowsCount);
        }

        /// <summary>
        /// Поиск городов в регионе
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<GeoCity> SearchCities(int regionId, string query, int rowsCount)
        {
            return Db.GeoCity.Where(c => c.City.StartsWith(query) && c.RegionId == regionId)
                .Take(rowsCount).ToList();
        }

        /// <summary>
        /// Поиск улиц в городе
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="query"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public List<GeoStreet> SearchStreets(int cityId, string query, int rowsCount)
        {
            return Db.GeoStreet.Where(s => s.Street.StartsWith(query) && s.CityId == cityId).Take(rowsCount).ToList();
            //return (from s in Db.GeoStreet
            //        where s.Street.StartsWith(query) && s.CityId == cityId
            //        select new locStreetDTO { Street = s.Street, Id = s.Id, StreetType = s.StreetType }).Take(rowsCount).ToList();
        }

        /// <summary>
        /// Универсальный метод для перестановки слов в результатах поиска - позволяет получать предсказуемый ответ при вводе "Башкорк..." (в БД как "Республика Башкоркостан"). Можно конечно поменять данные в БД, но это уменьшит универсальность и, возможную, достоверность базы
        /// </summary>
        /// <typeparam name="T">GeoRegion (GeoCity|GeoStreet)</typeparam>
        /// <param name="searchQuery"></param>
        /// <param name="searchResults"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public List<T> ReversSearchResults<T>(string searchQuery, List<T> searchResults, int rowsCount) where T : new()
        {
            var value = string.Empty;
            var regexpPattern = string.Format("\\b([а-я\\-]*?{0}\\S*)", searchQuery);
            var indexer = new int();
            
            if (typeof(T) == typeof(GeoRegion))
            {
                var returnList = new List<GeoRegion>();

                foreach (var one in searchResults as List<GeoRegion>)
                {

                    var revertedText = IsStartWith(searchQuery, one.Region) ? one.Region : RevertText(searchQuery, one.Region);
                    if (IsStartWith(searchQuery, revertedText))
                    {
                        if (indexer < rowsCount)
                        {
                            returnList.Add(new GeoRegion
                            {
                                Id = one.Id,
                                Region = revertedText,
                                OriginalRegion = one.Region
                            });
                            indexer++;
                        }
                    }
                }

                return returnList as List<T>;
            }

            return null;
        }

        internal static string RevertText(string query, string rowValue)
        {
            var splitter = rowValue.Split(' ');
            var val = new StringBuilder();

            if (splitter.Length > 1)
            {
                foreach (var one in splitter)
                {
                    if (IsStartWith(query, one))
                    {
                        val.Insert(0, one.Trim());
                        val.Insert(one.Length, " ");
                    }
                    else
                    {
                        val.Append(one.ToLower().Trim());
                        val.Append(" ");
                    }

                }
            }
            else val.Append(splitter[0]);

            return val.ToString().Trim();
        }

        internal static bool IsStartWith(string startString, string text)
        {
            return Regex.IsMatch(text, "^" + startString, RegexOptions.IgnoreCase);
        }

        #endregion //Locating
    }
}
