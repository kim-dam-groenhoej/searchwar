using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SearchValidate
/// </summary>

namespace SearchWar.SearchEngine.Validate {
    public class SearchValidate {

        public SearchValidate() {

        }

        public enum DateValidateMsg {
            Empty,
            FromDateIsSmallerThanDateTimeNow,
            FromTimeIsSmallerThanDateTimeNow,
            Valid
        }


        public bool ValidateClanContinent(int? value) {
            return value.HasValue;
        }

        public bool ValidateClanCountry(int? value) {
            return value.HasValue;
        }

        public bool ValidateSearchContinent(int? value) {
            return value.HasValue;
        }

        public bool ValidateGame(int? value) {
            return value.HasValue;
        }

        public bool ValidateGameType(int? value) {
            return value.HasValue;
        }

        public bool ValidateXvsX(int? value, int? value2) {
            return value2.HasValue && value.HasValue;
        }

        /// <summary>
        /// Vlidate date
        /// </summary>
        /// <param name="fromUtcDate">from datetime</param>
        /// <param name="toDate">to datetime (Can be null if you only want to validate with datetime now)</param>
        /// <param name="timeValidate">to validate time "00:00:00" then true</param>
        /// <returns>DateValidateMsg</returns>
        public DateValidateMsg ValidateDates(DateTime? fromUtcDate,
            bool timeValidate, 
            string currentUserIp) {

            DateTime dateTimeNow = TimeZoneManager.DateTimeNow;

            TimeZoneManager mngInfo = new TimeZoneManager(currentUserIp);

            
            fromUtcDate = fromUtcDate.Value;

            if (fromUtcDate.HasValue) {

                if (timeValidate == true) {

                   return fromUtcDate > new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, dateTimeNow.Hour, dateTimeNow.Minute, 0) ? DateValidateMsg.Valid : DateValidateMsg.FromTimeIsSmallerThanDateTimeNow;

                } else {

                        return fromUtcDate >= new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 0, 0, 0)
                                   ? DateValidateMsg.Valid
                                   : DateValidateMsg.FromDateIsSmallerThanDateTimeNow;

                }

            }  

            return DateValidateMsg.Empty;
        }

    }
}