using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi
{
    public static class DataReaderExtension
    {
        public static bool ColumnExists(this DbDataReader dataReader, string columnName)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
                if (dataReader.GetName(i).ToLower() == columnName.ToLower())
                    return true;

            return false;
        }

        public static string GetString(this DbDataReader dataReader, string columnName)
        {

            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            try
            {
                int index = dataReader.GetOrdinal(columnName);

                if (dataReader.IsDBNull(index))
                    return null;

                return dataReader.GetString(index);
            }
            catch
            { return null; }
        }

        public static string GetStringNullToEmpty(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return String.Empty;

            return dataReader.GetString(index);
        }

        public static decimal GetDecimal(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return 0;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetDecimal(index);
        }

        public static decimal GetDecimal(this DbDataReader dataReader, string columnName, decimal nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetDecimal(index);
        }

        public static decimal? GetDecimalNullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetDecimal(index);
        }

        public static byte GetByte(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return byte.MinValue;

            //if index is -1 then GetByte should throw an exception
            return dataReader.GetByte(index);
        }

        public static byte GetByte(this DbDataReader dataReader, string columnName, byte nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            //if index is -1 then GetByte should throw an exception
            return dataReader.GetByte(index);
        }

        public static byte? GetByteNullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetByte(index);
        }

        public static short GetInt16(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return short.MinValue;

            //if index is -1 then GetInt16 should throw an exception
            return dataReader.GetInt16(index);
        }

        public static short GetInt16(this DbDataReader dataReader, string columnName, short nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            //if index is -1 then GetInt16 should throw an exception
            return dataReader.GetInt16(index);
        }

        public static short? GetInt16Nullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetInt16(index);
        }

        public static int GetInt32(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return int.MinValue;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetInt32(index);
        }

        public static int GetInt32(this DbDataReader dataReader, string columnName, int nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetInt32(index);
        }

        public static int? GetInt32Nullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetInt32(index);
        }

        public static long GetInt64(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return int.MinValue;

            return dataReader.GetInt64(index);
        }

        public static long GetInt64(this DbDataReader dataReader, string columnName, long nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            return dataReader.GetInt64(index);
        }

        public static long? GetInt64Nullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetInt64(index);

        }

        public static bool GetBoolean(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return false;

            return dataReader.GetBoolean(index);
        }
        public static bool GetBooleanExtra(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return false;

            var result=dataReader.GetInt32(index);
            return result == 1 ? true : false;
        }

        public static bool GetBoolean(this DbDataReader dataReader, string columnName, bool nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            return dataReader.GetBoolean(index);
        }

        public static bool? GetBooleanNullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetBoolean(index);
        }

        //===================

        public static DateTime GetDateTime(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return DateTime.MinValue;

            return dataReader.GetDateTime(index);
        }

        public static DateTime GetDateTime(this DbDataReader dataReader, string columnName, DateTime nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            return dataReader.GetDateTime(index);
        }

        public static DateTime? GetDateTimeNullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetDateTime(index);
        }

        public static Guid GetGuid(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return Guid.Empty;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetGuid(index);
        }

        public static Guid GetGuid(this DbDataReader dataReader, string columnName, Guid nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetGuid(index);
        }

        public static Guid? GetGuidNullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetGuid(index);
        }

        public static double GetDouble(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return double.MinValue;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetDouble(index);
        }

        public static double GetDouble(this DbDataReader dataReader, string columnName, Double nullDefault)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return nullDefault;

            //if index is -1 then GetInt32 should throw an exception
            return dataReader.GetDouble(index);
        }

        public static double? GetDoubleNullable(this DbDataReader dataReader, string columnName)
        {
            //=============================================================================
            //DO NOT catch exceptions in this method.
            //We DO want an exception to generate an error log entry and an error message.
            //=============================================================================

            //Throws Exception if column name does not exist
            int index = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(index))
                return null;

            return dataReader.GetDouble(index);
        }
    }
}