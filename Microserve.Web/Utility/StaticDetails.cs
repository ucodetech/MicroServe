namespace Microserve.Web.Utility
{
    public class StaticDetails {
        public static string CouponAPIBase { get; set; } // this store the base url of the coupon api 
        public static string AuthAPIBase { get; set; } // this store the base url of the auth api 
        public static string LocatorAPIBase { get; set; } // this store the base url of the auth api 

        public const string RoleAdmin="ADMIN"; //ROLE ADMIN 
        public const string RoleCustomer="CUSTOMER"; //ROLE CUSTOMER
        public const string CookieToken="JwtToken"; //ROLE CUSTOMER

      
        public enum ApiType {
            GET, POST, PUT, DELETE

        }
    }
}
